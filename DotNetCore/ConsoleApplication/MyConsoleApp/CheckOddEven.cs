namespace MyConsoleApp
{
    public static class CheckOddEven
    {

        public static void Validate()
        {
            //identify odd and even number by using loop and array
            int[] num = { 7, 8, 19, 7, 8, 15, 16, 12, 5, 8, 9, };
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] % 2 == 0)
                {
                    Console.WriteLine("\n Even Number:" + num[i]);
                }
                else
                {
                    Console.WriteLine("\n Odd Number :" + num[i]);
                }
            }
        }
    }
}