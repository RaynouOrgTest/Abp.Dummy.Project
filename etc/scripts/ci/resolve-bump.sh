#!/usr/bin/env bash
# Lee la label release/major|minor|patch de la PR (JSON de labels en $LABELS_JSON,
# formato github.event.pull_request.labels) y valida que haya exactamente una.
# Imprime el tipo de bump (major|minor|patch) a stdout. Falla con exit 1 si no
# hay ninguna o hay más de una.
set -euo pipefail

: "${LABELS_JSON:?LABELS_JSON es requerido (JSON array de labels de la PR)}"

MATCHES=$(echo "$LABELS_JSON" | jq -r '[.[].name | select(test("^release/(major|minor|patch)$"))]')
COUNT=$(echo "$MATCHES" | jq 'length')

if [ "$COUNT" -ne 1 ]; then
  echo "::error::Se esperaba exactamente una label release/major, release/minor o release/patch en la PR (encontradas: $COUNT)." >&2
  exit 1
fi

echo "$MATCHES" | jq -r '.[0]' | sed 's#release/##'
