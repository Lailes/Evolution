using System.Collections.Generic;

namespace Algorithm
{
    public class Individual
    {
        public IList<int> Path { get; set; }
        public double SurviveChance { get; set; }

        public Individual(IList<int> path) => Path = path;
    }

    public class Deffictive : Individual
    {
        public Deffictive() : base(null) { }
    }
}