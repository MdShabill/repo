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

            customer.GetMoviesDetail("SRK", "ActorName", "DESC");



            Console.ReadLine();
        }
    }
}