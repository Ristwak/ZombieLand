#ZombieLand

PROJECT: ZombieLand
ENGINE: Unity 2022.3.20f1
REPOSITORY: Ristwak/ZombieLand

OVERVIEW:
ZombieLand is a 3D zombie survival game featuring third-person player movement, rifle shooting, melee combat, zombie AI, vehicle mechanics, health and ammunition pickups, and objective-based progression.

KEY SYSTEMS:
- Third-person player movement, jumping and sprinting.
- Rifle shooting, ammunition management and reloading.
- Melee combat using the player punch system.
- Zombie AI using Unity NavMesh.
- Patrolling and stationary zombie behaviours.
- Vehicle driving, steering, braking and combat.
- Health and ammunition pickups.
- Player health and damage feedback.
- Main menu, pause menu and objective progression.

TECHNICAL FINDINGS:

1. PLAYER MOVEMENT (Player.cs)
- PlayerMove() and Sprint() can both execute during the same Update cycle.
- Both methods can call CharacterController.Move(), potentially causing inconsistent movement.
- Jump animation parameters can conflict with movement animation states.
- Movement and animation handling should be consolidated.

2. WEAPON SYSTEM (Rifle.cs)
- Shooting and animation conditions contain overlapping logic.
- The FireWalk condition has an operator-precedence issue.
- Reloading restores movement speed using hardcoded values.
- Animation states should be managed more consistently.

3. ZOMBIE AI (Zombie1.cs, StandingZombie.cs)
- AI depends on valid NavMesh configuration and Inspector references.
- Patrol logic assumes valid waypoint assignments.
- Attack behaviour relies on raycast detection.
- A dedicated death state would prevent unnecessary AI processing after death.

4. ZOMBIE SPAWNING (ZombieSpawn.cs)
- Three zombies are instantiated every second.
- No active-enemy limit or pooling system is implemented.
- Repeated spawning can increase runtime memory and processing costs.
- OnTriggerExit() disables the spawn collider without checking the exiting object.

5. VEHICLE SYSTEM (VehicleController.cs)
- Zombie damage is applied through a raycast executed continuously while driving.
- Repeated raycast hits can apply damage every frame.
- Vehicle entry, exit and camera switching are tightly coupled.
- Damage handling should be controlled through collision or attack events.

6. HEALTH SYSTEM (Player.cs, HealthBoost.cs)
- Health pickup directly assigns health instead of adding a controlled amount.
- Health can exceed the player's initial maximum health.
- HealthBar is not updated after collecting a health pickup.
- Health restoration should use a maximum-health limit.

7. PICKUPS AND OBJECTIVES
- AddAmmo.cs replaces the magazine count instead of adding ammunition.
- RiflePickup.cs combines punch and weapon pickup input handling.
- Objective4.cs immediately loads the MainMenu scene after vehicle interaction.
- Objective progression could benefit from centralized state management.

CODE QUALITY:
- Gameplay systems are separated into individual scripts.
- Inspector-based configuration provides flexibility.
- Several scripts have tightly coupled dependencies.
- Some serialized references lack null validation.
- Input, animation and gameplay logic could be better separated.
- Repeated runtime Instantiate/Destroy operations could be optimized.

PERFORMANCE CONSIDERATIONS:
- Implement object pooling for zombies and temporary effects.
- Introduce an active-enemy spawn limit.
- Profile NavMesh updates with multiple active enemies.
- Avoid unnecessary repeated raycasts and per-frame operations.
- Improve enemy lifecycle management.

OVERALL ASSESSMENT:
ZombieLand provides a functional foundation for a 3D zombie survival game, with multiple interconnected gameplay systems including combat, enemy AI, vehicle mechanics, pickups and objectives.

The main areas requiring improvement are player movement consistency, weapon state management, zombie spawning, vehicle damage handling and health management.

Improving these systems would increase gameplay reliability, maintainability and runtime efficiency.

REVIEW SCOPE:
Static source-code inspection of gameplay scripts. Runtime behaviour, Unity Play Mode testing and Inspector-reference validation have not been independently verified.
