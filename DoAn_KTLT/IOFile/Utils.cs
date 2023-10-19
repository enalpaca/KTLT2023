using System;
using System.Globalization;
using System.Text;

namespace DoAn_KTLT.IOFile
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

        // https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        public static string RemoveDiacritics(string unicodeString)
        {

            var normalizedString = unicodeString.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static bool StringLike(string str1, string str2)
        {
            string Ascii1 = RemoveDiacritics(str1);
            string Ascii2 = RemoveDiacritics(str2);

            return Ascii1.Contains(Ascii2, StringComparison.OrdinalIgnoreCase);
        }

        public static int CalculateNumberOfPage(int totalRow, int pageSize)
        {
            int totalPage = 0;

            if (totalRow > pageSize)
            {
                totalPage = (int)Math.Ceiling((decimal)(totalRow / pageSize));

                if ((totalRow % pageSize) > 0)
                {
                    totalPage += 1;
                }
            }
            else
            {
                totalPage = 1;
            }

            return totalPage;
        }
    }
}
