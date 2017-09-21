using System.Collections.Generic;

namespace mouseHover
{
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
            var movementString = CreateMovementStringFromInput(input);
            var tempString = "";
            var previousLetter = "";

            for (var i = 0; i < movementString.Length; i++)
            {
                tempString = SplitStringIntoGroupedMovementCommands(movementString, tempString, previousLetter, i);
                tempString += movementString[i];
                previousLetter = movementString[i].ToString();
            }
            OutputMovementArray.Add(tempString);
        }

        private static string CreateMovementStringFromInput(string input)
        {
            var indexOfFirstMovementCommand = input.IndexOf(';') + 2;
            return input.Substring(indexOfFirstMovementCommand);
        }

        private string SplitStringIntoGroupedMovementCommands(string movementString, string tempString, string previousLetter, int i)
        {
            if (previousLetter != movementString[i].ToString() && i != 0)
            {
                OutputMovementArray.Add(tempString);
                tempString = "";
            }

            return tempString;
        }


    }
}