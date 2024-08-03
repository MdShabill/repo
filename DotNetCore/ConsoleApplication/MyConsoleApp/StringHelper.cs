using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class StringHelper
    {
        //get DomainName from email
        public void GetTotalCountOfString()
        {
            string text = "My N@me Is $h@bill Irf@ni";

            //text = "shabil@gmail.com";
            string[] words = text.Split('@');

            //Console.WriteLine(words[1]);

            int totalWords = words.Length;
            Console.WriteLine("The total count of string is: " + totalWords);

            Console.ReadLine();
        }
    
        //get id from email


        //get domain extentaion from domain name
        public void GetDomainExtentation()
        {
            Console.WriteLine("Enter Web Sit: ");
            string domainExtension = Console.ReadLine();
            string[] Words = domainExtension.Split('.');

            Console.WriteLine("This is your domain extension Name: " + Words[2]);
            Console.ReadLine();
        }


        //print the first occurance of a character


        //print the total occurance of a character

    }
}
