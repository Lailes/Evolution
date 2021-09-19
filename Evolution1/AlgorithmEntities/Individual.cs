using System;

namespace Console.AlgorithmEntities;

public record Individual(ushort Chromosome)
{
    public double SurviveChance { get; set; }
}

public record Deffictive() : Individual(0);

public record IndividualPosition(double X, double Y);
