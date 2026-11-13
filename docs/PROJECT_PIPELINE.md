# Allied World-inspired CI/CD practice — API component

These files reproduce the **observable pipeline structure** from the GitHub screenshots for educational practice, using **our own sample .NET 10 API**. This is not a copy of the private code and not an assertion that unseen parts are identical.

## Branch strategy reflected from screenshot
- `main` platform/shared changes
- `story/api/JIRA-123` created from `dev/api` for an API story, PR into `dev/api`
- `dev/api` -> `qa/api` via PR + review
- `qa/api` -> `prod/api` via PR + review
- `prod/api` -> `main` post-release sync
- Separate per-environment builds; no immutable artifact promotion between environments.

## Workflows
- `.github/workflows/api.yml`: PR checks and branch push; restore/build/test, publish `src/Sample.Api`, upload `api-${{ github.sha }}` artifact with 30-day retention, then calls deployment reusable workflow.
- `.github/workflows/_deploy-component.yml`: `workflow_call`, `component`, `environment`, `kind`, `artifact` inputs, GitHub environment `api-dev`/`api-qa`/`api-prod`, download artifact, Azure OIDC, conditional app type deployment, optional smoke test.
- The original project-specific `src/api/SDMS.Api.csproj` is replaced by our own `src/Sample.Api/Sample.Api.csproj`, since corporate code must not be copied.
- Action versions and smoke-test code here are **safe practice equivalents**, not guaranteed exact original versions.

## GitHub configuration to do in Settings
1. Settings > Environments: create `api-dev`, `api-qa`, `api-prod`. Add required reviewer(s) to QA/PROD if supported by plan.
2. Add environment vars `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`, `APP_NAME`; optional `SDMS_SMOKE_BASE_URL`. The deploy code mirrors the original conditional skip when `AZURE_CLIENT_ID` is unset, so green GitHub status **does not guarantee Azure deployment**.
3. Configure Azure Entra OIDC federated subjects: `repo:sivateja79/Sahrepoint:environment:api-dev`, `api-qa`, `api-prod` as separate credentials / appropriate identities.
4. Settings > Rules > Rulesets: protect `dev/api`, `qa/api`, `prod/api` (PR reviews; prevent force push/deletion), and protect `main`. After first run select actual required checks based on check names. QA/PROD additionally use release reviewer restrictions.
5. If you are the only collaborator, mandatory approvals can block promotion. Add another authorized reviewer before enforcing.
6. Start with PR `story/api/JIRA-123` -> `dev/api`. No Azure subscription is needed to practice build/test/artifact paths.

## What is NOT implemented
- Branch rulesets and protected environments: this GitHub integration cannot configure those settings directly.
- Azure resources: not created; only deployment workflow definitions.
- Automatic rollback: not in the screenshot-based deployment pipeline.
- Additional component pipelines: only API is replicated until their exact workflows are provided.

This reproduction deliberately keeps the observed **rebuild per environment** behavior (not the previously proposed build-once design).
