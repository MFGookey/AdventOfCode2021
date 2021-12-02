[CmdletBinding(SupportsShouldProcess=$false)]
param (
  [Parameter(Mandatory = $false, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true, Position=0)]
  [Alias('f')]
  [System.IO.FileInfo] $folderName = $null,

  [Parameter(Mandatory = $false, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true, Position=1)]
  [Alias('n')]
  [string] $projectName = $null
)

if($null -eq $folderName){
  $folderNameString = Read-Host -Prompt 'Please give a folder name'
  try{
    $folderName = $folderNameString
  }
  catch {
    Write-Error """$folderNameString"" is not a valid folder name"
    exit
  }
}

if($false -eq $projectName.trim()){
  $projectName = Read-Host -Prompt 'Please give a project name'
}

if($projectName -match '[^a-zA-Z0-9\.]|^$'){
  Write-Error "Project name is ""$projectName"" but must contain only alphanumeric chracters or ""."""
  exit
}

Write-Host "Initializing advent project $projectName at $folderName"

dotnet new advent -o "$folderName" -n "$projectName"