#!/bin/sh
echo -ne '\033c\033]0;WallE\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/WallE.x86_64" "$@"