# Ethan-Guardian-of-the-Void

Core Gameplay Features
1. Dynamic Combat System
Player controls Ethan in a top-down 2D space battlefield
Smooth gradient bullets create stunning visual effects with colored trails
Responsive controls allow for strategic positioning and combat
Battlefield features space-themed background with star fields
UI elements include red hearts for health and ghost icons for kill count
2. Health and Performance Mechanics
Dynamic speed system tied to health:
100% speed at full health
Gradually decreases to 40% at minimum health
Creates tension and challenge as damage accumulates
Innovative enemy difficulty scaling:
Enemy health decreases as player's health drops
Fewer bullets needed to defeat enemies when wounded
Provides strategic comeback opportunities
3. Environmental Hazards
Strategic obstacle placement throughout the battlefield
Obstacles feature dynamic properties:
Turn red every 10 seconds for 2-second intervals
Contact during red phase damages player's health
Enhanced with bloom effect for visual appeal
Invisible boundary system:
Contains both player and enemies within the battle arena
Creates focused combat zones
4. Enemy AI and Progression
Aliens can phase through obstacles while player must navigate around them
After 10 kills, enemies gain access to deadly kryptonite projectiles:
One-hit kill weapon
Requires extreme vigilance to avoid
Enemy bullets can pass through obstacles
Strategic spawn system keeps combat engaging
5. Visual and Audio Enhancement
Camera effects:
Screen shake on player damage
Dynamic zoom effects during game start
Smooth camera following system
Audio features:
Distinct sound effects for player and enemy weapons
Customizable background music system:
Default battlefield theme
Option to add personal playlist
Toggle-able through checkbox interface
6. Technical Implementation
Variable-based design for easy gameplay tweaking
Efficient restart system upon player death
Optimized collision detection and trigger systems
Post-processing effects for enhanced visuals:
Bloom effects on obstacles
Dynamic lighting
Trial rendering effects for bullets for better effect
7. Game Flow
Starting animation with strategic camera zoom
Continuous wave-based combat
Automatic game restart on player death
Scoring system based on player health and enemy kill score
Progressive difficulty through player health and enemy behavior patterns
8. Unique Selling Points
Dynamic difficulty adjustment based on player health
Phase-shifting enemies create unique tactical challenges
Customizable audio experience
Visually striking effects and animations
Development Notes
Built using Unity engine with C# programming
Modular design allows for easy feature additions
Optimized for performance with fixed delta time implementation
Extensive use of Unity's post-processing stack
Organized codebase with separate scripts for player, enemies, and game management

