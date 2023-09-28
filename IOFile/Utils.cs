using System;

namespace DoAn1.IOFile
{
    public class Utils
    {
        // https://www.geeksforgeeks.org/c-sharp-randomly-generating-strings/
        public static string GenerateString(int stringlen = 6)
        {
            // Creating object of random class
            Random rand = new Random();

            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {

                // Generating a random number.
                randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                str = str + letter;
            }

            return str;
        }

    }
}
