using System;
using System.Collections.Generic;

namespace mouseHover
{
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
}