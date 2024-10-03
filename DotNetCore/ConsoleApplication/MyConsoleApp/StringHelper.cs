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

        ////print the first occurrence of a character without IndexOf() method
        //public void GetFirstOccurrence()
        //{
        //    string text = "My Name is Shabill Irfani";
        //    //char characterToFind = 'i';

        //    Console.WriteLine("Please Enter The Character You Want Find: ");
        //    string input = Console.ReadLine();

        //    char characterToFind = input.ToLower()[0];

        //    //When index is not found then return -1
        //    int index = -1;

        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        if (char.ToLower(text[i]) == characterToFind)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }

        //    Console.WriteLine("The first occurrence of character '" + characterToFind + "' is at index: " + index);

        //    Console.ReadLine();
        //}

        ////print the total occurance of a character

        //public void CountTotalOccurrence()
        //{
        //    string text = "My Name is Shabill Irfani";

        //    Console.WriteLine("Please Enter The Character You Want Find: ");
        //    string input = Console.ReadLine();

        //    char characterToFind = input.ToLower()[0];

        //    int count = 0;

        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        if (char.ToLower(text[i]) == characterToFind)
        //        {
        //            count++;
        //        }
        //    }

        //    Console.WriteLine("The total occurrence of character '" + characterToFind + "' is: " + count);

        //    Console.ReadLine();
        //}

        ////Find matched elements between array1 and array2

        //public void GetMetchElementBetweenArray1andArray2()
        //{
        //    string[] array1 = { "Salman", "Zahid", "Shabill" };
        //    string[] array2 = { "Shabill", "Shabill", "Salman" };

        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            if (array1[i] == array2[j])
        //            {
        //                Console.WriteLine($"\nMatched Elements: {array1[i]}, Index in Array1: {i}, Index in Array2: {j}");
        //                break;
        //            }
        //        }
        //    }
        //}

        //Find matched elements between array1, array2 and array3

        //public void GetMatchElementsBetween3Array()
        //{
        //    string[] array1 = { "Salman", "Zahid", "Shabill" };
        //    string[] array2 = { "Abid", "Shabill", "Salman" };
        //    string[] array3 = { "Zahid", "Abid", "Shabill" };

        //    //find the common elements between array1 and array2
        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            if (array1[i] == array2[j])
        //            {
        //                //display matched elements between array1 and array2 with index position.
        //                Console.WriteLine($"\nMatched Elements between Array1 and Array2: {array1[i]}, Index in Array1: {i}, Index in Array2: {j}");
        //                break;
        //            }
        //        }
        //    }

        //    //find the common elements in all three arrays and display.
        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            for (int k = 0; k < array3.Length; k++)
        //            {
        //                if (array1[i] == array2[j] && array2[j] == array3[k])
        //                {
        //                    //display matched elements between all three arrays with index position.
        //                    Console.WriteLine($"\nMatched Elements in All Three Arrays: {array1[i]}, Index in Array1: {i}, Index in Array2: {j}, Index in Array3: {k}");
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //public void GetUniqueCharacters()
        //{
        //    Console.WriteLine("Enter Characters: ");
        //    var str = Console.ReadLine();

        //    var uniqueCharacters = "";

        //    Console.WriteLine("\nUnique Characters:");

        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        bool isUnique = true;

        //        // Check if the current character appears earlier in the string
        //        for (int j = 0; j < i; j++)
        //        {
        //            if (str[i] == str[j])
        //            {
        //                isUnique = false;
        //                break;
        //            }
        //        }

        //        // If the character is unique, print it
        //        if (isUnique)
        //        {
        //            uniqueCharacters += str[i];
        //            Console.WriteLine(str[i]);
        //        }
        //    }
        //}

        public void RemoveDuplicateCharacters()
        {
            Console.WriteLine("\nEnter Your Input:");
            var input = Console.ReadLine();

            // Convert the input string to lowercase
            input = input.ToLower();

            var uniqueCharacters = "";

            Console.WriteLine("\nAfter Remove Duplicate Characters:");

            //Iterate over each character in the input string
            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];
                bool isDuplicate = false;

                for (int j = i + 1; j < input.Length; j++)
                {
                    if (input[j] == character)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    uniqueCharacters += character;
                    Console.WriteLine(character);
                }
            }
        }
    }
}
