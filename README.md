# 426_final_you_must_come_home
A README.txt file explaining your design and its rationale, and how your components above come together with respect to gameplay and theme

Design Explanation: This level is supposed to represent our character Rehem's struggle to get to the train station after she recieves a mysterious phone call from an entity holding her mother captive. During this commute, the protagonist encounters a drunk man(mecanim) who is harassing her. She must find projectiles around the map to throw at(physics) the drunk man to stop him. The projectiles position is determined by a genetic algorithm that uses the players location to determine the best spot to have a projectile spawn at(AI). If Rehem fails to hit the man with the projectiles and he hits her, the she has 4 strikes until the game is over regardless of her microsleep or heart attack bar(traps+puzzles). The drunk man's path is an FSM, since he walks from spawn points to spawn point always in the same order, unless he sees the player. If the enemy sees the player he chases the player until they are out of sight and goes back to patrolling(AI). Rehem must also collect the energy drinks to keep herself awake(puzzles+traps). The drink spawn is determined by a Bayesian Algorithm(AI) that takes into account the player's health and energy status and the decided which spawn point to drop the drink at. Rehem's health bar is constantly increasing and decreasing depending on the health state and if the bar reaches microsleep the game is over. Since Rehem is so sleep deprived, she has hallucinations. On the right side of the map, she sees 2 hinge doors(physics) that say SAFE ZONE. If the player chooses to go to the safe zone and close the hinge door, she can block the drunk man from coming in but there are 2 hallucination NPCs inside of the room that scare her(mecanim). 
** Note ** 
There is a bounce physics that prevents the characters from going through walls and the main character is the 3rd mechanim

Theme: The level is supposed to represent the feeling of being trapped and being overwhelmed. The situation is high stakes and every aspect the of the map is meant to inconvienience the player. It's dark, the lamps are flickering, and there's a drunk man harassing you. 

Sound Design 
Deeya - Made theme song for intro, first level theme song + enemy song

Saanvi - Energy Drink Gulp sound

