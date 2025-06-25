# Little_npc_AI
Minimum AI for enemy on unity 3d


**Setup instructions:**
1. Setting up an enemy:

Create a GameObject for the enemy

Add the EnemyAI component

In the Player field, drag the player object (or leave it empty - the script will find it automatically by the "Player" tag)

Configure the parameters:


Move Speed ​​- movement speed (default 3)

Attack Range - attack radius (default 2)

Attack Damage - damage (15, as required)

Attack Cooldown - reload between attacks

Detection Range - player detection radius

2. Player setup:


Add the PlayerHealth component to the player object

Make sure the player has the "Player" tag

Configure health parameters

If desired, connect UI elements (Slider for health bar, Text for displaying numbers)
