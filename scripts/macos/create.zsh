#!/bin/zsh

# create clean CSharp classlib project
dotnet new classlib --language C# --name NetVampiro --output $PWD/NetVampiro
rm $PWD/NetVampiro/Class1.cs

# create solution with needed projects
dotnet new sln --name NetSpider --output $PWD
dotnet sln add NetVampiro

