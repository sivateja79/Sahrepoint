# Branching and release policy (practice)

## Branches
- `main` — protected, long-lived integration branch
- `feature/<component>/<ticket>` — temporary development branches
- `hotfix/<component>/<ticket>` — temporary urgent fix branches

Do not use separate dev/qa/prod Git branches. Environments are deployment targets with approval gates.

## Pull requests
Open a feature or hotfix PR into `main`. Require green CI, review and resolved conversations. Prefer squash merge. Confirm shared components and dependencies before merging.

## Releases
From an approved `main` commit, trigger the release workflow manually. This builds one immutable artifact per release run. DEV → QA → PROD deploy the identical artifact after required environment approvals, with smoke tests after each stage.

## Rollback
Use the rollback workflow with the run ID of the last known good release. Re-deploy its retained artifact to the selected environment, confirm health, and open a post-incident review. This does not revert database migrations, SharePoint changes or external state.

## Ruleset status
Rulesets must be configured and confirmed in GitHub Settings. This document alone does not activate or enforce protection; account plan may limit enforcement.
