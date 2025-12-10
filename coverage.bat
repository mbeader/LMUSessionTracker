rmdir LMUSessionTrackerCore.Tests\TestResults /s /q & ^
dotnet test LMUSessionTrackerCore.Tests --no-restore --collect:"XPlat Code Coverage" --settings LMUSessionTrackerCore.Tests\coverage.runsettings && ^
dotnet reportgenerator -reports:"*.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:reports\coverage -historydir:reports\coveragehistory
