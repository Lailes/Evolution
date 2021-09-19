using Console.AlgorithmEntities;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEvolution;

internal static class GeneticOperators
{
    public static IList<Individual> ProcessSurviveChance(this IList<Individual> individuals, FitnessFunction fitnessFunction)
    {
        var min = -1 * individuals.Min(individual => fitnessFunction(individual.Chromosome));

        var sequense = individuals
            .Select(individual => new { Individual = individual, Value = fitnessFunction(individual.Chromosome) + min });
        
        var total = sequense.Sum(pair => pair.Value);

        var result = sequense.Select(pair =>
            {
                var individual = pair.Individual;
                individual.SurviveChance = pair.Value / total;
                return individual;
            })
            .ToList();
        return result;
    }

    private const int BarabanSize = 10000;
    public static IList<Individual> Selection(this IList<Individual> individuals, IRandom random, int k = -1)
    {
        var next = new List<Individual>();
        var baraban = new List<Individual>(BarabanSize);

        foreach (var individual in individuals)
        {
            var instanceCount = (int)(individual.SurviveChance * BarabanSize);
            for (int i = 0; i < instanceCount; i++)
                baraban.Add(individual);
        }
        var nextCount = k < 0 ? individuals.Count() : k;
        for (int i = 0; i < nextCount; i++)
            next.Add(baraban[random.Next(0, baraban.Count)]);
        return next;
    }

    public static IList<Individual> Replication(this IList<Individual> individuals, IRandom random, double bornPropability) =>
        Enumerable
             .Range(0, individuals.Count)
             .Select(_ => (individuals[random.Next(0, individuals.Count)], individuals[random.Next(0, individuals.Count)])).ToList()
             .Select(pair => Born(pair.Item1, pair.Item2, random, bornPropability))
             .Where(individual => individual is not Deffictive)
             .Concat(individuals)
             .ToList();
    

    private static (ushort, ushort) Cross(ushort dna1, ushort dna2, int k) =>
         ((ushort)((dna2 & (0b11111111 << k)) | (dna1 & (0b11111111 >> k))),
          (ushort)((dna1 & (0b11111111 << k)) | (dna2 & (0b11111111 >> k))));

    public static Individual Born(Individual Parent1, Individual Parent2, IRandom random, double bornPropability) =>
        bornPropability > random.Next()
        ? new Deffictive()
        : new Individual(Cross(Parent1.Chromosome, Parent2.Chromosome, random.Next(1, 7)).Item1);

    public static IList<Individual> Reduction(this IList<Individual> individuals, IRandom random, int count, FitnessFunction? function = null) =>
      function is null 
        ? Enumerable
            .Range(0, count)
            .Select(_ => individuals[random.Next(0, individuals.Count)])
            .ToList()
        : individuals
            .ProcessSurviveChance(function)
            .Selection(random, count);


    public static IList<Individual> Mutation(this IList<Individual> individuals, IRandom random, double mutationPropability) =>
        individuals
            .Select(individual => Mutate(individual, random, mutationPropability))
            .ToList();

    private static Individual Mutate(Individual individual, IRandom random, double mutationPropability) =>
        random.Next() <= mutationPropability
            ? new Individual(Inverse(individual.Chromosome, random.Next(0, 7)))
            : individual;

    private static ushort Inverse(ushort num, int k) => (ushort) (num ^ (1 << k));
}

