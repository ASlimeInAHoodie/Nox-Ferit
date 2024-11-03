# Nox Ferit

I pitched this mod idea to my friends and they said I should make it. Two days later here it is.

Nox Ferit is a configurable race against a shorter clock. At a certain time, the *Nox Oculus* will cause a spike in the maximum quantity of enemies that can appear inside, and all vents with monsters waiting to jump out will do so - instantly maxing out the new capacity. However, it's not all bad news - at least you get a multiplier to the quantity of scrap that can be found. 

> [!IMPORTANT]
> Nox Ferit requires BepInExPack as a dependency.

# Bug Reports & Feature Requests
Feel free to reach out via issues on the [Github Page](https://github.com/ASlimeInAHoodie/Nox-Ferit). Please include any relevant logs for error reporting, thanks!

# Releases
## Release V1.2.0
- Reworked the monster spawning process to involve the vent count and the configured multiplier (ventCount * configMultiplier).
- Reworked time checking to be (hopefully) more accurate.
- Merged scrapMultiplier and enemyPower in configuration to Multiplier.
- Changed default config values to 1.5f multiplier, and 6f nightStrikeTime.
- Added checks to stop clients from trying to run server side code.
## Release v1.1.0
- Fixed a critical bug where the spike would only happen once every time you loaded the game.
- Fixed an issue where an error would show up in console when you quit to title.
- Made changes to PlayerControllerB patching to include debugging on space and separated cheats into its own patch.
    - Currently stopped usage of double-jump as it was causing issues with debugging.
- Scrap amount multiplier now works with other mods! It now adds the multiplier at the start of a load instead of sets it when scrap spawns (Has not been tested when loading a new save - may add multiple times).
- Modified the default value in config for Night Strike Time from 8 (around 3-4pm) to 7.
## Full Release! v1.0.0
Previously known as v0.3.8, the full release has been completed with tweaks to the debugging tools (godmode, inf sprint, double jump, etc).