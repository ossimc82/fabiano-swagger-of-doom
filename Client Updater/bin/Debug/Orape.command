#!/bin/bash
ABSPATH=$(cd "$(dirname "$0")"; pwd)
cd "$ABSPATH"
chmod +x abcexport
chmod +x abcreplace
chmod +x rabcasm
chmod +x rabcdasm
/Library/Internet\ Plug-Ins/JavaAppletPlugin.plugin/Contents/Home/bin/java -jar "$ABSPATH"/Orape.jar