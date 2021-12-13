.DEFAULT_GOAL := run
csprojLoc := $(CURDIR)/Moduit.Interview/Moduit.Interview/Moduit.Interview.csproj

run :
	dotnet run --project $(csprojLoc)

watch :
	dotnet watch --project $(csprojLoc) run

