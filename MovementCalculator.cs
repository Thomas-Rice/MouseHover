using System.Collections.Generic;

namespace mouseHover
{
    public static class MovementCalculator
    {
        public static string Move(string input)
        {
            var movementArray = new StringFormatter(input).OutputMovementArray;

            var position = StartingPosition(input);
            var finalPosition = CalculateFinalPositionAndDirectionFromArray(position, movementArray);

            return $"{finalPosition.X},{finalPosition.Y} {finalPosition.Direction}";
        }


        private static Position CalculateFinalPositionAndDirectionFromArray(Position position, List<string> movementArray)
        {
            var numberOfStepsToMove = 0;
            var numberOfStepsToTurn = 0;
            var updatedDirection = position.Direction;
            var arrayLengthCounter = 0;

            foreach (var movement in movementArray)
            {
                var firstCharInMovement = movement[0].ToString();
                var lastDirection = updatedDirection;

                switch (firstCharInMovement)
                {
                    case "M":
                        numberOfStepsToMove += NumberOfMovementSteps(movement);
                        break;
                    case "R":
                    case "L":
                        numberOfStepsToTurn += NumberOfStepsToTurn(movement);
                        updatedDirection = new DirectionChangeCalculator(updatedDirection, numberOfStepsToTurn).CalculateDirectionToTurn();
                        IncrementMovementAccordingToDirection(position, numberOfStepsToMove, lastDirection);
                        numberOfStepsToMove = 0;
                        break;
                }

                numberOfStepsToTurn = 0;
                arrayLengthCounter += 1;
                if (arrayLengthCounter != movementArray.Count) continue;
                IncrementMovementAccordingToDirection(position, numberOfStepsToMove, updatedDirection);
            }

            position.Direction = updatedDirection;
            return position;
        }

        private static void IncrementMovementAccordingToDirection(Position position, int numberOfStepsToMove, string finalDirection)
        {
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

        private static int NumberOfMovementSteps(string input)
        {
            return input.Substring(0).Length;
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