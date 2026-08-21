# 🏰 Chronos Bastion — 2D Portrait Tower Defense

> A vertical 2D Tower Defense game where players defend a central stronghold against 360°/top-down invading waves across historical eras.

[![Unity Version](https://img.shields.io/badge/Unity-6000.0.x-black?logo=unity)](https://unity.com/)
[![Language](https://img.shields.io/badge/Language-C%23-blue?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Play Demo](https://img.shields.io/badge/Play-WebGL%20Demo-green)](https://your-itch-io-link-here.itch.io)

---

## 🎮 Gameplay Preview
<!-- Đặt 1 ảnh GIF ngắn 10-15s quay gameplay mượt mà nhất vào đây -->
![Gameplay GIF](https://via.placeholder.com/600x350.png?text=Gameplay+Demo+GIF)

- **Platform:** Mobile (Portrait Mode, 9:16)
- **Playable Demo:** [WebGL Link trên Itch.io] | [Download Android APK v1.0]

---

## ⚡ Key Technical Features & Patterns

- **Targeting Algorithm:** Optimized $O(N)$ sorting to track enemies nearest to the castle gate without memory allocation (`NonAlloc` physics queries).
- **Era Evolution System:** Data-driven progression (Feudal Era ➔ Modern Era) built on `ScriptableObjects`.
- **Generic Object Pooling:** Zero runtime allocations for projectiles, impact particles, and enemy spawns to eliminate Garbage Collection spikes.
- **Event-Driven Architecture:** Decoupled Combat, Health, and UI layers via C# Events (`Action`).

---

## 📂 Project Architecture

```text
Assets/_Project/
├── Data/            # ScriptableObjects for Towers, Waves & Stats
├── Prefabs/         # Prefab templates (Towers, Enemies, VFX)
├── Scripts/
│   ├── Core/        # Game loop & State Machine
│   ├── Gameplay/    # Combat, Targeting, Castle Health, Waves
│   ├── UI/          # Decoupled UI Controllers
│   └── Utilities/   # Generic Object Pools & Helpers
