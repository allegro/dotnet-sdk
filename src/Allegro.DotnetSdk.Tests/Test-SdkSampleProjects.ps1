
$projsDir = Get-Item $PSScriptRoot/projects
$projects = Get-ChildItem -Directory $projsDir

$reports = @()

foreach ($projectDir in $projects) {
    $name = $projectDir.Name
    try {
        Push-Location $projectDir
        Write-Host "Building test project: $name"
        $buildArgs = @(
            '--nologo'
            '--no-incremental'
        )

        if ($projectDir.Name.EndsWith("Error")) {
            dotnet build @buildArgs | Tee-Object -Variable buildOutput && $(throw "Project $name build succeeded. Failure is expected.")
            Write-Host "Project $name build failed. Failure is expected."
        }
        else {
            dotnet build @buildArgs | Tee-Object -Variable buildOutput || $(throw "Project $name build failed. Failure is not expected.")
        }
        if (Test-Path $projectDir/message.txt) {
            $msg = Get-Content $projectDir/message.txt
            if ("$buildOutput" -notlike "*$msg*") {
                throw "Output of $name build did not contain expected message:`n$msg"
            }
        }
        Write-Host -ForegroundColor Green "Testing project $name succeeded."
    }
    catch {
        $reports += [pscustomobject]@{
            ok        = $false
            name      = $name
            exception = $_
            path      = $projectDir
        }
    }
    finally {
        Pop-Location
    }
    $reports += [pscustomobject]@{
        ok        = $true
        name      = $name
        exception = $null
        path      = $projectDir
    }
}

$reports | Out-Host

$failedProjects = $reports.Where({ $_.ok -eq $false })
if ($failedProjects.Count -gt 0) {
    throw "Some projects failed testing: $($failedProjects.name)"
}