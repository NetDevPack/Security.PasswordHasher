$version = $args[0]

if (!$version)
{
    $version = '3.1.1-prerelease1.' + $([System.DateTime]::Now.ToString('MM-dd-HHmmss'))
}
Write-Host 'Version: ' $version 
get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\JpProject.PasswordHasher.sln
dotnet test ..\src\JpProject.PasswordHasher.sln
dotnet pack ..\src\JpProject.PasswordHasher.sln -o .\ -p:PackageVersion=$version