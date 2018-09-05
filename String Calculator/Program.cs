using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace String_Calculator
{
    public class Calculator
    {
        //Declaring for future use
        int Result = 0;
        List<string> extraDelimeters;

        //Add method declaration, as per requirement
        public int Add(string numbers)
        {
            //Define list of Extra Delimiters and add \n
            extraDelimeters = new List<string>();
            extraDelimeters.Add("\\n");

            //Check if line contains extra delimiter
            if (numbers.Contains("//"))
            {
                //cut out delimiter starter
                numbers = numbers.Substring(2, numbers.Length - 2);

                //Determine if complex delimiter and what its end position
                int index = numbers.IndexOf("]");

                if (index == -1)
                {
                    //Add new delimiter to list
                    extraDelimeters.Add(numbers.Substring(0, 1));

                    //cut out delimiter substring 
                    numbers = numbers.Substring(3, numbers.Length - 3);
                }
                else
                {
                    //Call Complex Delimiter Search function
                    numbers = AddComplexDelimiters(numbers, index);
                    
                    //cut out end of delimiter substring 
                    numbers = numbers.Substring(2, numbers.Length - 2);
                }
            }

            //loop through extra delimiters
            foreach (string delim in extraDelimeters)
            {
                //Replace any delimeters with comma
                if (numbers.Contains(delim))
                    numbers = numbers.Replace(delim, ",");
            }

            //if string contains 2 delimeters in a row return 0
            if (numbers.Contains(",,"))
                return 0;

            //Splitting by comma only and it does take unknown amount of numbers
            var numbersArray = numbers.Split(',');
            
            //if string is emtpy, bale with 0
            if (numbersArray.Length == 0)
                return 0;

            //This will store our result
            int result = 0;

            //Instantiate exceptions list and exception message
            List<string> exceptions = new List<string>();
            string exceptionMessage = "negatives not allowed: ";

            //Looping through string, since we only do addition, we dont have to search for operator
            foreach (string n in numbersArray)
            {
                //check if any numbers is negative
                if (n.Contains("-"))
                {
                    //if yes, add to exceptions and continue
                    exceptions.Add(n);
                    continue;
                }
                
                //If input contains anything but 0 1 2 or , return 0
                if (Regex.IsMatch(n, "[^0123]"))
                    return 0;

                //if passed empty string, skip addition
                if (String.IsNullOrEmpty(n) || String.IsNullOrWhiteSpace(n))
                    continue;

                //Converting to int
                int num = Convert.ToInt32(n);

                //if larger than 1000 - ignore
                if (num > 1000)
                    continue;
                
                //otherwise perform addition
                result += num; 
            }

            //check if any exceptions
            if (exceptions.Count > 0)
            {
                //loop through exceptions and append exception message
                foreach (string ex in exceptions)
                    exceptionMessage = exceptionMessage + ex + " ";

                //Zero result
                result = 0;
                //Throw exception
                Console.Write(exceptionMessage);
            }

            //assign result to main var
            this.Result = result;

            //return result
            return this.Result;
        }

        private string AddComplexDelimiters(string numbers, int index)
        {
            //Add new delimiter to list
            extraDelimeters.Add(numbers.Substring(1, index - 1));

            //cut out delimiter substring 
            numbers = numbers.Substring(index + 1, numbers.Length - index - 1);

            //Determine if still present complex delimiter 
            index = numbers.IndexOf("]");

            //Return if no more complex delimiters
            if (index == -1)
                return numbers;
            else
                //Recursive call to find more delimiters
                return AddComplexDelimiters(numbers, index);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //Instantiate Calculator object
            Calculator cal = new Calculator();
            
            //Prompt for string 
            Console.Write("Give me string to Calculate?");

            //Take sting
            string s = Console.ReadLine();

            //Spit out the result
            Console.WriteLine("Result is: " + cal.Add(s));

            //Prompt for Key stroke
            Console.WriteLine("Press any key to continue...");

            //Receive key input
            Console.ReadKey(true);
        }
    }
}
