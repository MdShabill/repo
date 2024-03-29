﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableDemo
{
    public class Customer
    {
        //string firstName = "Md";
        //string middleName = "Shabill";
        //string lastName = "Irfani";

        public void GetFullName()
        {
            string firstName = "Md";
            string middleName = "Shabill";
            string lastName = "Irfani";
            string fullName = firstName + " " + middleName + " " + lastName;

            // - Concatenation
            Console.WriteLine("My Name Is " + fullName + " Thank You");
            Console.WriteLine("My Name is  " + firstName + " " + middleName + " " + lastName + " Thank you");

            //dynamic String using $
            Console.WriteLine($"My Name Is {firstName} {middleName} {lastName} Thank You");
        }


        public void GetFullName_Approach2(string firstName, string middleName, string lastName)
        { 
            Console.WriteLine($"My Name Is {firstName} {middleName} {lastName} Thank You");
        }

        //Fnction - GetNameAndAge
        //Input --- fullName, Age
        //String display - "Hey my full name is ..... and my age is ...."

        public void GetNameAndAge(string fullName, int age)
        {
            Console.WriteLine($"Hey My Full Name Is {fullName} And My Age Is {age}");
        }

        public void GetMoviesDetail(string searchKeyWord, string sortColumnName, string sortOrder)
        {
            string sqlBasicQuery = "Select * From Movies ";

            if (!string.IsNullOrWhiteSpace(searchKeyWord))
                sqlBasicQuery += @"Where ActorName LIKE '%' + @search + '%' 
                                    Or ActressName LIKE '%' + @search + '%' 
                                    Or Title Like '%' + @search + '%' ";

            sqlBasicQuery += $"Order By {sortColumnName} {sortOrder}";

            Console.WriteLine(sqlBasicQuery);
        }

        public void Add(int num1, int num2)
        {
            int total;
            total = num1 + num2;

            Console.WriteLine($"First Parameter Value Is {num1} Second Parameter Value Is {num2} and Addition Result is {total}");
        }

        public void Subtract(int num1, int num2)
        {
            int total;
            total = num1 - num2;

            Console.WriteLine($"First Parameter Value Is {num1} Second Parameter Value Is {num2} and Subtraction Result is {total}");
        }

        public void Multiply(int num1, int num2)
        {
            int total;
            total = num1 * num2;

            Console.WriteLine($"First Parameter Value Is {num1} Second Parameter Value Is {num2} and Multiplication Result is {total}");
        }

        public void Division(int num1, int num2)
        {
            int total;
            total = num1 / num2;

            Console.WriteLine($"First Parameter Value Is {num1} Second Parameter Value Is {num2} and Division Result is {total}");
        }
    }
}
