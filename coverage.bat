rmdir Core.Tests\TestResults /s /q & ^
dotnet test Core.Tests --no-restore --collect:"XPlat Code Coverage" --settings Core.Tests\coverage.runsettings && ^
dotnet reportgenerator -reports:"*.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:reports\coverage -historydir:reports\coveragehistory
