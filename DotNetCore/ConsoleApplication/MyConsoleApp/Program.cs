using System;

//Console.WriteLine("Hello, World!");
//
////---- Check whether that number exists in array or not -----
//
//int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//
//Console.Write("Enter Number: ");
//
//int inputNumber = int.Parse(Console.ReadLine());
//
////bool exists = false;
//
//for (int i = 0; i < numbers.Length; i++)
//{
//    if (numbers[i] == inputNumber)
//    {
//        //exists = true;
//        Console.WriteLine($"{inputNumber} exist in the array.");
//        return;
//    }
//}
//
//Console.WriteLine($"{inputNumber} does not exist in the array.");

//---Array - Print the first occurrence index of that number----   

int[] numbers = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 10 };

Console.Write("Enter the number: ");
int inputNumber = int.Parse(Console.ReadLine());

for (int i = 0; i < numbers.Length; i++)
{
    if (numbers[i] == inputNumber)
    {
        Console.WriteLine($"{inputNumber} exist in the index {i}");
        return;
    }
}

Console.WriteLine($"{inputNumber} does not exist in the array.");










