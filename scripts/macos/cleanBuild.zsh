#!/bin/zsh

time (
  dotnet clean
  rm -R NetVampiro/obj NetVampiro/bin
  dotnet build 
) 
