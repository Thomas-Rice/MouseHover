using NUnit.Framework;


namespace mouseHover
{
    [TestFixture]
    public class MouseHoverShould
    {
        //Logic to do
        // Count how many M's and move that many steps
        // If N Y +1
        // If S Y -1
        // If E X +1
        // If W X -1


        [TestCase("0,0 N; M", ExpectedResult = "0,1 N")]
        [TestCase("0,1 N; M", ExpectedResult = "0,2 N")]
        [TestCase("1,1 N; M", ExpectedResult = "1,2 N")]
        [TestCase("1,1 N; MMM", ExpectedResult = "1,4 N")]
        public string MoveForwardStep(string input)
        {
            return MovementCalculator.Move(input);
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
    }
}
