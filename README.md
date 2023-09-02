# Snake Prototype

This project is an attempt to prototype programming in Unity by creating simple arcade games. This project is playground to test out methods and disciplines such as Test Driven Development, Clean Code and Architecture in preparation for larger projects.

For this project it is a prototype of the classic game Snake popularised by Nokia.

## Progress
Snake Game 50%

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

### Adding a Border
A task on the back log came up to add a border and in the fashion of the Nokia classic the idea of a looping border sprung to mind. From this idea came the idea of having any size border, designed by the developer and in the situation this is implemented having an editor tool to see all the loops.

Parts to this update:
1. Creating the Loop discovery code
2. Adding an editor visualiser
3. Adding this code in Game

#### Creating the looping code
Creating this code required gathering an understanding of the Tilemap system in Unity. Using Test Driven Development helped a lot in breaking apart the solution and having the confidence to proceed to other sections which plugged into the implementation. 

**Some elements I had to figure out**:
Tilemaps in Unity are not provided in an array format for instance because their coordinates are incompatible with the general structure, the center of the world appears to be (0, 0) meaning a quarter of the world is entirely in negative co-ordinates. When searching for tiles therefore you use the origin and size to gather this information.

Converting between Vector2Int and Vector3 is annoying. I want to create a utilities class in my next large project to deal with some of these common situations as I found myself continually typing the same things. For this I went WET as to avoid project conflicts and as this is a prototype but in future I should add a Standalone Library for this.

Set tile directly also appeared to work far more often than other methods.

#### Creating the editor visualiser
For this I used the UI Builder to create the User Interface and run the code which would turn on the visuals. The UI Builder is rather similar to WPF XAML and even contains style sheets. The UI Builder uses .uxml which appears to be a similar mark-up to XAML containing the elements of the interface. These UXML files may even contain other UXML files just like XAML making this quite a powerful tool for tool making. The USS stylesheets in this tool offer a way to uniform the styling across all the tools and although not used extensively here are going to prove powerful in uniting the styling. To match up the UI Builder and the actual code the name element was used and picked up on the editor side (code), it seems as though Binding is meant to be used however I struggled to figure out or research a way to use this method at the time (there is a pending task to further research this).

|WPF|UI Builder|
|---|---|
|![WPF Image](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/AddedImages/animation_visualstudio.gif?raw=true)|![UI Builder](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/AddedImages/UIBuilder.PNG?raw=true)|

When creating the visual layer I used the Tilemap as the border was so that both could easily line up. A tile sheet of two types of arrows were created, one for entrances (where the Snake player enters, and one where the Snake player leaves a loop). The visual code takes a Tilemap and given a set border will run the same game code to determine the loops and set the arrows to what it finds.

Due to the fact the visual and borders were Tilemaps I could use Test Driven Development and provide an example map with it's solution for the tests. This meant I could create the solution and clean it up with the knowledge it worked. 

These tests were not without faults however and did require a lot of tweaking throughout the prototyping phase. In future with Unit tests for tile maps it may be preferable to create a module designed for assertions on Unity Tilemaps to avoid some of the pitfalls I fell into during the course of this project (which is still a positive as a key goal of prototyping is discovering these issues in a small project).

#### Adding the Code in Game
When placing a border the looping code accompanies it (in theory a multi-Tilemap version could be created) and this is given to the Snake Behaviour. When moving around the world the Snake uses this to figure out if the location they are moving to is a border tile. The reason this happens as they move to this tile and not via the collision is the ordering in Unity, the ideal place for this is before any Snake Head movement and tail movement so that everything remains in line. Placing this code where it is means that the Snake is teleported to the other side of the loop and the tail code works without any further implementation. To ensure this code remains fast Dictionaries (n(0) lookup) which would not change after the initial scan.

*Food spawning*

Not mentioned here is the food spawning code. When the border is closed the food will still spawn randomly even where the border is or under the player. As a stop gap there was an additional piece of code added in which a layer is added to define 'within the border' for where the food should spawn. This is to be done programmatically in future and not to spawn on the player. The stop gap version may be seen in the 'Larger Game' image below.

*Entrance and Exit toggle*

Separate toggles were an after thought of the project. When using the tool it seemed clearer to visualize one set of the arrows and these were broken out into two sets of arrows. The Flags (bit) with Enums proved useful for this type of data.

#### Results
|Standard Looping|Larger Game|
|--|--|
|![Standard Looping](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/005-LoopsInGame.gif?raw=true)|![Larger Game](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/005-LoopsInGame-Gameplay.gif?raw=true)

|Visual Layer|
|--|
|![Visualisers](https://github.com/ScottGarryFoster/PROJECT-ArcadeClassics/blob/main/Progress/Milestones/006-FurtherVisualiser.gif?raw=true)

### Food Spawning
A task which needs to be address now a border is in place is the food spawning in the correct location. Agile and prototyping in general tends to trend toward the concept of postponing decisions until the decisions are required. There are reasons to bring decisions forward or not but the general idea is that if one makes a decision or write a system to early issues would arise between when the need arise for the system which it is likely not to solve. In the case of food spawning I have implemented a stop gap to test the border code in that you could force the area for Food and therefore force Food only to spawn within the border. This is fine but the decision and system to make this automatic is now because now we have a border and all the other systems to make this automatic. Below were the goals for this system:

1. Food should spawn where ever the player can reach
2. Food should not spawn on the player or tail

#### Step 1. Where does the Player Spawn?
The first thought I had was to use a flood fill algorithm from where the Player spawns however in the current system the player spawns where ever the Prefab spawns. The designer currently does not choose where the Player spawns by default. Therefore the first point of call was to add a Tagged object 'Player' then to make it so that in the Session Controller this is where the player spawns.

|Player Placeholder|In Game|
|--|--|
| ![Add the Player Placeholder to spawn the player](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/blob/main/Progress/Milestones/007-PlayerPlaceholder.png?raw=true)| ![Gameplay of the Player Marker](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/blob/main/Progress/Milestones/007-PlayerPlaceholderGameplay.gif?raw=true)|

#### Step 2. Figure out where the Player can go
For this the first idea I had was the flood fill algorithm. From the player position I should look for free tiles in all four direction (North, East, South, West), add them to a list if free then explore from the queue and add more tiles from the queue. There would also need to be a maximum size for the world and a method to understand where the border is. This task is embeded into the Food at launch to gather the area the food may spawn (before a player/tail check). The image below shows the Flood Fill quite well.

|Flood Fill|
|--|
| ![Span fill Flood Fill](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/blob/main/Progress/AddedImages/007-FloodFillAddedImage.gif?raw=true)|
|[Comparing Flood Fill Algorithms in JavaScript by lukegarrigan](https://codeheir.com/2022/08/21/comparing-flood-fill-algorithms-in-javascript/)| 

#### The problem with Flood Fill
The Flood Fill worked well and used the area of the border tile map to limit the area to search for the area the player can move. This does mean that technically speaking the player can move is larger than where the food can spawn **if the border is open and the player leaves the area**. You cannot see where this is however and there is technically a way to fix this by either offering a leeway to the search given by a number (so the area would be tilemap + some number) but you cannot see it. Therefore the way to combat this first is a new visual, to visually see where the food is spawning then we can address this leeway.

[#24 Add a way to see where the Food Spawns](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/issues/24) | [#25 # Address Areas Player could move in but Food cannot spawn](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/issues/25)

#### Food not spawning on Player
Communication between game objects is increasingly becoming a thing within this project, for instance the border projects World Info and the Food grabs this using Unity Tags. Now the Player also communicates with a new project for communication and via an Action will just update it's location along with tail pieces. The Food piece will use this to not spawn on the player. The only way this is kept separated is via Unity Tags and Get operations which is not ideal in the grand scheme of things. Ideally the creator would set all this up and link everything together because when I do things like the below code what it feels like is a singleton or like a static. Functionally the reasons why we dislike singleton's and statics is because it is difficult to figure out who or what is interacting with it, essentially this is the same situation. I've placed the 'get information' and 'update information' behind two different interfaces and I did consider making an observer pattern here but it seemed like the wrong shape and if it did fit like overkill. I might go back to observers when this arises outside of prototyping.

```csharp
private void HookupStatusToCommunication(ISnakeBehaviour behaviour)  
{  
    GameObject controller = GameObject.FindGameObjectWithTag("GameController");  
    if (controller == null)  
    {  
        Debug.LogError($"{typeof(SnakePlayer)}: No Object with GameController. Cannot update player status.");  
        return;  
    }  
      
    ElementCommunication communication = controller.GetComponent<ElementCommunication>();  
    if (communication == null)  
    {  
        Debug.LogError($"{typeof(SnakePlayer)}: No {nameof(ElementCommunication)}. Cannot update player status.");  
        return;  
    }  
      
    PlayerStatus playerStatus = communication.PlayerStatus;  
    behaviour.UpdatePlayerLocation += playerStatus.UpdateLocation;  
}
```
*This feels like a singleton / static?*

#### Current Results and Reflection

The current results are good and promising, Food does not spawn on top of the player nor border and it is automatic. The Snake Prototype at this stage is at it's most playable and in theory without the other tasks this task brought up I could move on continue.

There are some points which are going to need to be addressed which are likely to not go into a task for instance the food is yet to receive full tests, this should occur with a refactor as some core reasons are the reasoning for this. For instance random numbers, gathering game objects means these need to be Unity Tests not editor tests. These will be addressed and will need to be addressed before this project is 'complete'.

In general though this section of project has shown the holes in my session controller implementation in that it does not fully setup the game objects, the game objects do. In making that decision I caused the singleton like behaviour of using `GameObject` tags to find things. This could have been avoided with a back channel, with `GameElement` having a reference to `Controller` then gaining access to other elements potentially even the border. Probably when doing this 'for real' in the next project I will do this and figure out a way I like for doing this.

It's an alright implementation and I like how separated the `GameElements` are however I can see how in a larger project this may become unmanageable.

![Gameplay without the Food spawning on Player](https://github.com/ScottGarryFoster/PROTOTYPE-Snake/blob/main/Progress/Milestones/007-FoodNotSpawningOnPlayerOrBorder.gif?raw=true)


# Standards and Research
This project exists as a prelude to the 'Snake' project found here: [Project-Snake](https://github.com/ScottGarryFoster/PROJECT-Snake) which contains the coding standards for this project and during the course of the Arcade Classics project will be prepared for development.
