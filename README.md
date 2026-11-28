# ZOMBIELAND — TECHNICAL REVIEW REPORT

**Project:** ZombieLand  
**Engine:** Unity 2022.3.20f1  
**Review Type:** Codebase and Gameplay Systems Technical Review  
**Repository:** Ristwak/ZombieLand  
**Review Scope:** C# Scripts, Gameplay Systems, AI Behaviour, Combat Mechanics, Project Structure and Performance Considerations

---

## 1. Project Overview

ZombieLand is a Unity-based 3D zombie survival game featuring third-person character movement, weapon-based combat, melee attacks, zombie AI, vehicle mechanics and objective-based progression.

The project implements multiple interconnected gameplay systems using C# scripts and Unity components. These include player movement and health management, rifle mechanics, enemy navigation, zombie spawning, vehicle controls, interactive pickups and objective tracking.

The game also incorporates audio feedback, animation controllers, UI elements and environmental interactions to support its gameplay experience.

This review focuses on the implementation of gameplay systems, code quality, system interactions and potential technical improvements.

---

## 2. Repository Structure

The GitHub repository contains the following main directories and files:

### Root Directory

- `.vscode` — Editor-related configuration.
- `Assets` — Game assets, scripts, scenes and other Unity resources.
- `Packages` — Unity package dependencies and package configuration.
- `ProjectSettings` — Unity project and editor settings.
- `.gitattributes` — Git attribute configuration.
- `.gitignore` — Files and directories excluded from Git tracking.
- `README.md` — Project documentation.

The repository follows the standard Unity project structure, separating assets, package dependencies and project settings.

---

## 3. Core Gameplay Systems

### 3.1 Player Movement and Health System

**Primary Script:** `Player.cs`

The player controller implements:

- Third-person movement using keyboard input.
- Camera-relative movement and character rotation.
- Jumping and gravity using CharacterController.
- Walking and sprinting mechanics.
- Player health and damage handling.
- Damage feedback through UI and audio.
- Death handling and end-game UI activation.

**Technical Observations:**

- Player movement and sprinting are handled through separate methods that can execute during the same Update cycle.
- Both methods can invoke CharacterController.Move(), potentially resulting in inconsistent movement.
- Animation parameters are modified across multiple methods, which can lead to conflicting animation states.
- Health management does not expose a centralized maximum-health value for other gameplay systems.

### 3.2 Weapon and Shooting System

**Primary Script:** `Rifle.cs`

The rifle system includes:

- Raycast-based shooting.
- Configurable shooting range and damage.
- Fire-rate control.
- Magazine and ammunition management.
- Reloading using coroutines.
- Muzzle effects and shooting audio.
- Damage interaction with zombies and destructible objects.
- Ammunition UI updates.

**Technical Observations:**

- Shooting and animation conditions contain overlapping logic.
- The FireWalk condition has an operator-precedence issue.
- Reloading restores player movement values using hardcoded defaults.
- Weapon animation states could be managed through a more centralized state system.

### 3.3 Melee Combat System

**Primary Script:** `PlayerPunch.cs`

The melee system uses camera-based raycasting to detect objects and enemies within punching range.

Implemented functionality includes:

- Configurable punch damage.
- Configurable attack range.
- Interaction with destructible objects.
- Damage application to different zombie types.

The system is simple and reusable, although attack timing and input handling are currently managed externally by `RiflePickup.cs`.

### 3.4 Zombie AI and Navigation

**Primary Scripts:** `Zombie1.cs`, `StandingZombie.cs`

Zombie behaviour is implemented using Unity NavMesh and physics-based player detection.

Implemented behaviours include:

- Player detection through vision and attack radii.
- Patrolling between predefined waypoints.
- Pursuing the player.
- Attacking when the player enters attack range.
- Health and damage management.
- Death animations and audio feedback.

**Technical Observations:**

- Zombie behaviour depends on correctly assigned NavMesh, waypoint and Inspector references.
- Patrol logic assumes valid waypoint entries.
- Attack execution depends on raycast detection even after the player enters the attacking radius.
- A dedicated death-state guard would prevent unnecessary AI processing after the zombie has been defeated.

### 3.5 Zombie Spawning System

**Primary Script:** `ZombieSpawn.cs`

The spawning system activates when the player or vehicle enters a designated trigger area.

Implemented functionality includes:

- Trigger-based spawning.
- Multiple predefined spawn positions.
- Repeated enemy spawning using InvokeRepeating.
- Danger-zone UI activation.
- Audio feedback during activation.

**Technical Observations:**

- Three zombies are spawned every second.
- There is no active-enemy limit.
- No object-pooling system is implemented.
- OnTriggerExit() disables the collider without checking which object exited.
- Spawn lifecycle management could be improved to avoid uncontrolled enemy generation.

### 3.6 Vehicle System

**Primary Script:** `VehicleController.cs`

The vehicle controller implements:

- WheelCollider-based movement.
- Vehicle acceleration and braking.
- Steering and wheel transformations.
- Player entry and exit interactions.
- Camera and UI switching.
- Raycast-based zombie and object damage.

**Technical Observations:**

- Vehicle damage is applied through a raycast executed continuously while driving.
- Repeated raycast hits can apply damage every frame.
- Vehicle entry, exit and camera switching are tightly coupled.
- Damage handling would benefit from controlled collision or attack events.

### 3.7 Health and Ammunition Pickups

**Primary Scripts:** `HealthBoost.cs`, `AddAmmo.cs`, `RiflePickup.cs`

The pickup system supports:

- Health restoration.
- Ammunition replenishment.
- Rifle collection.
- Interaction through proximity and keyboard input.
- Audio and animation feedback.

**Technical Observations:**

- HealthBoost directly assigns health rather than applying a controlled restoration amount.
- Health can exceed the player's initial health value.
- The health bar is not updated after collecting a health boost.
- AddAmmo replaces the magazine count instead of adding ammunition.
- Pickup interactions could use a shared interaction system.

### 3.8 Objective and UI Systems

**Primary Scripts:** `ObjectivesComplete.cs`, `Objective2.cs`, `Objective4.cs`, `Menu.cs`, `MainMenu.cs`, `HealthBar.cs`, `AmmoCount.cs`

The project includes:

- Main menu navigation.
- Character selection UI.
- Pause and resume functionality.
- Objective completion tracking.
- Health and ammunition displays.
- Game-over UI handling.

**Technical Observations:**

- Objective completion is managed through separate methods.
- Some gameplay systems directly reference UI components.
- AmmoCount and ObjectivesComplete use static singleton references without duplicate-instance handling.
- Objective4 immediately loads the MainMenu scene after vehicle interaction.

---

## 4. Code Quality and Architecture

### Strengths

- Gameplay systems are divided into individual MonoBehaviour scripts.
- Inspector-based configuration provides flexibility.
- Unity NavMesh is used for enemy navigation.
- Coroutines and InvokeRepeating support timed gameplay operations.
- Combat, movement, pickups and objectives have separate script implementations.

### Areas for Improvement

- Reduce tight coupling between gameplay scripts.
- Centralize player health and weapon state management.
- Separate input processing from gameplay execution.
- Improve animation-state management.
- Add validation for serialized Inspector references.
- Introduce better enemy lifecycle and spawn management.
- Reduce repeated per-frame operations where event-driven logic is more suitable.

---

## 5. Performance Considerations

The main potential performance concern is uncontrolled zombie spawning.

The current spawning implementation creates three zombie instances every second without an active-enemy limit or pooling mechanism.

Additional areas worth profiling include:

- NavMesh updates across multiple active enemies.
- Repeated raycasts during combat and vehicle operation.
- Runtime Instantiate and Destroy operations.
- Repeated UI and GameObject activation.
- Enemy AI processing after death.

Object pooling, controlled spawning and optimized enemy state management could improve runtime efficiency.

---

## 6. Recommended Improvements

The following improvements are recommended:

1. Consolidate player movement and sprint logic.
2. Improve consistency of Animator parameter management.
3. Introduce controlled weapon firing and damage execution.
4. Add an active-enemy limit and consider object pooling.
5. Implement a centralized health-management approach.
6. Correct vehicle damage execution to avoid repeated per-frame damage.
7. Improve zombie death-state handling.
8. Introduce reusable pickup and interaction logic.
9. Improve validation of Inspector-assigned references.
10. Reduce unnecessary coupling between gameplay and UI systems.

---

## 7. Overall Assessment

ZombieLand provides a solid foundation for a 3D zombie survival game, incorporating several interconnected gameplay systems such as player movement, weapon combat, zombie AI, vehicle mechanics, pickups and objective progression.

The project demonstrates the implementation of multiple Unity gameplay concepts, including CharacterController movement, NavMesh navigation, raycast-based combat, WheelCollider vehicle physics, coroutines and UI integration.

The main areas requiring improvement are gameplay state management, movement consistency, enemy spawning, vehicle damage handling and health-system integration.

Addressing these areas would improve gameplay reliability, code maintainability and overall runtime efficiency.

**Review Scope:** This review is based on static inspection of the repository's gameplay scripts and project structure. Runtime behaviour, Unity Play Mode testing, scene execution and Inspector-reference validation have not been independently verified.
