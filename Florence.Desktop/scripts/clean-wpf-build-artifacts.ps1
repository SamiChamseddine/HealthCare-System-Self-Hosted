$paths = @("bin","obj",".vs")
foreach ($p in $paths) {
    if (Test-Path $p) {
        Write-Host "Removing $p ..."
        Remove-Item -Recurse -Force $p -ErrorAction SilentlyContinue
    }
}
dotnet build