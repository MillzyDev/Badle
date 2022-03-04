using Badle.Wordle;
using System;

namespace Badle.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            string randomWord = Words.Instance.Answers[random.Next(Words.Instance.Answers.Length)];
            Console.WriteLine(randomWord);
        }
    }
}
