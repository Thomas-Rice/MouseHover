using System;
using System.Collections.Generic;
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
        public string ChangeDirectionClockwise(string currentDirection)
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
        public string DirectionToTurn { get; set; }
        public string FinalDirection { get; set; } = "N";

        static readonly List<string> Directions = new List<string>()
        {
            "N",
            "E",
            "S",
            "W"
        };

        public Turn(string originalDirection, string directionToTurn)
        {
            OriginalDirection = originalDirection;
            DirectionToTurn = directionToTurn;
        }

        public string CalculateDirectionToTurn()
        {
            if (DirectionToTurn.Contains("R"))
                FinalDirection = "E";
            if (DirectionToTurn.Contains("RR"))
                FinalDirection = "S";
            if (DirectionToTurn.Contains("RRR"))
                FinalDirection = "W";
            if (DirectionToTurn.Contains("RRRR"))
                FinalDirection = "N";
            return FinalDirection;
        }

    }




    public static class MovementCalculator
    {
        public static string Move(string input)
        {
            var startingPosition = StartingPosition(input);

            startingPosition.Y += NumberOfStepsToMove(input);

            var finalDirection = new Turn("N", input).CalculateDirectionToTurn();

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

        private static Position StartingPosition(string input)
        {
            var x = int.Parse(input.Split(',')[0]);
            var y = int.Parse(input.Split(',', ' ')[1]);
            return new Position(x, y);
        }


        static readonly List<string> Directions = new List<string>()
        {
            "N",
            "E",
            "S",
            "W"
        };




        public static string TurnAntiClockwise(string currentDirection)
        {
            if (currentDirection == "N")
                return Directions[3];
            var indexOfNewDirection = Directions.IndexOf(currentDirection) - 1;
            return Directions[indexOfNewDirection];
        }

        public static List<string> ParseMovementString(string input)
        {
            List<String> commandList = new List<string>();
            string movementstring = "";

            commandList.Add(ParseOriginalPosition(input));
            var indexOfFirstMovementCommand = input.IndexOf('M');
            var movementCommands = (input.Substring(indexOfFirstMovementCommand));

            foreach (char command in movementCommands)
            {
                if (command == 'M')
                    movementstring += 'M';

                else
                {
                    commandList.Add(movementstring);
                    movementstring = "";
                    if (command == 'N')
                        commandList.Add(command.ToString());
                    if (command == 'E')
                        commandList.Add(command.ToString());
                    if (command == 'S')
                        commandList.Add(command.ToString());
                    if (command == 'W')
                        commandList.Add(command.ToString());
                }

            }
            commandList.Add(movementstring);
            return commandList;
        }

        public static string ParseOriginalPosition(string input)
        {
            var splitCommands = input.Split(';');
            return splitCommands[0];

        }
    }
}