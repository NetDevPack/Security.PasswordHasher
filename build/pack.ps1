param([string] $v)

if (!$v)
{
    $version = '3.0.2-prerelease1.' + $([System.DateTime]::Now.ToString('MM-dd-HHmmss'))
}
else{
	$version = $v
}
Write-Host 'Version: ' $version 
get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\JpProject.PasswordHasher.sln
dotnet test ..\src\JpProject.PasswordHasher.sln
dotnet pack ..\src\JpProject.PasswordHasher.sln -o .\ -p:PackageVersion=$version