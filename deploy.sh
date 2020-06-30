#!/bin/bash

DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)


dotnet publish

cf push Formation.SpringCloud.Client     -f ./ci/manifest.yml -p ./Formation.SpringCloud.Client/bin/Debug/netcoreapp3.1/publish --random-route # random-route is here for dev purpose

