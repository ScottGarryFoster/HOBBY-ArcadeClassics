
# Arcade Classics
This project is an attempt to prototype programming in Unity by creating simple arcade games. This project is playground to test out methods and disciplines such as Test Driven Development, Clean Code and Architecture in preparation for larger projects.

## Projects
The planned projects under this repository are as follows:
1. Snake Game (classic Arcade and Nokia game)
2. Pac-man arcade game
3. Space Invaders

## Progress
1. Snake Game 20%
2. Pac-man 0%
3. Space Invades 0%

## Milestones / History
This is a record of notable times in the project a snapshot was taken.
### Snake Movement
The initial player movement took the longest, as this was designed to be quite flexible. The entire system was designed to be testable but refactorable with the player logic contained within SnakePlayer and then shared logic extracted out.
![Snake moving](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/001-SnakePlayerMovement.gif?raw=true)

### Tail Growth
After a lot of unit tests and logic the snake's tail now grows as I would expect it.
Adding this feature was an eye opening experience, not so much for the implementation but the ability to do so using Test Driven Development. Unity's Test Driven Development framework although the most fleshed out of any game platform I've used, has some large caveats and when using Rider did not play well when using certain settings.
These caveats were fully explained in the Snake Project documentation so that I do not fall into them again and the project grows more smoothly in future.
In general the feature appears to work well however the results are not going to see their full potential until animations and images are added.
![Snake tail growth](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/002-TailGrowth.gif?raw=true)

### Death on Tail Collision
There are three parts parts to this update:
1. Scene or Session control to recreate the state at the start of the game
2. Communication between the tail and the head of the tail.
3. Communication to the game controller to trigger death.

#### Game and Scene Controller
There were some choices I wanted to make at this stage. If this project was ever going to have multiple games within it there must not be any dependency between the Snake Game elements and a Scene Controller. The two elements should have references between one another but there should be a level of abstraction in the middle such that one could completely remove Snake Game and the Game Controller would still compile in theory. The implementation I decided upon was a generic 'GameElement'. Everything in the scene which could communicate all the way up to the game or scene controller is a GameElement and then Unity handles spawning these via Prefabs.

#### Responsibilities
Any Game Element could in theory call the 'end of the game' but only the Player usually would do so (maybe a menu in future). Upon this the Game Controller tells everything the session is finished and it is their responsibility to remember how they began (or acquire this information) and return there (Reset). The reasoning is that if every object resets itself it can clean up any objects it created, it can return it's own state and the source of truth is next to the place the Unit tests would be (you can create Unit tests for OnReset).

The long term issues I see with this approach is with Save points where instead of storing the initial point of the object and cleaning up, points at certain times during gameplay would need to be stored. To solve this I would probably move the storage of truth to a save point controller and use that data to reset the object. The game object itself would still be responsible for resetting itself - meaning any privates / protected which a company the public saved properties would remain encapsulated. Saving is complicated, a full TDD (or set of) would be required for this.

#### 	What is happening
In short the Player checks to see if it collides with a Tail piece. The white piece is the tail or 'head'. Upon collision if the piece is a tail then it informs the Game Controller that it is dead using the Action.
```C#
/// <summary>  
/// Occurs on a Trigger Entered in 2D Space.  
/// </summary>  
/// <param name="collider2D">Other collider. </param>  
public void OnTriggerEnter2D(Collider2D collider2D)  
{  
   if (collider2D.CompareTag("SnakeFood"))  
   {  
      collider2D.gameObject.tag = "Untagged";  
      this.growingLag = true;  
   }  
   else if (collider2D.CompareTag("SnakeTail"))  
   {  
      EndTrigger?.Invoke();  
   }  
}
```
The Game controller then removes anything it created after the scene was put into the 'start' position. Then calls reset on everything in existence in the scene.
```C#
private void OnEndTrigger()  
{  
   this.objectsCreatedDuringTheScene.ForEach(x => x.DestroyGameElement?.Invoke());  
   this.objectsCreatedDuringTheScene.ForEach(x => GameObject.Destroy(x.gameObject));  
   this.objectsCreatedDuringTheScene.Clear();  
   this.objectsInTheScene.ForEach(x => x.ResetElement?.Invoke());  
}
```
This essentially resets the entire session. There would be protected methods for session start and the trigger the player sends to the game to signal the game has *started* to allow for more game modes.

This is the result and the the next step in terms of the controllers is to handle score boards and user interface:
![Snake tail growth](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/003-DeathOnTailCollision.gif?raw=true)

# Standards and Research
This project exists as a prelude to the 'Snake' project found here: [Project-Snake](https://github.com/ScottGarryFoster/PROJECT-Snake) which contains the coding standards for this project and during the course of the Arcade Classics project will be prepared for development.