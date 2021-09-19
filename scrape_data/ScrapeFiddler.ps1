param(
[Parameter(Mandatory=$true)][string]$ScrapeDataFile,

[switch]$UpdateInfo,
[switch]$GetInfo,
[string]$Description,
[int]$Rating,
[string]$MapName,
[string]$MapUrl,

[switch]$GetRandomMap,
[switch]$ListMaps,
[switch]$CountMaps,
[switch]$NoRating,
[switch]$NoDescription,
[switch]$NoLabels,
[switch]$CheckDuplicateNames
)
"============================"

$file = Get-Item $ScrapeDataFile
if(-not $file){
    "Cannot read $ScrapeDataFile. Quitting..."
    exit 1
}

$json = Get-Content $file | ConvertFrom-Json
if(-not $json){
    "Cannot parse $ScrapeDataFile as json. Quitting..."
    exit 1
}

function GetMapWithName($name){
    $filtered = $json.MapInfo | ?{$_.Name -eq $name}
    if(-not $filtered){"Cannot find map with $name"; exit 1}
    if($filtered -is [array]){"Cannot get unique map $name. Map name is duplicated!"; exit 1}
    return $filtered
}

function GetMapWithURL($url){
    $filtered = $json.MapInfo | ?{$_.Link -eq $url}
    if(-not $filtered){"Cannot find map with URL $url"; exit 1}
    if($filtered -is [array]){"Cannot get unique map with URL $url. Map is duplicated!"; exit 1}
    return $filtered
}

"Total maps in file: $($json.MapInfo | measure | select -ExpandProperty count)"

if($GetRandomMap -or $ListMaps -or $CountMaps){
    $filteredMaps = $json.MapInfo
    if($NoRating)       { $filteredMaps  = $filteredMaps | ?{-not $_.RobRating}}
    if($NoDescription)  { $filteredMaps  = $filteredMaps | ?{-not $_.RobDescription}}
    if($NoLabels)       { $filteredMaps  = $filteredMaps | ?{-not $_.RobLabels}}

    if(-not $filteredMaps){ "No maps found!"; exit 1}

    "============================`n$($filteredMaps | measure | select -ExpandProperty Count) found with properties
     
NoRating: $NoRating    
NoDescription: $NoDescription
NoLabels: $NoLabels`n============================"

    if($GetRandomMap){
        $randomMap = $filteredMaps | Get-Random
        "$($randomMap.Name) copied to clipboard!"
        $randomMap.Name | Set-Clipboard
    }
    elseif($ListMaps){$filteredMaps}

    exit 0
}

if($CheckDuplicatesNames){
    $tempDict = @{}
    $json.MapInfo | %{
        if($tempDict.ContainsKey($_.Name)){ "Found duplicate map name: $($_.Name)"}
        $tempDict[$_.Name] = 1
    }
    exit 0
}

if($GetInfo){
    if(-not $MapName){"GetInfo set but no MapName provided. Quitting..."; exit 1}
    GetMapWithName($MapName)    
    exit 0
}

if($UpdateInfo){
    if($MapName -and $MapUrl){"Both MapName and MapUrl provided. Use one or the other..."; exit 1}
    if(-not $MapName -and -not $MapUrl){"Neither MapName nor MapUrl provided. Use one or the other..."; exit 1}
    if($MapName){$updateInfoMap = GetMapWithName($MapName)}
    else{$updateInfoMap = GetMapWithURL($MapUrl)}

    if($Rating){$updateInfoMap | Add-Member -MemberType NoteProperty -Name 'RobRating' -Value $Rating -Force}
    if($Labels){$updateInfoMap | Add-Member -MemberType NoteProperty -Name 'RobLabels' -Value $Labels -Force}
    if($Description){$updateInfoMap | Add-Member -MemberType NoteProperty -Name 'RobDescription' -Value $Description -Force}
    $updateInfoMap.GetType()

    "Writing updated data to $ScrapeDataFile"

    $json | ConvertTo-Json | Set-Content -Path $ScrapeDataFile

    exit 0
}
