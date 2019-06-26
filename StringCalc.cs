using System;
using System.Collections.Generic;
using System.Linq;

public class StringCalc
{
    static void Main(string[] args)
    {
        var a = new StringCalculator();
        var b = a.Add("1,2\n3,4\n5,1001");
        Console.Write(b.ToString());
        Console.Read();
    }
    public class StringCalculator
    {
        private readonly List<string> _commonDelimiters = new List<string> { ",", "\n" }; // Input Delimiters from Step 1 , and Step 2 \n (new line)
        private const int StartIndexOfNumsWithCustomDelimiter = 3;
        private const int StartIndexOfCustomDelimiter = 2;
        private const int MaxLimit = 1000;
        private const string CustomDelimiters = "//";
        
        // Step 1: Create a simple String calculator with a method: int Add(string numbers) 
        public int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers)) return 0;    // Empty String returns 0 condition check for step 1
                if (numbers.StartsWith(CustomDelimiters))
                {
                    numbers = NumExcludingCustomDelimiter(numbers);
                }
            var sumOfNumbers = SumOfNums(numbers);
            return sumOfNumbers;
        }
        // Step 4 Negative # should throw an exception
        private static void CheckNumberPositiveOrNot(IReadOnlyCollection<int> convertedNums)
        {
            if (!convertedNums.Any(x => x < 0)) return;

            var negativeNums = string.Join(",", convertedNums.Where(x => x < 0).Select(x => x.ToString()).ToArray());
            throw new FormatException($"Negatives not allowed => '{negativeNums}'");
        }

        private int SumOfNums(string numbers)
        {
            var convertedNums = numbers.Split(_commonDelimiters.ToArray(), StringSplitOptions.None).Select(int.Parse).ToList();

            CheckNumberPositiveOrNot(convertedNums);

            var sumOfNumbers = convertedNums.Where(x => x <= MaxLimit).Sum();
            return sumOfNumbers;
        }

        private static IList<string> GetCustomDelimiters(string numbers)
        {
            var allDelimiters = numbers.Substring(StartIndexOfCustomDelimiter, numbers.IndexOf('\n') - StartIndexOfCustomDelimiter);
            var splitDelimiters = allDelimiters.Split('[').Select(x => x.TrimEnd(']')).ToList();

            if (splitDelimiters.Contains(string.Empty))
            {
                splitDelimiters.Remove(string.Empty);
            }
            return splitDelimiters;
        }

        private int SetCustomDelimiterNReturnStartingIndexOfNums(string numbers)
        {
            var customDelimiters = GetCustomDelimiters(numbers);
            _commonDelimiters.AddRange(customDelimiters);

            var hasMultipleDelimiters = customDelimiters.Count > 1;
            var multipleDelimiterLength = hasMultipleDelimiters ? (customDelimiters.Count * 2) : 0;

            return StartIndexOfNumsWithCustomDelimiter + customDelimiters.Sum(x => x.Length) + multipleDelimiterLength;
        }

        private string NumExcludingCustomDelimiter(string numbers)
        {
            var startIndexOfString = SetCustomDelimiterNReturnStartingIndexOfNums(numbers);

            numbers = numbers.Substring(startIndexOfString);
            return numbers;
        }


    }
}
