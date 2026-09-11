#!/usr/bin/env bash
# Calcula la nueva versión a partir del bump indicado (major|minor|patch).
# Lee la versión actual de common.props y angular/package.json (deben coincidir)
# y sube en 1 solo el número correspondiente (no resetea los inferiores).
# Si $GITHUB_OUTPUT existe, escribe ahí current=<x> y new=<y>.
# Uso: compute-version.sh <major|minor|patch>
set -euo pipefail

BUMP="${1:?Uso: compute-version.sh <major|minor|patch>}"

CURRENT=$(grep -oP '(?<=<Version>)[0-9]+\.[0-9]+\.[0-9]+(?=</Version>)' common.props)
PKG=$(jq -r .version angular/package.json)

if [ "$CURRENT" != "$PKG" ]; then
  echo "::error::Version desincronizada antes del bump: common.props=$CURRENT angular/package.json=$PKG" >&2
  exit 1
fi

IFS='.' read -r MAJOR MINOR PATCH <<< "$CURRENT"
case "$BUMP" in
  major) MAJOR=$((MAJOR + 1));;
  minor) MINOR=$((MINOR + 1));;
  patch) PATCH=$((PATCH + 1));;
  *)
    echo "::error::Bump desconocido: $BUMP (esperado major, minor o patch)" >&2
    exit 1
    ;;
esac
NEW="$MAJOR.$MINOR.$PATCH"

echo "Bump: $BUMP | $CURRENT -> $NEW" >&2

if [ -n "${GITHUB_OUTPUT:-}" ]; then
  echo "current=$CURRENT" >> "$GITHUB_OUTPUT"
  echo "new=$NEW" >> "$GITHUB_OUTPUT"
fi
