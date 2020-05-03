#!/bin/sh

input="main.tex"
output="README.md"

set -e
set -x

tmp=$(tempfile)
trap "rm -f -- '$tmp'" EXIT


# TODO \maketitle, abstract, citations
# - output format is just markdown, not gfm (github flavoured markdown, which is
#   based on commonmark) in order to output math wrapped in dollars
# - explicitly set -raw_tex due to pandoc issue #4527
# - disable footnotes for better compatibility with github markdown
# - standalone is to emit abstract and title as YAML metadata
pandoc -f latex -t markdown-raw_tex-footnotes --standalone --atx-headers "$input" > "$tmp"

# note that readme2tex apparently has a bug where it skips math
# that is indented due to appearing in a markdown list
# there also seems to be a bug where inline math that follows display math in
# the same paragraph is output as display math. as a simple workaround we can
# start a new paragraph with \noident
python3 -m readme2tex --nocdn --output "$output" "$tmp"

rm -f -- "$tmp"
trap - EXIT

git add "$output" svgs/

git status
