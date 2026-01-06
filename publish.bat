rmdir Client\out /s /q & ^
rmdir Server\out /s /q & ^
dotnet publish Client\Client.csproj -p:PublishProfile=FolderProfile -p:InterceptorsPreviewNamespaces=Microsoft.Extensions.Configuration.Binder.SourceGeneration && ^
dotnet publish Server\Server.csproj -p:PublishProfile=FolderProfile
