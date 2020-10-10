# CODEME

## Development hacks

    # check version in VampireApi using projects
    tail -c 1740 bin/Debug/netcoreapp3.1/NetVampiro.dll 
    
    # replace version in VampireApi using project (build,copy,run)
    dotnet build
    cp NetVampiro/bin/Debug/netcoreapp3.1/NetVampiro.* ./bin/Debug/netcoreapp3.1
    dotnet run --no-build
    
    