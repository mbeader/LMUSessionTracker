rmdir Core.Tests\TestResults /s /q & ^
rmdir Server.Tests\TestResults /s /q & ^
rmdir Web\coverage /s /q & ^
dotnet test Core.Tests --no-restore --collect:"XPlat Code Coverage" --settings Core.Tests\coverage.runsettings && ^
dotnet test Server.Tests --no-restore --collect:"XPlat Code Coverage" --settings Server.Tests\coverage.runsettings && ^
pushd Web && ng test --watch false --coverage --coverage-reporters html && popd && ^
dotnet reportgenerator -reports:"*.Tests\TestResults\*\coverage.cobertura.xml" -targetdir:reports\coverage -historydir:reports\coveragehistory
