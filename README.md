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

# Standards and Research
This project exists as a prelude to the 'Snake' project found here: [Project-Snake](https://github.com/ScottGarryFoster/PROJECT-Snake) which contains the coding standards for this project and during the course of the Arcade Classics project will be prepared for development.