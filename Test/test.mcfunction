scoreboard objectives add name dummy 
scoreboard objectives remove name
scoreboard players set @a sa 1
scoreboard players operation @s sa += @s sa
scoreboard players add @s sa 4
scoreboard players operation @s _ *= @s _