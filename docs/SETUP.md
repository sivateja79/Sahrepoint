# Setup guide — personal SDMS DevOps practice

This repository is a **learning environment** with original sample code, not company IP.

## 1. Run locally
Install .NET 10 SDK, then run:

```bash
dotnet restore tests/Sample.Api.Tests/Sample.Api.Tests.csproj
dotnet test tests/Sample.Api.Tests/Sample.Api.Tests.csproj
dotnet run --project src/Sample.Api
```

Open the locally reported URL plus `/health`. The sample has GET/POST `/api/documents` endpoints using in-memory storage only.

## 2. Git branching and PR rules (GitHub web UI)
Use `main` as the **only long-lived code branch**. Work on `feature/<component>/<ticket>` branches. Open pull requests into main. Deploy each component independently by workflow, not by environment-specific Git branches.

Repository > Settings > Rules > Rulesets > New branch ruleset:
- Target: default branch `main`
- Require pull request before merging
- Require 1 approving review (for practice, requires another collaborating GitHub account to approve)
- Dismiss stale approvals (recommended)
- Require conversation resolution
- Block force pushes and branch deletion
- Require status checks: after the first CI run, choose `Build and test .NET 10` (check the actual GitHub check name)
- Require branches up to date before merging (optional; consider merge queue at higher volume)

**Important:** Ruleset enforcement may be limited on a public personal repository depending on GitHub plan. Check the ruleset status in GitHub UI. As solo owner, requiring one approval can block your own merges. Don't activate mandatory reviewer approval until you have a second reviewer.

## 3. Create GitHub environments
Repository > Settings > Environments:
- `dev` — no deployment review required
- `qa` — require reviewers when the plan permits
- `prod` — require reviewers and prevent self-review if supported

If required reviewers are unavailable on your plan, use manual release initiation and documented approval procedures; do not claim GitHub is enforcing them.

Create variables in **each environment**:
- `AZURE_CLIENT_ID` — OIDC federated application's client ID
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`
- `APP_NAME` — target Azure App Service name for that environment
- `APP_URL` — target base URL like `https://<your-app>.azurewebsites.net`

Do **not** store passwords or publish profiles in the repository.

## 4. Azure resources and OIDC
Create three distinct Azure App Services (and plans as appropriate) for practice in **your personal Azure subscription**. Use .NET 10 supported runtime and suitable OS. Create an Entra application/service principal with least-privilege RBAC scoped to appropriate resources. Configure **federated identity credentials** for GitHub environment subjects:

```
repo:sivateja79/Sahrepoint:environment:dev
repo:sivateja79/Sahrepoint:environment:qa
repo:sivateja79/Sahrepoint:environment:prod
```

Ensure GitHub environment names match these exactly. Test Azure OIDC login independently before running a release. Never paste Azure secrets into chat.

## 5. Run CI and deployment
- `API CI` runs on PR and push to `main`.
- `API release - build once, deploy DEV QA PROD` is **manual**. Only start it after environment variables, Azure permissions and resources exist.
- The release workflow builds and tests exactly **once**, uploads `sample-api` artifact (90-day retention requested), and deploys the same artifact to DEV, then QA, then PROD. The workflow waits on required approvals if environment protection rules are supported/configured.
- Azure configuration is validated; missing variables cause a clear **failure**, not a misleading skipped green deployment.
- Review each environment's run logs and `/health` smoke check.

## 6. Manual rollback
In Actions, run `Manual rollback to retained release artifact`. Provide a previous successful **release workflow run ID** and target environment. It fetches the old `sample-api` artifact and redeploys it; no rebuild occurs. Rollback depends on GitHub artifact availability and retention. For real production, use long-lived immutable release storage and a release manifest as well. Database and external state changes are **not** undone by app rollback.

## 7. Caveats
- No live SharePoint/Graph/APIM/Bicep deployed here — only a sample API and a ready-to-configure CI/CD example.
- The in-memory document registry is not persistent, and does not upload files.
- GitHub repository rulesets / environment required reviewers are configured in GitHub Settings, not by the workflows.
- Avoid mixing Azure deployment credentials into PR code. Protect `main` and environments before real deployments.
