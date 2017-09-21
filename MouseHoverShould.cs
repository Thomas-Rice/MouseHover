using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


namespace mouseHover
{
    [TestFixture]
    public class MouseHoverShould
    {
        //Logic to do
        // Count how many M's and move that many steps
        // Move in X if W or E


        [TestCase("0,0 N; M", ExpectedResult = "0,1 N")]
        [TestCase("0,1 N; M", ExpectedResult = "0,2 N")]
        [TestCase("1,1 N; M", ExpectedResult = "1,2 N")]
        [TestCase("1,1 N; MMM", ExpectedResult = "1,4 N")]
        public string MoveForwardStep(string input)
        {
            return MovementCalculator.Move(input);
        }


        [TestCase("1,1 N; R", ExpectedResult = "1,1 E")]
        [TestCase("1,1 N; RR", ExpectedResult = "1,1 S")]
        [TestCase("1,1 N; RRR", ExpectedResult = "1,1 W")]
        [TestCase("1,1 N; RRRR", ExpectedResult = "1,1 N")]
        [TestCase("1,1 N; RRRRR", ExpectedResult = "1,1 E")]
        public string ChangeDirectionClockwise(string currentDirection)
        {
            return MovementCalculator.Move(currentDirection);
        }

        [TestCase("1,1 N; L", ExpectedResult = "1,1 W")]
        [TestCase("1,1 N; LL", ExpectedResult = "1,1 S")]
        [TestCase("1,1 N; LLL", ExpectedResult = "1,1 E")]
        [TestCase("1,1 N; LLLL", ExpectedResult = "1,1 N")]
        [TestCase("1,1 N; LLLLL", ExpectedResult = "1,1 W")]
        public string ChangeDirectionAntiClockwise(string currentDirection)
        {
            return MovementCalculator.Move(currentDirection);
        }

        [TestCase("1,1 N; MRM", ExpectedResult = "2,2 E")]
        [TestCase("1,1 N; MMMRRMMM", ExpectedResult = "1,1 S")]
        [TestCase("1,1 N; MMMRRMMMR", ExpectedResult = "1,1 W")]
        [TestCase("1,1 N; MMMRRMMMRMMRM", ExpectedResult = "-1,2 N")]
        public string MoveTurnMove(string currentDirection)
        {
            return MovementCalculator.Move(currentDirection);
        }

    }


    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public class Turn
    {
        public string OriginalDirection { get; set; }
        public string FinalDirection { get; set; } = "N";
        public int StepsToMove { get; set; }

        static readonly List<string> Directions = new List<string>()
        {
            "N",
            "E",
            "S",
            "W"
        };

        static readonly List<string> ReversedDirections = new List<string>()
        {
            "W",
            "S",
            "E",
            "N"
        };

        public Turn(string originalDirection, int stepsToMove)
        {
            OriginalDirection = originalDirection;
            StepsToMove = stepsToMove;
        }

        public string CalculateDirectionToTurn()
        {
            if (StepsToMove <= -1)
            {
                return CalculateAntiClockwiseDirection();
            }
            return CalculateClockwiseDirection();
        }

        private string CalculateClockwiseDirection()
        {
            StepsToMove = CalculateIndexToUse(StepsToMove);
            var indexOfNewDirection = Directions.IndexOf(OriginalDirection) + StepsToMove;
            indexOfNewDirection = CalculateIndexToUse(indexOfNewDirection);
            return Directions[indexOfNewDirection];
        }

        private string CalculateAntiClockwiseDirection()
        {
            StepsToMove = CalculateStepsToMoveForAntiClockwise();
            var indexOfNewDirection = ReversedDirections.IndexOf(OriginalDirection) + StepsToMove;
            indexOfNewDirection = CalculateIndexToUse(indexOfNewDirection);
            return ReversedDirections[indexOfNewDirection];
        }

        private int CalculateStepsToMoveForAntiClockwise()
        {
            StepsToMove = Math.Abs(StepsToMove);
            return CalculateIndexToUse(StepsToMove);
        }

        private int CalculateIndexToUse(int value)
        {
            if (value > 3)
                value = (value % 4);
            return value;
        }
    }




    public static class MovementCalculator
    {
        public static string Move(string input)
        {
            var startingPosition = StartingPosition(input);
            var movementArray = CreateMovementArray(input);
            var startingDirection = StartingDirection(input);


            int numberOfStepsToMove = 0;
            int numberOfStepsToTurn = 0;
            var lastDirection = "";
            var finalDirection = startingDirection;
            var arrayLengthCounter = 0;

            foreach (var command in movementArray)
            {
                var firstLetterOfString = command[0].ToString();
                if (firstLetterOfString == "M")
                {
                    numberOfStepsToMove += NumberOfStepsToMove(command);
                }
                if (firstLetterOfString == "R" || firstLetterOfString == "L")
                {
                    numberOfStepsToTurn += NumberOfStepsToTurn(command);
                    lastDirection = finalDirection;
                    finalDirection = new Turn(finalDirection, numberOfStepsToTurn).CalculateDirectionToTurn();

                    if (lastDirection == "E")
                    {
                        startingPosition.X += numberOfStepsToMove;
                        lastDirection = "E";

                    }
                    if (lastDirection == "N")
                    {
                        startingPosition.Y += numberOfStepsToMove;
                        lastDirection = "N";
                    }
                    if (lastDirection == "S")
                    {
                        startingPosition.Y -= numberOfStepsToMove;
                        lastDirection = "S";
                    }
                    if (lastDirection == "W")
                    {
                        startingPosition.X -= numberOfStepsToMove;
                        lastDirection = "W";
                    }
                    numberOfStepsToMove = 0;
                }
                arrayLengthCounter += 1;
                numberOfStepsToTurn = 0;
                if (arrayLengthCounter == movementArray.Count)
                {
                    if (finalDirection == "E")
                        startingPosition.X += numberOfStepsToMove;
                    else if (finalDirection == "N")
                        startingPosition.Y += numberOfStepsToMove;
                    else if (finalDirection == "S")
                        startingPosition.Y -= numberOfStepsToMove;
                    else if (lastDirection == "" && startingDirection == "N")
                        startingPosition.Y += numberOfStepsToMove;
                }

            }





            return $"{startingPosition.X},{startingPosition.Y} {finalDirection}";
        }

        private static List<string> CreateMovementArray(string input)
        {
            var movementArray = new List<string>();
            var movementString = GetMovementString(input);
            var tempString = "";
            var previousLetter = "";

            for(var i=0; i<movementString.Length; i++)
            {
                if (previousLetter != movementString[i].ToString() && i != 0)
                { 
                    movementArray.Add(tempString);
                    tempString = "";
                }

                if (movementString[i].ToString() == "M")
                    tempString += movementString[i];
                if (movementString[i].ToString() == "R")
                    tempString += movementString[i];
                if (movementString[i].ToString() == "L")
                    tempString += movementString[i];
                previousLetter = movementString[i].ToString();

            }
            movementArray.Add(tempString);
            return movementArray;

        }

        private static string GetMovementString(string input)
        {
            var indexOfFirstMovementCommand = input.IndexOf(';') + 2;
            return input.Substring(indexOfFirstMovementCommand);
        }

        private static int NumberOfStepsToMove(string input)
        {
            var numberOfStepsToMove = 0;
            if (input[0] == 'M')
                numberOfStepsToMove = (input.Substring(0)).Length;
            return numberOfStepsToMove;
        }
        private static int NumberOfStepsToTurn(string input)
        {
            var numberOfStepsToTurn = 0;
            if (input[0] == 'R')
                numberOfStepsToTurn = (input.Substring(0)).Length;
            if (input[0] == 'L')
                numberOfStepsToTurn = ((input.Substring(0)).Length) * -1;
            return numberOfStepsToTurn;
        }

        private static Position StartingPosition(string input)
        {
            var x = int.Parse(input.Split(',')[0]);
            var y = int.Parse(input.Split(',', ' ')[1]);
            return new Position(x, y);
        }

        private static string StartingDirection(string input)
        {
            return input.Split(' ', ';')[1];
        }


    }





}