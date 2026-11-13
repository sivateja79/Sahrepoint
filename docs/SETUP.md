# Setup checklist — follow actual component branching model

This personal learning repository mirrors the **visible** Allied World SDMS API workflow structure. It is not an official company template and contains no company code.

## Code and branches
- `src/Sample.Api` = original .NET 10 sample API
- `tests/Sample.Api.Tests` = unit tests
- `.github/workflows/api.yml` = CI and 3 deployment caller jobs
- `.github/workflows/_deploy-component.yml` = reusable deploy workflow
- Branches: `main`, `dev/api`, `qa/api`, `prod/api`; story PRs from `dev/api`.

## Branch policies (not yet active)
Go to **Settings > Rules > Rulesets > New branch ruleset**:
- Target `main`, `dev/api`, `qa/api`, `prod/api` (separate or one target ruleset)
- Require PR before merge; require 1 reviewer (requires eligible collaborator)
- Require conversations resolved and forbid force-push/deletion
- Choose required CI check after first run, based on exact GitHub check names
- Where supported, restrict QA/PROD promotion approvals to authorized release reviewers

Check the ruleset's enforcement state. Your GitHub plan may affect private/public repository protection features.

## GitHub environments (not yet active)
Go to **Settings > Environments** and create:
- `api-dev`
- `api-qa` — required reviewer if available
- `api-prod` — required reviewer and prevent self-review if supported

For each environment, configure variables (not passwords):
- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`
- `APP_NAME`
- Optional `SDMS_SMOKE_BASE_URL`

These names are used by the reusable workflow. If `AZURE_CLIENT_ID` is missing, it **skips Azure deployment with a notice**, like the observed project pattern; check step logs, do not mistake green status for an Azure deployment.

## Azure OIDC
Create appropriate App Services in your own Azure subscription and an Entra OIDC identity with minimum required RBAC. Configure GitHub environment subject identifiers:
- `repo:sivateja79/Sahrepoint:environment:api-dev`
- `repo:sivateja79/Sahrepoint:environment:api-qa`
- `repo:sivateja79/Sahrepoint:environment:api-prod`

Never add passwords, personal access tokens, Zscaler session URLs or corporate secrets.

## Practice flow
1. Start from `dev/api`, create `story/api/JIRA-123`, change sample source, open PR into `dev/api`.
2. Observe .NET CI; merge and check the `api-dev` deployment steps.
3. Raise `dev/api` -> `qa/api` PR, review, merge; confirm fresh QA build and `api-qa` target.
4. Raise `qa/api` -> `prod/api` PR, review, merge; confirm fresh PROD build and `api-prod` target.
5. Review the produced GitHub Actions artifact, name `api-<commitSHA>`, retention 30 days.

## Current limitations
Branch rulesets, environment protection and Azure services **have not been configured via this connection**. Only Git branches, YAML and sample application were added. The original project's true security and rollback implementation cannot be inferred beyond shown screenshots.
