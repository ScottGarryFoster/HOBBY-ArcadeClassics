#technical #knowleadgebase #technologies #unity #tdd #testdrivendevelopment 

|Owner|State|Last_update|
|--|--|--|
|@ScottGarryFoster|Development|29th July 2023|

# Summary
It may be easy to claim the advantages of test driven development without showing a re-world advantage. Artificial examples may also be simplistic and not fully show off the techniques and advantages of TDD. To show of one of the major advantages of test driven development this is a use case in development of prototypes of the process of cleaning code after producing a working module.

# Use case
The public method has this behaviour:
1. Find the edges of the given tile map and produce a data structure representing the loops

Rules for loops:
1. Two tiles are upon the same axis (X or Y).
2. The two tiles do not touch
3. The tiles have at least two empty tiles between them

# The API
The method which has been implemented is the first method which would be run from the outside. This method calculates the loops from a tile map. 

Below is the interface:
```cs
public interface ILoopedWorldDiscoveredByTile : IPositionOnWorldCollision  
{  
    /// <summary>  
    /// Calculates the looped positions based on the tilemap.  
    /// </summary>  
    /// <param name="tilemap">Tilemap to look for the loop position. </param>  
    /// <param name="centerTile">The center of the room. </param>  
    /// <param name="borderTile">Tile to look for as the border. </param>  
    /// <param name="widthHeight">The width and height of the room to scan. </param>  
    /// <param name="loopAnswer">Answers or discovered loops. </param>  
    /// <returns>True means there were no issues. </returns>  
    bool CalculateLoops(  
    Tilemap tilemap,  
    Vector3Int centerTile,  
    Tile borderTile,  
    int widthHeight,  
    out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer  
    );
}
```

# Tests
Four grids are used and entered into the tests along with null.

## TestGrid
A grid with many loops.
![[T-KB-T-U-TDD-005.png]]

## TestGrid-CloseBy2
A close grid but not too close.
![[T-KB-T-U-TDD-006.png]]

## TestGrid-CloseBy1
A grid with tiles too close but not all touching. There is a potential for the four middle ones to create loops.
![[T-KB-T-U-TDD-007.png]]

## TestGrid-AllTouching
A grid with everything touching.
![[T-KB-T-U-TDD-008.png]]

## Tests
These are then the tests using these grids.

**CalculateLoops_ReturnsFalse_WhenGivenTilemapIsNullTest**
Test giving null tilemap to the method.

**CalculateLoops_ReturnsTrue_WhenGivenAMapWithBorderTest**
Test the return value flips when given good values.

**CalculateLoops_ReturnsBorderLocations_WhenGivenAMapWithBorderTest**
Checks only the outer dictionary, the location are all correct.

**CalculateLoops_ReturnsCorrectDirections_WhenGivenAMapWithBordersTest**
Checks the entire value of the loop output is correct with a full map.

**CalculateLoops_ReturnsCorrectDirections_WhenBordersHaveTwoEmptyBlocksTest**
Checks the grid output with two empty block gap grid.

**CalculateLoops_DoesNotReturnAnyDirections_WhenBordersHasOneEmptyBlocksTest**
Checks that the locations exist but no loops exist if there is only a single empty block.

**CalculateLoops_DoesNotReturnAnyDirections_WhenAllBlocksAreTouchingTest**
Checks that the locations exist but no loops exist if all blocks touch.

## Example test
To see what an example of a 'good' state looks like this is grid with many loops in.

```cs
[Test]
public void CalculateLoops_ReturnsCorrectDirections_WhenGivenAMapWithBordersTest()
{
    // Arrange
    Tilemap testTilemap = GetTestBorderTileMap(TestGridLocation);
    var borderTile = Resources.Load<Tile>(BorderTileLocation);
    var expectedLocations = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>
    {
        // Right side
        {new Vector2Int(-3, -3), new Dictionary<Direction, CollisionPositionAnswer>()},
        {new Vector2Int(-3, -2), MakeSimpleLoopAnswer(Direction.Left, 1, -2)},
        {new Vector2Int(-3, -1), MakeSimpleLoopAnswer(Direction.Left, 1, -1)},
        {new Vector2Int(-3, 0), MakeSimpleLoopAnswer(Direction.Left, 1, 0)},
        {new Vector2Int(-3, 1), MakeSimpleLoopAnswer(Direction.Left, 1, 1)},
        {new Vector2Int(-3, 2), new Dictionary<Direction, CollisionPositionAnswer>()},
        
        // Left side
        {new Vector2Int(2, -3), new Dictionary<Direction, CollisionPositionAnswer>()},
        {new Vector2Int(2, -2), MakeSimpleLoopAnswer(Direction.Right, -2, -2)},
        {new Vector2Int(2, -1), MakeSimpleLoopAnswer(Direction.Right, -2, -1)},
        {new Vector2Int(2, 0), MakeSimpleLoopAnswer(Direction.Right, -2, 0)},
        {new Vector2Int(2, 1), MakeSimpleLoopAnswer(Direction.Right, -2, 1)},
        {new Vector2Int(2, 2), new Dictionary<Direction, CollisionPositionAnswer>()},
        
        // Top side
        {new Vector2Int(-2, -3), MakeSimpleLoopAnswer(Direction.Up, -2, 1)},
        {new Vector2Int(-1, -3), MakeSimpleLoopAnswer(Direction.Up, -1, 1)},
        {new Vector2Int(0, -3), MakeSimpleLoopAnswer(Direction.Up, 0, 1)},
        {new Vector2Int(1, -3), MakeSimpleLoopAnswer(Direction.Up, 1, 1)},
        
        // Bottom side
        {new Vector2Int(-2, 2), MakeSimpleLoopAnswer(Direction.Down, -2, -2)},
        {new Vector2Int(-1, 2), MakeSimpleLoopAnswer(Direction.Down, -1, -2)},
        {new Vector2Int(0, 2), MakeSimpleLoopAnswer(Direction.Down, 0, -2)},
        {new Vector2Int(1, 2), MakeSimpleLoopAnswer(Direction.Down, 1, -2)}
    };
    
    // Act
    this.testClass.CalculateLoops(testTilemap, new Vector3Int(), borderTile, 10,
        out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> actual);
    
    // Assert
    Assert.AreEqual(expectedLocations.Keys.Count, actual.Keys.Count);
    foreach (var expectedLocation in expectedLocations)
    {
        // Check the keys in the first dictionary
        Assert.IsTrue(actual.ContainsKey(expectedLocation.Key), 
           $"Does not contain {expectedLocation.Key}");
        actual.TryGetValue(expectedLocation.Key, 
            out Dictionary<Direction, CollisionPositionAnswer> actualLocationValue);
        Assert.NotNull(actualLocationValue, 
           $"actualValue is null. {actualLocationValue}");

        foreach (var expectedValue in expectedLocation.Value)
        {
            if (expectedValue.Value.Answer == ContextToPositionAnswer.NoValidMovement)
            {
                continue;
            }
            
            // Check the value in the first dictionary
            Assert.IsTrue(actualLocationValue.ContainsKey(expectedValue.Key),
                $"Does not contain direction {expectedLocation.Key}.{expectedValue.Key}");
            actualLocationValue.TryGetValue(expectedValue.Key, 
                out CollisionPositionAnswer actualCollisionAnswer);
            Assert.NotNull(actualCollisionAnswer, 
                "actualCollisionAnswer is null meaning direction exists but object does not.");
            
            // Check the final bit in the dictionary
            Assert.AreEqual(expectedLocation.Key, actualCollisionAnswer.NewDirection,
                $"Direction in final answer was incorrect. {expectedLocation.Key}.{expectedValue.Key}");
            Assert.AreEqual(ContextToPositionAnswer.NewPositionIsCorrect, actualCollisionAnswer.Answer,
                $"Enum Answer in final answer was incorrect. {expectedLocation.Key}.{expectedValue.Key}");
            Assert.AreEqual(expectedValue.Value.NewPosition, actualCollisionAnswer.NewPosition,
                $"Position in final answer was incorrect. {expectedLocation.Key}.{expectedValue.Key}");
        }
    }
}
```

The reasoning for the many assert types is the more verbose and easily debug-able nature of the output; when using `CollectionAssert.AreEquivalent` the output appeared to be either true or false with an answer which required the debugger.

# Original code
In test driven development the first step is to code the minimum and whatever is needed to create a passing test. These tests were not particularly easy to create small code for however they were easy to create messy code for. The original code is messy, repetitive and in need of the refactor step.

```cs
/// <summary>
/// Discovers which tiles should link together using the tilemap given.
/// </summary>
public class LoopedWorldDiscoveredByTile : LoopingWorld, ILoopedWorldDiscoveredByTile
{
    /// <summary>
    /// Calculates the looped positions based on the tilemap.
    /// </summary>
    /// <param name="tilemap">Tilemap to look for the loop position. </param>
    /// <param name="centerTile">The center of the room. </param>
    /// <param name="widthHeight">The width and height of the room to scan. </param>
    /// <param name="loopAnswer">Answers or discovered loops. </param>
    /// <returns>True means there were no issues. </returns>
    public bool CalculateLoops(
        Tilemap tilemap,
        Vector3Int centerTile,
        Tile borderTile,
        int widthHeight,
        out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer
    )
    {
        bool didCalculate = false;
        loopAnswer = null;
        
        if (tilemap == null)
        {
            Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                                  $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
            return didCalculate;
        }

        int leftLimit = centerTile.x - widthHeight;
        int rightLimit = centerTile.x + widthHeight;
        int lowerLimit = centerTile.y - widthHeight;
        int upperLimit = centerTile.y + widthHeight;
        Bounds searchArea = new Bounds()
        {
            min = new Vector2(leftLimit, lowerLimit),
            max = new Vector2(rightLimit, upperLimit)
        };
        
        loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
        for (int x = leftLimit; x < rightLimit; ++x)
        {
            for (int y = lowerLimit; y < upperLimit; ++y)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, centerTile.z));
                if (tile == borderTile)
                {
                    var position = new Vector2Int(x, y);

                    var positionAnswer = new Dictionary<Direction, CollisionPositionAnswer>();
                    loopAnswer.Add(position, positionAnswer);
                    if (FindLoop(
                            Direction.Up, 
                            new Vector3Int(position.x, position.y, centerTile.y),
                            tilemap,
                            tile,
                            searchArea,
                            out CollisionPositionAnswer answer))
                    {
                        positionAnswer.Add(Direction.Down, answer);
                    }
                    
                    if (FindLoop(
                                 Direction.Down, 
                                 new Vector3Int(position.x, position.y, centerTile.y),
                                 tilemap,
                                 tile,
                                 searchArea,
                                 out CollisionPositionAnswer answer2))
                    {
                        positionAnswer.Add(Direction.Up, answer2);
                    }
                    
                    if (FindLoop(
                                 Direction.Left,
                                 new Vector3Int(position.x, position.y, centerTile.y),
                                 tilemap,
                                 tile,
                                 searchArea,
                                 out CollisionPositionAnswer answer3))
                    {
                        positionAnswer.Add(Direction.Right, answer3);
                    }

                    if (FindLoop(
                            Direction.Right, 
                            new Vector3Int(position.x, position.y, centerTile.y),
                            tilemap,
                            tile,
                            searchArea,
                            out CollisionPositionAnswer answer4))
                    {
                        positionAnswer.Add(Direction.Left, answer4);
                    }
                }
            }
            
        }

        return loopAnswer.Keys.Any();
    }

    private bool FindLoop(
        Direction direction, 
        Vector3Int position,
        Tilemap tilemap,
        TileBase tile,
        Bounds searchArea,
        out CollisionPositionAnswer collisionPositionAnswer)
    {
        collisionPositionAnswer = new CollisionPositionAnswer()
        {
            Answer = ContextToPositionAnswer.NoValidMovement
        };
        bool didFindLoop = false;
        
        if (direction == Direction.Left)
        {
            for (int x = position.x - 1; x < searchArea.min.x; --x)
            {
                TileBase currentTile = tilemap.GetTile(new Vector3Int(x, position.y, position.z));
                if (currentTile == tile)
                {

                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                    collisionPositionAnswer.NewDirection = Direction.Right;
                    collisionPositionAnswer.NewPosition = new Vector2Int(x + 1, position.y);
                    didFindLoop = true;
                    break;
                }
            }
        }
        else if (direction == Direction.Right)
        {
            for (int x = position.x + 1; x < searchArea.max.x; ++x)
            {
                TileBase currentTile = tilemap.GetTile(new Vector3Int(x, position.y, position.z));
                if (currentTile == tile)
                {
                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                    collisionPositionAnswer.NewDirection = Direction.Left;
                    collisionPositionAnswer.NewPosition = new Vector2Int(x - 1, position.y);
                    didFindLoop = true;
                    break;
                }
            }
        }
        else if (direction == Direction.Up)
        {
            for (int y = position.y + 1; y < searchArea.min.y; --y)
            {
                TileBase currentTile = tilemap.GetTile(new Vector3Int(position.x, y, position.z));
                if (currentTile == tile)
                {
                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                    collisionPositionAnswer.NewDirection = Direction.Down;
                    collisionPositionAnswer.NewPosition = new Vector2Int(position.x, y + 1);
                    didFindLoop = true;
                    break;
                }
            }
        }
        else if (direction == Direction.Down)
        {
            for (int y = position.y - 1; y < searchArea.max.y; ++y)
            {
                TileBase currentTile = tilemap.GetTile(new Vector3Int(position.x, y, position.z));
                if (currentTile == tile)
                {
                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                    collisionPositionAnswer.NewDirection = Direction.Up;
                    collisionPositionAnswer.NewPosition = new Vector2Int(position.x, y - 1);
                    didFindLoop = true;
                    break;
                }
            }
        }

        return didFindLoop;
    }
}
```

# Refactoring
Refactoring is the process of changing the internal structure of software to make it easier to understand. Fowler writes in the book Refactoring that refactoring makes code easier to understand without changing it's observable behaviour and that it is the process of applying a series of refactoring's in this process of cleaning.

The refactoring process involves many techniques. Some of which are explained below:
1. Removing duplication
2. Removing Ambiguous or unclear name
3. Reducing the length of methods for comprehension
4. Reducing the parameter list passed into classes or methods
5. Reducing property scope to that which is required
6. Moving logic such that if a collection of methods change for a particular reason or data structure maybe they should be split out into separate implementations
7. Rearranging logic in to a more sensible way. This may begin as the opposite of shorter concise methods and classes with longer larger methods and classes however once similar logic is in a single location it may be easier to re-separate and create a more DRY approach.

For this use case many of these will be used along with the tests.

## Tests and Refactoring
Test driven development as Kent Beck writes in Test Driven Development by Example, involves three stages, write a failing test for which the code should pass in the near future, make the code pass then refactor the code. The Red Green Clean loop ensures that provided the code is under test the code should be clean.

In this instance the code is under test and therefore refactoring may occur because at every stage of refactoring the tests may be rerun to confirm it's behaviour. When refactoring this code the tests will be run and therefore the end result should be much cleaner.

# Refactorings
These are then the refactor steps taken and the reasons why.

## Replace Inline Code with Function Call
This is useful for reducing the size of methods, clarity with the purpose of the code and makes it easier to add future functionality in the future.

The first candidate for this is right at the top of the code:
```cs
bool didCalculate = false;
loopAnswer = null;

if (tilemap == null)
{
    Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                          $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
    return didCalculate;
}
```

This code does not repeat so it is unlike a lot of the other code in the module in terms of refactoring. The goal of the conditional check is to ensure that the parameters are setup correctly however it is not apparent at first look.

The temptation is to add a comment, something like:
```cs
bool didCalculate = false;
loopAnswer = null;

// Validate inputs
if (tilemap == null)
{
    Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                          $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
    return didCalculate;
}
```

However this does not approach the problem head on. Code is written and read, the programmer should be considerate to the reader and ensure it is easier for the reader to both understand and understand when the level of detail is not required. When searching for bugs it is difficult to figure out if code is important or not important. When code is labelled and clear it is easier to figure out where errors of particular types are likely to be. When code is long and complicated the 'jumping off point' or rather the point at when debugging is no longer required is unclear.

Instead of a comment here, a method is far clearer.
The result is the following:
```cs
loopAnswer = null;  
if (!ParametersAreValidForCalculation(tilemap))  
{  
    return false;  
}  
  
bool didCalculate = false;

/// <summary>  
/// Determines if the given parameters would be valid when calculating loops for a world.  
/// </summary>  
/// <param name="tilemap">Tilemap to look for the loop position. </param>  
/// <returns>True means valid. </returns>  
private bool ParametersAreValidForCalculation(Tilemap tilemap)  
{  
    bool isValid = true;  
    if (tilemap == null)  
    {  
        Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +  
        $"{nameof(tilemap)} is null and therefore cannot calculate loops.");  
        isValid = false;  
    }  
      
    return isValid;  
}
```

The next section of code may equally be squirrelled away:
```cs
int leftLimit = centerTile.x - widthHeight;
int rightLimit = centerTile.x + widthHeight;
int lowerLimit = centerTile.y - widthHeight;
int upperLimit = centerTile.y + widthHeight;
Bounds searchArea = new Bounds()
{
    min = new Vector2(leftLimit, lowerLimit),
    max = new Vector2(rightLimit, upperLimit)
};
```

This becomes:
```cs
Bounds searchArea = ExtractWorldArea(centerTile, widthHeight);

/// <summary>  
/// Extracts the bounds of the world from the position and size.  
/// </summary>  
/// <param name="centerTile">Center tile of the world. </param>  
/// <param name="widthHeight">Size of the tile as a width and height. </param>  
/// <returns>The bounds of the world. </returns>  
private Bounds ExtractWorldArea(Vector3Int centerTile, int widthHeight)  
{  
    int leftLimit = centerTile.x - widthHeight;  
    int rightLimit = centerTile.x + widthHeight;  
    int lowerLimit = centerTile.y - widthHeight;  
    int upperLimit = centerTile.y + widthHeight;  
    return new Bounds()  
    {  
        min = new Vector2(leftLimit, lowerLimit),  
        max = new Vector2(rightLimit, upperLimit)  
    };  
}
```

> Fowler, M. and Beck, K. (2019) ‘Chapter 8 Moving Features’, in _Refactoring: Improving the design of existing code_. Boston etc.: Addison-Wesley, p. 222.

## Minimising context switching
Extracting functions to create smaller purposeful ones is a basic step in refactoring. Extraction is not always for duplication, context switching and purpose are reasons to extract methods. Logical information is often best absorbed when context switching is kept low.

The approach of method abstraction is done on this code:
```cs
for (int x = (int)searchArea.min.x; x < (int)searchArea.max.x; ++x)  
{  
    for (int y = (int)searchArea.min.y; y < (int)searchArea.max.y; ++y)  
    {  
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, centerTile.z));  
        if (tile == borderTile)  
        {  
            var position = new Vector2Int(x, y);  
              
            var positionAnswer = new Dictionary<Direction, CollisionPositionAnswer>();  
            loopAnswer.Add(position, positionAnswer);  
            if (FindLoop(  
                Direction.Up,  
                new Vector3Int(position.x, position.y, centerTile.y),  
                tilemap,  
                tile,  
                searchArea,  
                out CollisionPositionAnswer answer))  
                {  
                    positionAnswer.Add(Direction.Down, answer);  
                }
            }
        }
    }
}
```

Loops make a good candidate for method abstraction because the core concept of performing an operation on the singular is fundamentally different than the same to collections. Loops change in that the operations one might do in one collective they might change and do differently in another. Debugging the singular is much easier to debug, read and understand.

Lets look at where we're at and the decisions to make.
```cs
public bool CalculateLoops(  
Tilemap tilemap,  
Vector3Int centerTile,  
Tile borderTile,  
int widthHeight,  
out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer  
)  
{  
    loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();  
    if (!ParametersAreValidForCalculation(tilemap))  
    {  
        return false;  
    }  
      
    loopAnswer = CalculateLoopsForGivenTilemap(tilemap, centerTile, borderTile, widthHeight);  
    return loopAnswer.Keys.Any();  
}  
      
private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> CalculateLoopsForGivenTilemap(  
Tilemap tilemap, Vector3Int centerTile, Tile borderTile, int widthHeight)  
{  
    var loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();  
    Bounds searchArea = ExtractWorldArea(centerTile, widthHeight);  
    for (int x = (int) searchArea.min.x; x < (int) searchArea.max.x; ++x)  
    {  
        for (int y = (int) searchArea.min.y; y < (int) searchArea.max.y; ++y)  
        {  
            if (CalculateLoopsAtGivenTile(tilemap, borderTile,  
                new Vector3Int(x, y, (int) centerTile.z), searchArea,  
                out Dictionary<Direction, CollisionPositionAnswer> tileAnswer))  
            {  
                loopAnswer.Add(new Vector2Int(x,y ), tileAnswer);  
            }  
        }  
    }  
      
    return loopAnswer;  
}  
  
private bool CalculateLoopsAtGivenTile(  
    Tilemap tilemap, Tile borderTile,  
    Vector3Int location, Bounds searchArea, out Dictionary<Direction, CollisionPositionAnswer> loopAnswer)  
{  
    loopAnswer = new Dictionary<Direction, CollisionPositionAnswer>();  
      
    TileBase tile = tilemap.GetTile(location);  
    if (tile != borderTile)  
    {  
        return false;  
    }  
      
    bool isBorder = true;  
    if (FindLoop(  
        Direction.Up,  
        location,  
        tilemap,  
        borderTile,  
        searchArea,  
        out CollisionPositionAnswer answer))  
    {  
        loopAnswer.Add(Direction.Down, answer);  
    }  
      
    if (FindLoop(  
        Direction.Down,  
        location,  
        tilemap,  
        tile,  
        searchArea,  
        out CollisionPositionAnswer answer2))  
    {  
        loopAnswer.Add(Direction.Up, answer2);  
    }  
      
    if (FindLoop(  
        Direction.Left,  
        location,  
        tilemap,  
        tile,  
        searchArea,  
        out CollisionPositionAnswer answer3))  
    {  
        loopAnswer.Add(Direction.Right, answer3);  
    }  
      
    if (FindLoop(  
        Direction.Right,  
        location,  
        tilemap,  
        tile,  
        searchArea,  
        out CollisionPositionAnswer answer4))  
    {  
        loopAnswer.Add(Direction.Left, answer4);  
    }  
      
    return isBorder;  
}
```

There is a question about structure regarding the next step. FindLoop uses the direction to decide the loop type and then operation for the check. We could simply call this with every iteration of the loop however this means that the loop is likely still resolving the direction, it is passing the decision over.

To figure this out let's look at the Find Loop function.

```cs
private bool FindLoop(  
    Direction direction,  
    Vector3Int position,  
    Tilemap tilemap,  
    TileBase tile,  
    Bounds searchArea,  
    out CollisionPositionAnswer collisionPositionAnswer)  
{  
    collisionPositionAnswer = new CollisionPositionAnswer()  
    {  
        Answer = ContextToPositionAnswer.NoValidMovement  
    };  
    bool didFindLoop = false;  
      
    if (direction == Direction.Left)  
    {  
        for (int x = position.x - 1; x < searchArea.min.x; --x)  
        {  
            TileBase currentTile = tilemap.GetTile(new Vector3Int(x, position.y, position.z));  
            if (currentTile == tile)  
            {  
              
                collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;  
                collisionPositionAnswer.NewDirection = Direction.Right;  
                collisionPositionAnswer.NewPosition = new Vector2Int(x + 1, position.y);  
                didFindLoop = true;  
                break;  
        }  
    }  
    
    return didFindLoop;
}
```

So the direction calculation repeats and the logic which changes are the following:
1. The loop changes
2. The new direction when finding a loop changes
3. The new position changes

If I can address all three of these with extra methods then I can have find loop with a direction given and give away the responsibility of deciding how to handle direction to other methods. These are methods I am using to handle these situations:
1. Change the for loop to a while loop - I can use ref or out to make this work
2. Create a method for opposite direction which is the significance of direction
3. Instead of taking the position and moving back, store the position from the last loop, this means that regardless of the direction the loop is moving it should still be the position the head of the snake should go in.

This is the result of this method:
```cs
/// <summary>
/// Calculating loops from the given tile.
/// </summary>
/// <param name="tilemap">Tilemap to search for loops. </param>
/// <param name="borderTile">Border tile to use to search. </param>
/// <param name="location">Location to start. </param>
/// <param name="searchArea">Bounds of the tilemap. </param>
/// <param name="loopAnswer">Answer if successful. </param>
/// <returns>True means there was a border. </returns>
private bool CalculateLoopsAtGivenTile(
    Tilemap tilemap, 
    Tile borderTile,
    Vector3Int location, 
    Bounds searchArea, 
    out Dictionary<Direction, CollisionPositionAnswer> loopAnswer)
{
    loopAnswer = new Dictionary<Direction, CollisionPositionAnswer>();

    TileBase tile = tilemap.GetTile(location);
    if (tile != borderTile)
    {
        return false;
    }
    
    foreach (int i in Enum.GetValues(typeof(Direction)))
    {
        var direction = (Direction) i;
        if (CalculateLoopsAtGivenTileInDirection(
                direction,
                location,
                tilemap,
                borderTile,
                searchArea,
                out CollisionPositionAnswer answer))
        {
            loopAnswer.Add(direction, answer);
        }
    }

    return true;
}

/// <summary>
/// Calculating loops from the given tile in the direction.
/// </summary>
/// <param name="direction">Direction to search in. </param>
/// <param name="position">Position to start. </param>
/// <param name="tilemap">Tilemap to search for loops. </param>
/// <param name="borderTile">Border tile to use to search. </param>
/// <param name="searchArea">Bounds of the tilemap. </param>
/// <param name="collisionPositionAnswer">Answer if successful. </param>
/// <returns>True means there was a loop. </returns>
private bool CalculateLoopsAtGivenTileInDirection(
{
    bool didFindLoop = false;
    collisionPositionAnswer = new CollisionPositionAnswer()
    {
        Answer = ContextToPositionAnswer.NoValidMovement
    };
    
    Vector3Int previousPosition = new Vector3Int();
    Vector3Int loopPosition = new Vector3Int(position.x, position.y, position.z);
    FindLoopLoopSearchAreaSetup(direction, ref loopPosition);
    while (FindLoopLoopSearchArea(direction, searchArea, ref loopPosition))
    {
        if (IsCurrentTileALoop(
                loopPosition, 
                previousPosition, 
                tilemap, 
                borderTile, 
                direction,
                out collisionPositionAnswer))
        {
            didFindLoop = true;
            break;
        };
        previousPosition = loopPosition;
    }
    
    return didFindLoop;
}

/// <summary>
/// Starts the loop to search a given area based.
/// </summary>
/// <param name="direction">Direction to search. </param>
/// <param name="loopPosition">Position to setup. </param>
private void FindLoopLoopSearchAreaSetup(Direction direction, ref Vector3Int loopPosition)
{
    FindLoopLoopSearchArea(direction, new Bounds(), ref loopPosition);
}

/// <summary>
/// Condition when looping the search area.
/// </summary>
/// <param name="direction">Direction to search. </param>
/// <param name="searchArea">Search bound. </param>
/// <param name="loopPosition">Position searching currently. </param>
/// <returns>True means are still searching. </returns>
/// <exception cref="NotImplementedException">
/// Not implemented <see cref="Direction"/>.
/// </exception>
private bool FindLoopLoopSearchArea(Direction direction, Bounds searchArea, ref Vector3Int loopPosition)
{
    switch (direction)
    {
        case Direction.Left:
            --loopPosition.x;
            return loopPosition.x < searchArea.min.x;
        case Direction.Right:
            ++loopPosition.x;
            return loopPosition.x < searchArea.max.x;
        case Direction.Up:
            --loopPosition.y;
            return loopPosition.y < searchArea.min.y;
        case Direction.Down:
            ++loopPosition.y;
            return loopPosition.y < searchArea.max.y;
        default:
            throw new NotImplementedException(
                $"{typeof(LoopedWorldDiscoveredByTile)}: " +
                $"No implementation for {nameof(direction)}");
    }
}

/// <summary>
/// Figures out if the current tile is a border loop.
/// </summary>
/// <param name="currentPosition">Current position to inspect. </param>
/// <param name="previousPosition">Previous position to use in the answer. </param>
/// <param name="tilemap">Tilemap to loop up the position. </param>
/// <param name="borderTile">Border tile to look for. </param>
/// <param name="currentDirection">Current direction moving in. </param>
/// <param name="collisionPositionAnswer">Answer to create. </param>
/// <returns>True means border tile found and <see cref="CollisionPositionAnswer"/> created. </returns>
private bool IsCurrentTileALoop(
    Vector3Int currentPosition, 
    Vector3Int previousPosition, 
    Tilemap tilemap, 
    TileBase borderTile,
    Direction currentDirection,
    out CollisionPositionAnswer collisionPositionAnswer)
{
    bool isLoop = false;
    collisionPositionAnswer = new CollisionPositionAnswer();
    
    TileBase currentTile = tilemap.GetTile(new Vector3Int(currentPosition.x, currentPosition.y, currentPosition.z));
    if (currentTile == borderTile)
    {
        collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
        collisionPositionAnswer.NewDirection = GetOppositeDirection(currentDirection);
        collisionPositionAnswer.NewPosition = new Vector2Int(previousPosition.x, previousPosition.y);
        isLoop = true;
    }

    return isLoop;
}

/// <summary>
/// Returns the opposite direction of the given direction.
/// </summary>
/// <param name="direction">Direction to find the opposite. </param>
/// <returns>Opposite direction of the given. </returns>
/// <exception cref="NotImplementedException">
/// Not implemented <see cref="Direction"/>.
/// </exception>
private Direction GetOppositeDirection(Direction direction)
{
    switch(direction)
    {
        case Direction.Down: return Direction.Up;
        case Direction.Up: return Direction.Down;
        case Direction.Left: return Direction.Right;
        case Direction.Right: return Direction.Left;
        default:
            throw new NotImplementedException(
                $"{typeof(LoopedWorldDiscoveredByTile)}: " +
                $"No implementation for {nameof(direction)}");
    }
}
```

This is certainly not perfect but the result is a small method which does exactly as expected. The tests obviously still pass and the FindLoop method only needs to pass `Direction` into other methods for them to change logic. 

There was also some renames in this when finishing up the file for instance `FindLoop` is now `CalculateLoopsAtGivenTileInDirection`.

> Fowler, M. and Beck, K. (2019) ‘Chapter 6 A first set of refactorings’, in _Refactoring: Improving the design of existing code_. Boston etc.: Addison-Wesley, pp. 106-114.

### Replace Nested Conditional with Guard Clauses
At the start of CalculateLoopsAtGivenTile another technique is used, which is to identify guard clauses. The entire method hinges on the tile given being a border tile, without that fact the rest of the method cannot continue. This is a guard clause and the method to solve it originally was to nest the entire method in the conditional. When reading the rest of the method it is always in the context of the original condition but given nothing else occurs if the conditional fails it is a little pointless. For this reason a guard clause is clearer, cleaner and more direct, it indicates to the programmer this is a requirement.

> Fowler, M. and Beck, K. (2019) ‘Chapter 10 Simplifying conditional logic’, in _Refactoring: Improving the design of existing code_. Boston etc.: Addison-Wesley, pp. 266-271.

# Conclusion
The refactoring techniques are neither all good nor bad, instead there appear to be techniques and uses for them in many situations. It is unclear when refactoring whether one technique blocks another or which techniques should be combined, for instance moving code around may open up the possibilities for other techniques however this is not always obvious. Further refactoring research and use case might be required with examples to narrow down the positions which one technique is preferred over another.