#!/usr/bin/env bash
# Sincroniza dev con lo último de main (incluyendo el commit de bump de versión).
set -euo pipefail

git fetch origin dev
git checkout dev
git merge --no-edit origin/main
git push origin dev
