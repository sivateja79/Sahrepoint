# SDMS DevOps Practice

Personal training repository with original sample code — **not** copied from any employer's private repository.

## Contents
- `.NET 10` sample Web API (`src/Sample.Api`)
- xUnit tests (`tests/Sample.Api.Tests`)
- GitHub Actions CI (`.github/workflows/ci.yml`)
- Build-once DEV → QA → PROD release pipeline (`.github/workflows/release.yml`)
- Retained-artifact manual rollback (`.github/workflows/rollback.yml`)
- Branch governance, Azure OIDC, environment approval instructions (`docs/SETUP.md`)

Start locally with `dotnet run --project src/Sample.Api` and visit `/health` at the localhost address printed by dotnet.

**Note:** This API uses in-memory sample data. It does not access SharePoint, Microsoft Graph, or production systems. CI works immediately; Azure deployments require your own App Services and configured GitHub environments.
