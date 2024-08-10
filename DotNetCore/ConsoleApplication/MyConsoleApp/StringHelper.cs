using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class StringHelper
    {
        //public void GetTotalCountOfString()
        //{
        //    string text = "My Name Is Shabill Irfani";

        //    int totalCount = text.Length;
        //    Console.WriteLine("The total count of string is: " + totalCount);

        //    Console.ReadLine();
        //}

        ////get id from email
        //public void GetIdFromEmail()
        //{
        //    Console.WriteLine("Enter Your Email: ");
        //    string email = Console.ReadLine();

        //    string[] words = email.Split('@');
        //    Console.WriteLine("This is your email id: "+ words[0]);

        //    Console.ReadLine();
        //}


        ////get domain extension from domain name
        //public void GetDomainExtension()
        //{
        //    Console.WriteLine("Enter Web Sit: ");
        //    string domainExtension = Console.ReadLine();
        //    string[] Words = domainExtension.Split('.');

        //    Console.WriteLine("This is your domain extension Name: " + Words[2]);
        //    Console.ReadLine();
        //}


        ////print the first occurrence of a character with IndexOf() method
        //public void GetFirstOccurrenceOfIndex()
        //{
        //    string text = "My Name is Shabill Irfani";
        //    char characterToFind = 'a';

        //    //The IdexOf() method find the index of first occurrence in given string characters
        //    int index = text.IndexOf(characterToFind);

        //    Console.WriteLine("The first occurrence of character '" + characterToFind + "' is at index: " + index);

        //    Console.ReadLine();
        //}

        //print the first occurrence of a character without IndexOf() method
        public void GetFirstOccurrence()
        {
            string text = "My Name is Shabill Irfani";
            //char characterToFind = 'i';

            Console.WriteLine("Please Enter The Character You Want Find: ");
            string input = Console.ReadLine();

            char characterToFind = input.ToLower()[0];

            //When index is not found then return -1
            int index = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (char.ToLower(text[i]) == characterToFind)
                {
                    index = i;
                    break;
                }
            }

            Console.WriteLine("The first occurrence of character '" + characterToFind + "' is at index: " + index);

            Console.ReadLine();
        }

        //print the total occurance of a character

        public void CountTotalOccurrence()
        {
            string text = "My Name is Shabill Irfani";

            Console.WriteLine("Please Enter The Character You Want Find: ");
            string input = Console.ReadLine();

            char characterToFind = input.ToLower()[0];

            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (char.ToLower(text[i]) == characterToFind)
                {
                    count++;
                }
            }

            Console.WriteLine("The total occurrence of character '" + characterToFind + "' is: " + count);

            Console.ReadLine();
        }
    }
}
