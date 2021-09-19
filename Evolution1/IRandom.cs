using System;

namespace ConsoleEvolution
{
    public interface IRandom
    {
        public double Next();
        public int Next(int a , int b);
    }


    public class ClassicRandom : IRandom
    {
        private Random _random = new();

        public double Next() => _random.Next();

        public int Next(int a, int b) => _random.Next(a, b);
    }
}
