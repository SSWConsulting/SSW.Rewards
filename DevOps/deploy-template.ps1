param(
	[ValidateNotNullOrEmpty()]
	[string]$DeploymentName = [guid]::newguid().guid,

	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()]
	[string]$TemplateFile,
	
	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()]
	[string]$ParametersFile,
	
	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()][string]$SubscriptionId,

	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()][string]$ResourceGroupName,
	
	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()][string]$Location
)

function DeployTemplate {
	param(
		[string]$DeploymentName,
		[string]$ResourceGroupName,
		[string]$Location,
		[string]$SubscriptionId,
		[string]$Template,
		[string]$Parameters
	)

	Write-Host "Creating resource group: $ResourceGroupName ($Location)" -ForegroundColor Magenta

	az group create `
		--name $ResourceGroupName `
		--location $Location

	Write-Host "Creating Deployment" -ForegroundColor Magenta

	$deployment = az group deployment create `
		--verbose `
		--name $DeploymentName `
		--resource-group $ResourceGroupName `
		--template-file $Template `
		--subscription $SubscriptionId `
		--parameters $Parameters

	# Write-Host ($deployment | ConvertTo-Json) -ForegroundColor Magenta

	return $deployment
}

Write-Host "Deploying ARM Template (deployment name: $DeploymentName)" -ForegroundColor Green
DeployTemplate `
	-DeploymentName $DeploymentName `
	-ResourceGroupName $ResourceGroupName `
	-Location $Location `
	-SubscriptionId $SubscriptionId `
	-Template $TemplateFile `
	-Parameters $ParametersFile
	
# TODO: Check success of the deployment before going ahead with keyVault 
.\deploy-keyvault.ps1 -ResourceGroupName $ResourceGroupName -Location $Location -Environment $Environment -Project $Project