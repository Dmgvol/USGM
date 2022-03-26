# Universal Save Game Manager (USGM)
A simple yet universal tool to manage save files, allowing to store and load different save files.

## How to use
- Download the latest [version](https://github.com/Dmgvol/USGM/raw/main/USGM/bin/Release/USGM.exe).
- Place the exe in the SavedGames folder: </br>
`%localappdata%\<Game>\Saved\SaveGames`
- Create a new directory called `Saves`.
- Place all of your custom saves in that folder, and name them to differentiate between them. 
- Launch USGM, it will generate the default config file suited for Shadow Warrior 3.
- Once you're ready, select the save, slot id and click "Load save".


## Configuring for different games
The config structure file is really simple, just 5 parameters;
```css
SaveFilePrefix=SaveSlotIndex{n}
SaveFileCountStart=1
TotalSlots=3
IsComplex=True
Sort=True
```

- SaveFilePrefix - Specify how the save file is named,</br>
can have multiple prefixs if you set `IsComplex` to True.
  
- - Example with incremental ID (for Shadow Warrior 3):</br>
`SaveFilePrefix=SaveSlotIndex{n}` </br>
Example with no incremental ID (for Ghostrunner):</br>
`SaveFilePrefix=Ghostrunner` </br>

- SaveFileCountStart - The incremental ID number which it starts to count from.
- TotalSlots - Corresponds to the number of save slots.

- IsComplex - In case your game uses multiple save files per slot. 
- - Please see [Folder Hierarchy](#saves-folder-hierarchy) for proper usage.
- - And [Configuring For Different Games](#configuring-for-different-games) for examples.
- Sort -  In case you want the saves to be visually sorted.


## Saves Folder Hierarchy 
- If you have `IsComplex` set to **False** (single `.sav` file) then use the following hierarchy:
```css
-Saves
  └ 2 Way to Motoko.sav
  └ 3 Motoko's Thunderdome.sav
  └ 3 Motoko's Thunderdome2.sav

  and etc..
```

- If you have `IsComplex` set to **True** (multiple `.sav` files) then use the following hierarchy:
```css
-Saves
└ 2 Way to Motoko
  └ ProgressionSlotIndex2.sav
  └ SaveSlotIndex2.sav
└ 3 Motoko's Thunderdome
  └ ProgressionSlotIndex2.sav
  └ SaveSlotIndex2.sav

and etc...
```

_Note: file names are just for example purposes._</br>

## Configs for different games 
Ghostwire: Tokyo:
```css
SaveFilePrefix=SaveGameSlot{n}
SaveFileCountStart=1
TotalSlots=10
IsComplex=False
Sort=True
```

Shadow Warrior 3:
```css
SaveFilePrefix=SaveSlotIndex{n}
SaveFilePrefix=ProgressionSlotIndex{n}
SaveFileCountStart=1
TotalSlots=3
IsComplex=True
Sort=True
```

Solar Ash:
```css
SaveFilePrefix=Save_{n}
SaveFileCountStart=0
TotalSlots=3
IsComplex=False
Sort=True
```


Ghostrunner:
```css
SaveFilePrefix=Ghostrunner
SaveFileCountStart=0
TotalSlots=1
IsComplex=False
Sort=True
```

## Credits
Coded by [DmgVol](https://github.com/Dmgvol/), making it universal for other games using a simple config file.</br>
Design and functionality is inspired by [SASGM](https://github.com/Micrologist/SASGM) by [Micrologist](https://github.com/Micrologist).
