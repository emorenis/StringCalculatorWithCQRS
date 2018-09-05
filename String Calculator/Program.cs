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
            //If input contains anything but 0 1 2 or , return 0
            if (Regex.IsMatch(numbers, "[^012,]"))
                return 0;

            //Splitting by comma only
            var numbersArray = numbers.Split(',');
            
            //if string is emtpy, bale with 0
            if (numbersArray.Length == 0)
                return 0;

            //This will store our result
            int result = 0;

            //Looping through string, since we only do addition, we dont have to search for operator
            foreach (string n in numbersArray)
            {
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
            Calculator cal = new Calculator();
            
            //Prompt for string 
            Console.Write("Give me string to Calculate?");

            //Take sting
            string s = Console.ReadLine();

            //Spit out the result
            Console.WriteLine("Result is: " + cal.Add(s));

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey(true);
        }
    }
}
