# SharePoint DMS CI/CD Practice — screenshot-based recreation

This is a **personal educational simulation** of the Allied World SDMS CI/CD layout based on the screenshots shared in chat. All example .NET code is original; no private corporate source code is included.

## What matches the documented project structure
- One monorepo and component-specific environment branches: `dev/api`, `qa/api`, `prod/api`
- Story branches based on a component DEV branch: `story/api/JIRA-123`
- PRs: story → DEV, DEV → QA, QA → PROD, PROD → main
- `.github/workflows/api.yml` — component CI, .NET 10 build/test, publish and 30-day per-run GitHub build artifact
- `.github/workflows/_deploy-component.yml` — reusable deployment workflow; deployment kind, GitHub environment, artifact download, Azure OIDC and conditional Azure deployment
- Separate rebuild for each environment (as shown by your actual project YAML)

## Learn and run
- [Project pipeline guide](docs/PROJECT_PIPELINE.md)
- [Branching and governance](docs/BRANCHING.md)
- Run API locally: `dotnet run --project src/Sample.Api`
- Run tests: `dotnet test tests/Sample.Api.Tests/Sample.Api.Tests.csproj`

## What's not yet configured
- GitHub environment approval protections / rulesets: need to be enabled in GitHub Settings and may require plan support
- Azure App Services, OIDC credentials, and environment variables: need your personal Azure setup
- Rollback: **not included** because the shared project workflow screenshots did not show an actual rollback implementation
- Other service-specific workflows cannot be copied accurately without their YAML; sample only covers the `api` component

The sample API keeps a temporary in-memory document registry. It does **not** upload actual files, connect to Microsoft Graph or SharePoint, or contain any employer IP.
