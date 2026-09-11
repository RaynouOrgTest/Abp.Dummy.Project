#!/usr/bin/env bash
# Commitea el bump de versión en main, lo taguea vX.Y.Z y pushea ambos.
# Uso: commit-and-tag.sh <new-version>
set -euo pipefail

NEW="${1:?Uso: commit-and-tag.sh <new-version>}"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add common.props angular/package.json
git commit -m "chore(release): v${NEW} [skip ci]"
git tag "v${NEW}"
git push origin main
git push origin "v${NEW}"
