$rootPath = $PSScriptRoot
if (-not $rootPath) { $rootPath = Get-Location }

$outputPath = Join-Path $rootPath "project_code_slim.txt"

Write-Host "--- Collecting SLIM version of project ---" -ForegroundColor Cyan

if (Test-Path $outputPath) { Remove-Item $outputPath -Force }

# Только самые важные расширения
$extensions = @("*.cs", "*.csproj", "*.json", "*.yml", "*.yaml", "Dockerfile", "*.proto")

try {
    $files = Get-ChildItem -Path $rootPath -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
        # Исключаем всё лишнее, что раздувает файл
        $_.FullName -notmatch "\\bin\\" -and 
        $_.FullName -notmatch "\\obj\\" -and 
        $_.FullName -notmatch "\\\.vs\\" -and
        $_.FullName -notmatch "\\\.git\\" -and
        $_.FullName -notmatch "\\Migrations\\" -and  # Убираем миграции БД
        $_.FullName -notmatch "\\TestResults\\" -and 
        $_.Name -ne "project_code.txt" -and
        $_.Name -ne "project_code_slim.txt" -and
        $_.Name -ne "collect.ps1" -and
        $_.Name -notmatch "package-lock.json" # Если есть фронтенд
    } | Where-Object {
        $fileName = $_.Name
        $isMatch = $false
        foreach ($ext in $extensions) {
            if ($fileName -like $ext) { $isMatch = $true; break }
        }
        $isMatch
    }

    foreach ($file in $files) {
        # Пропускаем слишком большие файлы (больше 200 КБ), скорее всего это логи или кэш
        if ($file.Length -gt 200000) { continue }

        $relativeName = $file.FullName.Substring($rootPath.Length)
        Write-Host "Adding: $relativeName" -ForegroundColor Gray
        
        $header = "`n`n" + ("=" * 60) + "`nFILE: $relativeName`n" + ("=" * 60) + "`n"
        Add-Content -Path $outputPath -Value $header
        
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        Add-Content -Path $outputPath -Value $content
    }

    Write-Host "`nDONE! File: project_code_slim.txt" -ForegroundColor Green
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Read-Host "Press Enter..."