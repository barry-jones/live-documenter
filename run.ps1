param (
    [string]$type,
    [string]$appargs
)

if($type -match "perf") {
    push-location -path "source\performancetests\bin\release"
    .\performancetests.exe $appargs
}
