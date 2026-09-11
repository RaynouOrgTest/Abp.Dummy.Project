#!/usr/bin/env bash
# Escribe la nueva versión en common.props y angular/package.json.
# Uso: bump-version-files.sh <current-version> <new-version>
set -euo pipefail

CURRENT="${1:?Uso: bump-version-files.sh <current-version> <new-version>}"
NEW="${2:?Uso: bump-version-files.sh <current-version> <new-version>}"

sed -i "s#<Version>${CURRENT}</Version>#<Version>${NEW}</Version>#" common.props

jq --indent 2 --arg v "$NEW" '.version = $v' angular/package.json > angular/package.json.tmp
mv angular/package.json.tmp angular/package.json
