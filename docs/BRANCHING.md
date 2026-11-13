# Branching and Governance — project screenshot recreation

## Long-lived branches for the API component
- `dev/api` — PR and branch push CI, DEV deployment
- `qa/api` — PR and branch push CI, QA deployment
- `prod/api` — PR and branch push CI, PROD deployment
- `main` — shared/platform integration and post-release sync

## Story and promotion flow
1. Create `story/api/JIRA-123` **from `dev/api`**; submit a PR into `dev/api`.
2. Once approved and merged, `api.yml` runs on push, builds and tests, then calls `_deploy-component.yml` with `environment: dev`.
3. Open a promotion PR `dev/api` -> `qa/api`, get approval, merge. QA branch rebuilds its own package, then QA deployment runs.
4. Open a promotion PR `qa/api` -> `prod/api`, get approval, merge. PROD branch rebuilds its own package, then PROD deployment runs.
5. Post-release, sync `prod/api` -> `main`. Review any platform changes `main` -> DEV before merging.

## GitHub rulesets — configure manually
GitHub Settings > Rules > Rulesets. Target `dev/api`, `qa/api`, `prod/api` and `main`. Require PR, prevent force pushes/deletion, and require green `component-ci` or the actual check name after workflow execution. For QA and PROD add approval policies (subject to account plan and available reviewers).

GitHub Settings > Environments: create `api-dev`, `api-qa`, `api-prod` and set reviewer protection on QA/PROD where supported. Note: a solo repository owner cannot provide independent PR approval; if no eligible reviewer exists, policy may block merging.

**Status:** Documentation only; rulesets and environments were not created by the connected GitHub API tools.

## Important tradeoffs
The observed workflow **rebuilds per environment** and retains artifacts 30 days. We reproduced that instead of designing an alternative. A tested immutable promotion and automated rollback mechanism are **not** evidenced by the provided YAML and are not claimed here.
