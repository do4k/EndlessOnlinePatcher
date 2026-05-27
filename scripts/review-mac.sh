#!/usr/bin/env bash
set -euo pipefail

# Downloads the latest eopatcher.exe from GitHub Releases and opens it under Wine.
# Requires Wine: brew install --cask wine-stable

REPO="do4k/EndlessOnlinePatcher"
EXE_DIR="$(dirname "$0")/../.review"
EXE="$EXE_DIR/eopatcher.exe"

# --- Check Wine ---
if command -v wine64 &>/dev/null; then
    WINE="wine64"
elif command -v wine &>/dev/null; then
    WINE="wine"
else
    echo "Wine not found. Install it with:"
    echo "  brew install --cask wine-stable"
    exit 1
fi

mkdir -p "$EXE_DIR"

# --- Download latest release ---
if command -v gh &>/dev/null; then
    echo "Downloading latest eopatcher.exe via gh CLI..."
    gh release download --repo "$REPO" --pattern "eopatcher.exe" --output "$EXE" --clobber
else
    echo "gh CLI not found, falling back to curl..."
    URL=$(curl -fsSL "https://api.github.com/repos/$REPO/releases/latest" \
        | grep '"browser_download_url"' \
        | grep 'eopatcher\.exe' \
        | cut -d '"' -f 4)
    if [[ -z "$URL" ]]; then
        echo "No release found. Push to main to trigger a CI build first."
        exit 1
    fi
    curl -fSL -o "$EXE" "$URL"
fi

echo "Launching under $WINE..."
"$WINE" "$EXE"
