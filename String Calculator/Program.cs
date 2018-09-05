using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace String_Calculator
{
    //Declaring EventBroker Class
    public class EventBroker
    {
        //This is list that will store all events
        public List<Event> Events = new List<Event>();

        //This is Commands Event Handler
        public event EventHandler<Command> Commands;

        //This is Query Event Handler
        public event EventHandler<Query> Queries;

        //Declare Command Invoke Method  
        public void Command(Command command) => Commands?.Invoke(this, command);
    }

    //Declaring Event Class
    public class Event
    {

    }

    //Declaring Command Class
    public class Command : EventArgs
    {
        //Declare Calculator and Passed string
        public Calculator Calc;
        public string Numbers;

        //Instantiate properties in constructor 
        public Command(Calculator calc, string numbers)
        {
            Calc = calc;
            Numbers = numbers;
        }
    }

    //Declaring Query Class
    public class Query
    {

    }

    public class Calculator
    {
        //Declaring instance of EventBroker
        EventBroker broker;
        
        //Encapsulating Result variable
        private int Result = 0;

        //Declaring internal List for extraDelimiters
        List<string> extraDelimeters;

        //Constructor, initializing EventBroker
        public Calculator(EventBroker broker)
        {
            //Assign broker 
            this.broker = broker;
            //Declare Delegate
            broker.Commands += Perform;
        }

        private void Perform(object origin, Command command)
        {
            //check if command is present and Calculator object is correct
            if (command != null && command.Calc == this)
            {
                Add(command.Numbers);
            }
        }

        //Add method declaration, as per requirement
        public int Add(string numbers)
        {
            //Define list of Extra Delimiters and add \n
            extraDelimeters = new List<string>();
            extraDelimeters.Add("\\n");

            //define operator
            string op = "+";

            //check if operator provided in for of !+1,2 or !*//[***]\n1***2***3
            if (numbers.Contains("!"))
            {
                //delect operator appropriately , defaulting to addition
                switch (numbers.Substring(1, 1))
                {
                    case "*":
                        op = "*";
                        break;
                    case "/":
                        op = "/";
                        break;
                    case "-":
                        op = "-";
                        break;
                    default:
                        op = "+";
                        break;
                }
                //remove operator substring
                numbers = numbers.Substring(2, numbers.Length - 2);
            }

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
            int result = -1;

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

                //If it is the first operand, then assign to result
                if (result == -1)
                    result = num;
                else
                {

                    //otherwise perform operation
                    if (op == "/" && num != 0)
                        result /= num;
                    else if (op == "*")
                        result *= num;
                    else if (op == "-")
                        result -= num;
                    else
                        result += num;
                }
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
            //Declare new EventBroker instance
            var broker = new EventBroker();
            
            //Instantiate Calculator object
            Calculator cal = new Calculator(broker);

            //Prompt for string 
            Console.Write("Give me string to Calculate?");

            //Take sting
            string s = Console.ReadLine();

            //Invoke command
            broker.Command(new Command(cal, s));

            //Spit out the result
            Console.WriteLine("Result is: " + cal.Add(s));

            //Prompt for Key stroke
            Console.WriteLine("Press any key to continue...");

            //Receive key input
            Console.ReadKey(true);
        }
    }
}
