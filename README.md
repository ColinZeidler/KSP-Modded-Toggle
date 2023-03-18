# KSP2 Modded Toggle
a simple tool for managing multiple mod states for KSP2
uses sym links to link your mods into the KSP2 install, and remove them when you don't want to use them.

## Usage
 - Right side displays the currently selected configuration.
 - Left side displays your list of configurations

On Initial startup one new config will be created

### Fields
 - KSP Directory, This is a global value shared by all configurations, point it at the directory where KSP2_x64.exe is located
 - Modded, Flags if the current configuration applies mods, if this is selected Mods Directory can be left blank
 - Active, Tells you if the current Configuration is the one last applied to KSP2
 - Name, Name of the configuration, User info only
 - Mods Directory, where the mods you want to use are located. Expects to find a BepInEx Mod directory, you can download and extract then point this there.

### Buttons
 - Save, saves the entered values to disk
 - Apply Mod Env, applies the current configuration, making it the active
 - Delete, Deletes the current configuration.
 - Add Env, Creates a new blank environment to configure
