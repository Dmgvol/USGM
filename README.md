# Universal Save Game Manager (USGM)
A simple yet universal tool to manage save files, allowing to store and load different save files.

*Note: Supports only single sav files, doesn't support chained/multiple files(yet).*

## How to use
- Download the latest [version](https://github.com/Dmgvol/USGM/raw/main/USGM/bin/Release/USGM.exe).
- Place the exe in the SavedGames folder: </br>
`%localappdata%\<Game>\Saved\SaveGames`
- Create a new directory called `Saves`.
- Place all of your custom saves in that folder, and name them to differentiate between them.
- Launch the USGM, it will generate the default config file suited for Shadow Warrior 3.
- Once you're ready, select the save, slot id and click "Load save".


## Configuring for different games
The config structure file is really simple, just 3 parameters;
```css
SaveFilePrefix=SaveSlotIndex{n}
SaveFileCountStart=1
TotalSlots=3
```

- SaveFilePrefix - Specify how the save file is named
  
- - Example with incremental ID (for Shadow Warrior 3):</br>
`SaveFilePrefix=SaveSlotIndex{n}` </br>
Example with no incremental ID (for Ghostrunner):</br>
`SaveFilePrefix=Ghostrunner` </br>

- SaveFileCountStart - In case there is an incremental ID in the save file, specify from which number to begin with, in case it starts with 0.
- TotalSlots - Corresponds to the number of save slots.

## Configs for different games 
Shadow Warrior 3:
```
SaveFilePrefix=SaveSlotIndex{n}
SaveFileCountStart=1
TotalSlots=3
```

Solar Ash:
```
SaveFilePrefix=Save_{n}
SaveFileCountStart=0
TotalSlots=3
```


Ghostrunner:
```
SaveFilePrefix=Ghostrunner
SaveFileCountStart=0
TotalSlots=1
```

## Credits
Coded by [DmgVol](https://github.com/Dmgvol/), making it universal for other games using a simple config file.</br>
Design and functionality is inspired by [SASGM](https://github.com/Micrologist/SASGM) by [Micrologist](https://github.com/Micrologist).
