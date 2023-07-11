using System.Security.Cryptography.X509Certificates;

namespace VariableDemo
{
    public class Program
    {
        //string firstName = "Md";
        //string middleName = "Shabill";
        //string lastName = "Irfani";

        //string fruitsBox = "Mango";
        //string SnacksBox = "Chocolate";
        //string dessertsBox = "Baked Cham cham";
        //string clothesBox = "Shirt";

        static void Main(string[] args)
        {
            Customer customer = new();
            customer.GetFullName();


            // supplying hard code value
            //customer.GetFullName_Approach2("Md", "Shabill", "Irfani");
            //customer.GetFullName_Approach2("Md", "Shahrukh", "Irfani");
            //customer.GetFullName_Approach2("Md", "Zahid", "Ahmad");

            //Console.WriteLine("---------------------------\n");

            //Console.WriteLine("Enter First Name");
            //string fName = Console.ReadLine();

            //Console.WriteLine("Enter Middle Name");
            //string mName = Console.ReadLine();

            //Console.WriteLine("Enter Last Name");
            //string lName = Console.ReadLine();

            //customer.GetFullName_Approach2(fName, mName, lName);

            //Console.WriteLine("----------------------------\n");

            //Console.WriteLine("Enter your Full Name");
            //string fullName = Console.ReadLine();

            //Console.WriteLine("Enter your Age");
            //int age = int.Parse(Console.ReadLine());

            //customer.GetNameAndAge(fullName, age);


            //string firstName = "Md";
            //string middleName = "Shabill";
            //string lastName = "Irfani";
            //string fullName = firstName + " " + middleName+ " " + lastName;

            //Console.WriteLine("My Name Is "+ fullName + " Thank You");

            //Console.WriteLine("My Name is  " + firstName + " " + middleName + " " + lastName + " Thank you");

            //Console.WriteLine($"My Name Is {firstName} {middleName} {lastName} Thank You");

            //Console.WriteLine("------------------------------------\n");
            //
            //Console.WriteLine("Enter Your Search Key Word");
            //string searchKeyWord = Console.ReadLine();
            //
            //Console.WriteLine("Enter Your Sort Column Name");
            //string sortColumnName = Console.ReadLine();
            //
            //Console.WriteLine("Enter Your Sort Order");
            //string sortOrder = Console.ReadLine();
            //
            //customer.GetMoviesDetail(searchKeyWord, sortColumnName, sortOrder);

            //---------------------------------------------------------------------

            //Console.WriteLine("Enter First Number \n");
            //int num1 = Convert.ToInt32(Console.ReadLine());
            //
            //Console.WriteLine("Enter Second Number \n");
            //int num2 = Convert.ToInt32(Console.ReadLine());   
            //
            //customer.Add(num1,num2);
            //customer.subtract(num1,num2);
            //customer.multiply(num1,num2);
            //customer.division(num1,num2);



            Console.ReadLine();
        }
    }
}