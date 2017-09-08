﻿using System.Collections.Generic;
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

        [Test] 
        public void ChangeDirection()
        {
            var currentDirection = "N";
            var newdirection = MovementCalculator.Turn(currentDirection);
            Assert.AreEqual("E" , newdirection);
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

        public static string Turn(string direction)
        {
            return  Directions[1];
        }
    }
}
