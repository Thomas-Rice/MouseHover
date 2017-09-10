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


        [TestCase("0,0 N; M", ExpectedResult = "0,1 N")]
        [TestCase("0,1 N; M", ExpectedResult = "0,2 N")]
        [TestCase("1,1 N; M", ExpectedResult = "1,2 N")]
        [TestCase("1,1 N; MMM", ExpectedResult = "1,4 N")]
        public string MoveForwardStep(string input)
        {
            return MovementCalculator.Move(input);
        }


        [TestCase("N", ExpectedResult = "E")]
        [TestCase("E", ExpectedResult = "S")]
        [TestCase("S", ExpectedResult = "W")]
        [TestCase("W", ExpectedResult = "N")]
        public string ChangeDirectionClockwise(string currentDirection)
        {
            return MovementCalculator.TurnClockwise(currentDirection);
        }

        [TestCase("N", ExpectedResult = "W")]
        [TestCase("E", ExpectedResult = "N")]
        [TestCase("S", ExpectedResult = "E")]
        [TestCase("W", ExpectedResult = "S")]
        public string ChangeDirectionAntiClockwise(string currentDirection)
        {
            return MovementCalculator.TurnAntiClockwise(currentDirection);
        }
    }

    public static class MovementCalculator
    {
        public static string Move(string input)
        {
            var startingPositionX = (int)char.GetNumericValue(input[0]);
            var startingPositionY = (int)char.GetNumericValue(input[2]);
            var indexOfFirstMovementCommand = input.IndexOf('M');
            var numberOfStepsToMove = (input.Substring(indexOfFirstMovementCommand)).Length;

            startingPositionY += numberOfStepsToMove;
            return $"{startingPositionX},{startingPositionY} N";
        }

        static readonly List<string> Directions = new List<string>()
        {
            "N",
            "E",
            "S",
            "W"
        };

        public static string TurnClockwise(string currentDirection)
        {
            if(currentDirection == "W")
                return Directions[0];
            var indexOfNewDirection = Directions.IndexOf(currentDirection) + 1;
            return Directions[indexOfNewDirection];
        }

        public static string TurnAntiClockwise(string currentDirection)
        {
            if (currentDirection == "N")
                return Directions[3];
            var indexOfNewDirection = Directions.IndexOf(currentDirection) - 1;
            return Directions[indexOfNewDirection];
        }
    }
}
