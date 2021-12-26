#! /bin/bash

cd src
dotnet publish -c Release
vercel --prod bin/Release/net6.0/publish/wwwroot/
