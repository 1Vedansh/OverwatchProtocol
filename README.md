# OverwatchProtocol
A Unity game created for Zense recruitment.

## Overview
**OverwatchProtocol** is a 3D first person shooter survival game where your goal is to survive against waves of robot enemies using your weapons and aiming skills. Each playthrough features a procedurally generated world, ensuring a unique experience every time.

## Features
- **Different Weapons** : The game features two primary weapons, the Rifle and the Sniper. The Sniper deals higher damage at longer ranges, while the Rifle has a higher fire rate but lower damage. Thanks to the modular code, new weapons can be added instantly without any hassle.

- **Enemy** : The enemy has its own AI that can be idle, walk, or charge towards the player. Defeating enemies restores the player's health and provides the ammo needed to survive. Thanks to the modular code, new enemies can be added with ease. The AI uses pathfinding algorithms to reach the player.

- **Difficulty** : The game has three difficulties: easy, medium, and hard, providing an opportunity for players of all skill levels.

- **Procedural Map** : Each world has a unique layout generated using Perlin noise.

- **Post Apocalyptic Theme** :  A world where robots have taken over. 

## Development Tools
- **Game Engine**: Unity

- **Models & Sound Effects**: Unity Asset Store and Various Other Free Sources.

- **Animations**: Some created by me, and others sourced from Mixamo.

## How To Play
- **Movement**: WASD to move, Spacebar to jump, Shift to sprint.

- **Scope**: Use the right mouse button to scope with the rifle, and the scroll wheel to zoom in or out.

- **Shoot**: Hold the left mouse button to fire automatically.

- **Leave Game**: Press Escape in-game to exit the game.

- **Sensitivity** : You can change your sensitivity in the main menu settings.

- **Loading Time** : Loading a map can take 10 to 20 seconds.

**Demo Gameplay** : https://www.youtube.com/watch?v=60y8AslfLNA

## Future Scope
- Add a multiplayer co-op feature.

- Add more exciting and diverse types of enemies.

- Add a boss level.

- Experiment with more procedural generation.

## Installation & Setup
1. **Clone the Repository**

2. **Open in Unity:**
- Open Unity Hub and click on "Add" -> "Add project from disk".

- Navigate to the cloned repository folder and select it.

It is also available to play online at : https://strangecraft.itch.io/overwatch-protocol

## Challenges faced:
- Learning 3D mesh manipulation was a whole new experience for me. It was a bit unintuitive at first, but over time I got the hang of it.

- Animation was my weakest area; I don’t like animating things, so instead, I tried to create procedural animations that are done through code. For example, the recoil is procedural and can be modified as desired.

- Finding the correct spawn logic for the enemy was tricky as I had to find the right balance of gameplay difficulty and player enjoyment. 
