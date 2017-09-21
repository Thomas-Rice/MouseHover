using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
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
        public string ChangeDirectionClockwise(string input)
        {
            return MovementCalculator.Move(input);
        }

        [TestCase("1,1 N; L", ExpectedResult = "1,1 W")]
        [TestCase("1,1 N; LL", ExpectedResult = "1,1 S")]
        [TestCase("1,1 N; LLL", ExpectedResult = "1,1 E")]
        [TestCase("1,1 N; LLLL", ExpectedResult = "1,1 N")]
        [TestCase("1,1 N; LLLLL", ExpectedResult = "1,1 W")]
        public string ChangeDirectionAntiClockwise(string input)
        {
            return MovementCalculator.Move(input);
        }

        [TestCase("1,1 N; MRM", ExpectedResult = "2,2 E")]
        [TestCase("1,1 N; MMMRRMMM", ExpectedResult = "1,1 S")]
        [TestCase("1,1 N; MMMRRMMMR", ExpectedResult = "1,1 W")]
        [TestCase("1,1 N; MMMRRMMMRMMRM", ExpectedResult = "-1,2 N")]
        [TestCase("1,1 N; MLM", ExpectedResult = "0,2 W")]
        [TestCase("1,1 N; MLMLM", ExpectedResult = "0,1 S")]
        public string MoveTurnMove(string input)
        {
            return MovementCalculator.Move(input);
        }

        [TestCase("1,1 E; MRM", ExpectedResult = "2,0 S")]
        [TestCase("1,2 S; MRM", ExpectedResult = "0,1 W")]
        [TestCase("3,2 W; MRM", ExpectedResult = "2,3 N")]
        [TestCase("3,2 W; MRM", ExpectedResult = "2,3 N")]
        public string MoveTurnMoveFromDifferentStartingDirection(string input)
        {
            return MovementCalculator.Move(input);
        }

        [TestCase("3,2 E; MRMMRM", ExpectedResult = "3,0 W")]
        [TestCase("3,2 E; MRMMRMLM", ExpectedResult = "3,-1 S")]
        public string MoveTurnMoveMoveTurnMoveFromDifferentStartingDirection(string input)
        {
            return MovementCalculator.Move(input);
        }

    }


    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }

        public Position(int x, int y, string direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
    }


    public class Turn
    {
        public string OriginalDirection { get; set; }
        public string FinalDirection { get; set; } = "N";
        public int StepsToMove { get; set; }

        

        internal class Directions
        {
            public static List<string> DirectionsList { get; } = new List<string>()
            {
                "N",
                "E",
                "S",
                "W"
            };

            public static List<string> ReversedDirectionsList { get; } = new List<string>()
            {
                "W",
                "S",
                "E",
                "N"
            };
                
        }

        public Turn(string originalDirection, int stepsToMove)
        {
            OriginalDirection = originalDirection;
            StepsToMove = stepsToMove;
        }

        public string CalculateDirectionToTurn()
        {
            return StepsToMove <= -1 ? CalculateAntiClockwiseDirection() : CalculateClockwiseDirection();
        }

        private string CalculateClockwiseDirection()
        {
            StepsToMove = CalculateIndexToUse(StepsToMove);
            var indexOfNewDirection = Directions.DirectionsList.IndexOf(OriginalDirection) + StepsToMove;
            indexOfNewDirection = CalculateIndexToUse(indexOfNewDirection);
            return Directions.DirectionsList[indexOfNewDirection];
        }

        private string CalculateAntiClockwiseDirection()
        {
            StepsToMove = CalculateStepsToMoveForAntiClockwise();
            var indexOfNewDirection = Directions.ReversedDirectionsList.IndexOf(OriginalDirection) + StepsToMove;
            indexOfNewDirection = CalculateIndexToUse(indexOfNewDirection);
            return Directions.ReversedDirectionsList[indexOfNewDirection];
        }

        private int CalculateStepsToMoveForAntiClockwise()
        {
            StepsToMove = Math.Abs(StepsToMove);
            return CalculateIndexToUse(StepsToMove);
        }

        private static int CalculateIndexToUse(int value)
        {
            if (value > 3)
                value = (value % 4);
            return value;
        }
    }

    public class StringFormatter
    {

        public string InputString { get; set; }
        public List<string> OutputMovementArray { get; set; }

        public StringFormatter(string inputString)
        {
            InputString = inputString;
            OutputMovementArray = new List<string>();
            CreateMovementArray(InputString);
        }


        private void CreateMovementArray(string input)
        {
            var movementString = FormatMovementString(input);
            var tempString = "";
            var previousLetter = "";

            for (var i = 0; i < movementString.Length; i++)
            {
                tempString = AddStringToArray(this.OutputMovementArray, movementString, tempString, previousLetter, i);
                tempString += movementString[i];
                previousLetter = movementString[i].ToString();
            }
            this.OutputMovementArray.Add(tempString);
        }

        private static string FormatMovementString(string input)
        {
            var indexOfFirstMovementCommand = input.IndexOf(';') + 2;
            return input.Substring(indexOfFirstMovementCommand);
        }

        private static string AddStringToArray(List<string> movementArray, string movementString, string tempString, string previousLetter, int i)
        {
            if (previousLetter != movementString[i].ToString() && i != 0)
            {
                movementArray.Add(tempString);
                tempString = "";
            }

            return tempString;
        }


    }

    public static class MovementCalculator
    {
        public static string Move(string input)
        {
            var position = StartingPosition(input);
            var stringFormatter = new StringFormatter(input);
            var movementArray = stringFormatter.OutputMovementArray;

            var finalPosition = CalculateFinalPositionAndDirectionFromArray(position, movementArray);
            return $"{finalPosition.X},{finalPosition.Y} {finalPosition.Direction}";
        }


        private static Position CalculateFinalPositionAndDirectionFromArray(Position position, List<string> movementArray)
        {
            var numberOfStepsToMove = 0;
            var numberOfStepsToTurn = 0;
            var finalDirection = position.Direction;
            var arrayLengthCounter = 0;

            foreach (var command in movementArray)
            {
                var firstLetterOfString = command[0].ToString();
                var lastDirection = finalDirection;

                switch (firstLetterOfString)
                {
                    case "M":
                        numberOfStepsToMove += NumberOfStepsToMove(command);
                        break;
                    case "R":
                    case "L":
                        numberOfStepsToTurn += NumberOfStepsToTurn(command);
                        finalDirection = new Turn(finalDirection, numberOfStepsToTurn).CalculateDirectionToTurn();
                        IncrementMovementAccordingToDirection(position, numberOfStepsToMove, lastDirection);
                        numberOfStepsToMove = 0;
                        break;
                }

                numberOfStepsToTurn = 0;
                arrayLengthCounter += 1;
                if (arrayLengthCounter != movementArray.Count) continue;
                IncrementMovementAccordingToDirection(position, numberOfStepsToMove, finalDirection);
            }

            position.Direction = finalDirection;
            return position;
        }

        private static void IncrementMovementAccordingToDirection(Position position, int numberOfStepsToMove, string finalDirection)
        {

            //var dictionary = new Dictionary<string, int>
            //{
            //    {"E", position.X += numberOfStepsToMove},
            //    {"W", position.X -= numberOfStepsToMove},
            //    {"N", position.Y += numberOfStepsToMove},
            //    {"S", position.Y -= numberOfStepsToMove}
            //};
            //position = dictionary(finalDirection);

            switch (finalDirection)
            {
                case "E":
                    position.X += numberOfStepsToMove;
                    break;
                case "W":
                    position.X -= numberOfStepsToMove;
                    break;
                case "N":
                    position.Y += numberOfStepsToMove;
                    break;
                case "S":
                    position.Y -= numberOfStepsToMove;
                    break;
            }
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
            var direction = input.Split(' ', ';')[1];
            return new Position(x, y, direction);
        }


    }





}