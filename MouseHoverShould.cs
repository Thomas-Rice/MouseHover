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
            var numberOfStepsToMove = NumberOfStepsToMove(input);
            var numberOfStepsToTurn = NumberOfStepsToTurn(input);
            var startingDirection = StartingDirection(input);

            startingPosition.Y += numberOfStepsToMove;
            //steps to move and directions to turn need to be separate
            var finalDirection = new Turn(startingDirection, numberOfStepsToTurn).CalculateDirectionToTurn();

            return $"{startingPosition.X},{startingPosition.Y} {finalDirection}";
        }


        private static int NumberOfStepsToMove(string input)
        {
            var numberOfStepsToMove = 0;
            var indexOfFirstMovementCommand = input.IndexOf(';') + 2;
            if (input[indexOfFirstMovementCommand] == 'M')
                numberOfStepsToMove = (input.Substring(indexOfFirstMovementCommand)).Length;
            return numberOfStepsToMove;
        }
        private static int NumberOfStepsToTurn(string input)
        {
            var numberOfStepsToTurn = 0;
            var indexOfFirstMovementCommand = input.IndexOf(';') + 2;
            if (input[indexOfFirstMovementCommand] == 'R')
                numberOfStepsToTurn = (input.Substring(indexOfFirstMovementCommand)).Length;
            if (input[indexOfFirstMovementCommand] == 'L')
                numberOfStepsToTurn = ((input.Substring(indexOfFirstMovementCommand)).Length) * -1;
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