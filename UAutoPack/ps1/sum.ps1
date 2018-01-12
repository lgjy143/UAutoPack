function Sum
{
	param([int]$first, [int]$second)
	$result = $first + $second
	return $result
}

function Sum1
{
	param([int]$first, [int]$second,[int]$three)
	$result = $first + $second+$three
	return $result
}


function GetMsBuildPath([switch] $Use32BitMsBuild)
{
	## 判断是否存在MSBuild v15
	if(Test-Path -Path $Msbuild15Path)
	{
		return $Msbuild15Path
	}
	
    ## 获取MsBuild.exe的最新版本的路径。如果找不到MsBuild.exe，则抛出异常。
	$registryPathToMsBuildToolsVersions = 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\'
	if ($Use32BitMsBuild)
	{
		## 如果32位路径存在则使用它，否则将使用与当前系统位一致的路径。
		$registryPathTo32BitMsBuildToolsVersions = 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\'
		if (Test-Path -Path $registryPathTo32BitMsBuildToolsVersions)
		{
			$registryPathToMsBuildToolsVersions = $registryPathTo32BitMsBuildToolsVersions
		}
	}

	## 获取MsBuild最新版本所在目录的路径。
	$msBuildToolsVersionsStrings = Get-ChildItem -Path $registryPathToMsBuildToolsVersions | Where-Object { $_ -match '[0-9]+\.[0-9]' } | Select-Object -ExpandProperty PsChildName
	$msBuildToolsVersions = @{}
	$msBuildToolsVersionsStrings | ForEach-Object {$msBuildToolsVersions.Add($_ -as [double], $_)}
	$largestMsBuildToolsVersion = ($msBuildToolsVersions.GetEnumerator() | Sort-Object -Descending -Property Name | Select-Object -First 1).Value
	$registryPathToMsBuildToolsLatestVersion = Join-Path -Path $registryPathToMsBuildToolsVersions -ChildPath ("{0:n1}" -f $largestMsBuildToolsVersion)
	$msBuildToolsVersionsKeyToUse = Get-Item -Path $registryPathToMsBuildToolsLatestVersion
	$msBuildDirectoryPath = $msBuildToolsVersionsKeyToUse | Get-ItemProperty -Name 'MSBuildToolsPath' | Select -ExpandProperty 'MSBuildToolsPath'

	if(!$msBuildDirectoryPath)
	{
		throw 'The registry on this system does not appear to contain the path to the MsBuild.exe directory.'
	}

	## 获取MsBuild可执行文件的路径。
	$msBuildPath = (Join-Path -Path $msBuildDirectoryPath -ChildPath 'msbuild.exe')

	if(!(Test-Path $msBuildPath -PathType Leaf))
	{
		throw "MsBuild.exe was not found on this system at the path specified in the registry, '$msBuildPath'."
	}

	return $msBuildPath
}

function BuildSln([string]$slns){
   $msBuildPath=GetMsBuildPath(true)
   ."$msBuildPath" $slns
}	


