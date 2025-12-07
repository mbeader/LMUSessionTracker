dotnet test LMUSessionTrackerCore.Tests --no-restore -- --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml && ^
dotnet reportgenerator -reports:"*.Tests\bin\Debug\net8.0\TestResults\coverage.cobertura.xml" -targetdir:reports\coverage -historydir:reports\coveragehistory
