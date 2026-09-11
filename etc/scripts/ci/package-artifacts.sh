#!/usr/bin/env bash
# Empaqueta los publish de backend, dbmigrator y frontend en zips con el
# patrón Abp.Dummy.Project-<componente>-v<version>.zip dentro de release-assets/.
# Espera que ya existan artifacts/backend, artifacts/dbmigrator (dotnet publish)
# y angular/dist/Project (ng build) en el directorio de trabajo.
# Uso: package-artifacts.sh <version>
set -euo pipefail

VERSION="${1:?Uso: package-artifacts.sh <version>}"
: "${GITHUB_WORKSPACE:=$(pwd)}"

mkdir -p release-assets

FRONTEND_DIST=angular/dist/Project/browser
[ -d "$FRONTEND_DIST" ] || FRONTEND_DIST=angular/dist/Project

(cd artifacts/backend && zip -qr "$GITHUB_WORKSPACE/release-assets/Abp.Dummy.Project-backend-v${VERSION}.zip" .)
(cd artifacts/dbmigrator && zip -qr "$GITHUB_WORKSPACE/release-assets/Abp.Dummy.Project-dbmigrator-v${VERSION}.zip" .)
(cd "$FRONTEND_DIST" && zip -qr "$GITHUB_WORKSPACE/release-assets/Abp.Dummy.Project-frontend-v${VERSION}.zip" .)

ls -lh release-assets
