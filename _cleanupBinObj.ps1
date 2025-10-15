Get-ChildItem -Directory -Recurse |
    Where-Object {
        $_.Name -match '^(bin|obj|x64|x86)$' -and
        $_.FullName -notmatch 'vcpkg_installed'
    } |
    ForEach-Object {
        Write-Host ("Lösche Ordner: " + $_.FullName) -ForegroundColor Yellow
        Remove-Item -Path $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }
