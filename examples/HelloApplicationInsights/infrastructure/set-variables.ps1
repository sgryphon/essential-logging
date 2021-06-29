function Set-Variables
{
  [CmdletBinding()]
  param([parameter(Mandatory)][string]$EnvironmentName)

  if ($EnvironmentName -eq "demo")
  {
    $global:Location = 'westus'
    $global:ResourceGroup='hello-ai-rg'
    $global:LogAnalyticsWorkspaceName='hello-ai-logs'
    $global:AppInsightsName='hello-ai-app-insights'
  }
  else
  {
    Write-Error "Unrecognized environment name, cannot infer other variable values"
  }
}
