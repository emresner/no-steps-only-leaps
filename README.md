<div align="center">

# 🐸 No Steps, Only Leaps!

**You can't walk. Charge, aim, leap, and don't miss the platform.**

[![Play now](https://img.shields.io/badge/▶_PLAY_NOW-in_your_browser-F2C94C?style=for-the-badge)](https://emresner.github.io/no-steps-only-leaps/)

![Unity](https://img.shields.io/badge/Unity-6.3_LTS-000000?logo=unity&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-WebGL-990000?logo=webgl&logoColor=white)
![Mobile](https://img.shields.io/badge/Mobile-touch_controls-2EA44F)
![Language](https://img.shields.io/badge/C%23-source_showcase-7B4FD1?logo=csharp&logoColor=white)

[**▶ Play**](https://emresner.github.io/no-steps-only-leaps/) · [**🎮 Controls**](#-controls) · [**✨ Features**](#-features) · [**📱 Responsive & mobile**](#-responsive--mobile) · [**🛠️ Tech**](#️-tech) · [**🙏 Credits**](#-credits)

</div>

## 🎯 The Game

Your character can't walk. Every move is a jump: hold to charge, let go to leap, and pick your direction before you commit. Climb to the top as fast as you can, with traps, wall bounces and checkpoints along the way.

A good jump saves you seconds. A bad one can drop you far enough to lose a whole minute. It's a precision platformer about learning the level and getting a little more consistent with every run.

> [!TIP]
> Runs in any modern browser, desktop or phone (landscape). No install, no account.

## 🎮 Controls

| Action | ⌨️ Keyboard | 👆 Touch |
|---|---|---|
| Face left / right | `A` / `D` | `<` / `>` buttons |
| Charge and jump | Hold `Space`, release to jump | Hold `JUMP`, release to jump |
| Back to checkpoint | `R` | `R` button |
| Pause | `Esc` or `P` | `II` button |

Changing direction while charging cancels the jump, so decide before you press. The `JUMP` button fills up as you charge, so you can see how hard you're about to leap.

## ✨ Features

**Gameplay**
- 🦘 Charge-based jumping with direction control, no walking at all
- 🧱 Wall bounces and a hard-landing hit state
- 🚩 Checkpoints with a quick return-to-checkpoint action
- 🔥 Traps: spikes, fire traps, moving spiked balls and falling platforms
- 🍓 Collectible fruits that persist between runs
- ⏱️ Run timer and saved best time

**Game feel and UI**
- 🎨 Unlockable and selectable skins in an in-game market
- 💥 Camera shake, spawn and pickup VFX
- 💬 Typewriter-style ending dialogue
- ⏯️ Main menu, how-to-play panel, pause menu and ending screen
- 🔊 Sound effects with an audio toggle
- 💾 Progress, skins and best time saved in your browser

## 📱 Responsive & mobile

The game started as a desktop-only build and was reworked to play properly anywhere in the browser.

- 🖼️ **Fixed 16:9 play area.** Every screen size sees exactly the same part of the level, so the difficulty doesn't change with your device. Extra space becomes black bars, and the UI lives inside the same 16:9 frame.
- 📲 **Touch controls** built with Unity's Input System On-Screen Controls. They appear the moment you touch the screen and hide again when you press a key, and the same input actions drive both keyboard and touch.
- 🔄 **Landscape only.** Hold the phone upright and you get a "rotate your device" screen instead of a tiny, unreadable game.
- 🌐 **Custom WebGL template** with a full-screen canvas, sharp pixel scaling, a themed loading bar and a fullscreen button.

## 🛠️ Tech

| | |
|---|---|
| Engine | Unity **6.3 LTS** (`6000.3.28f1`), Universal Render Pipeline (2D) |
| Input | Unity Input System (keyboard + on-screen touch controls) |
| Camera | Cinemachine 3 |
| UI | uGUI + TextMeshPro |
| Architecture | Finite state machine for the player, singleton managers, ScriptableObject skin data |
| Saving | JSON save data, PlayerPrefs-backed persistence on WebGL |
| Web | WebGL, custom HTML template, hosted on GitHub Pages |
| Language | C# |

## 📁 Repository notes

This repo is a **source code showcase**.

- `main` has the gameplay and UI scripts and the input actions asset.
- The playable build is deployed from the `gh-pages` branch.
- Art, audio, scenes, prefabs and the rest of the Unity project are **not** included, so the project won't open as-is from this repo.

## 🧠 How it came together

This is my first complete game, built while learning Unity, 2D gameplay programming, UI systems, saving and WebGL publishing. It started as a small platformer and slowly grew into a finished, playable game. The latest round of work was making it feel right on every screen: the fixed 16:9 viewport, the portrait overlay, and the touch controls with the jump charge fill.

Next up: more polished projects and cleaner systems.

## 🙏 Credits

- 🎨 Character and environment art: **Pixel Adventure 1** by **Pixel Frog**
- 🔤 Font: **Pixelify Sans** (SIL Open Font License)
- 🔠 Default UI font: **Liberation Sans** (SIL Open Font License), included with TextMeshPro
- 🔊 Sound effects: made with **ChipTone** by SFBGames
- 🖌️ UI buttons and icons: drawn by me in **Aseprite**
- 🛠️ Built with **Unity 6** and **C#**

Thank you to Pixel Frog for sharing such a great art pack. 💛

<div align="center">

Made for fun and for learning.

</div>
