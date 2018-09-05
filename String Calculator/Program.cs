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
        
        //Add method declaration, as per requirement
        public int Add(string numbers)
        {
            //Replace new line delimeters with comma
            if (numbers.Contains("\\n"))
                numbers = numbers.Replace("\\n", ",");

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

            //Looping through string, since we only do addition, we dont have to search for operator
            foreach (string n in numbersArray)
            {
                //If input contains anything but 0 1 2 or , return 0
                if (Regex.IsMatch(n, "[^0123]"))
                    return 0;

                //if passed empty string, skip addition
                if (String.IsNullOrEmpty(n) || String.IsNullOrWhiteSpace(n))
                    continue;
                
                //otherwise perform addition
                result += Convert.ToInt32(n); 
            }

            //assign result to main var
            this.Result = result;

            //return result
            return this.Result;
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
