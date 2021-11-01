using System.Collections.Generic;
using System.Linq;

using FitnessFunction = System.Func<System.Collections.Generic.IList<int>, double>;
using IndividualList = System.Collections.Generic.IList<Algorithm.Individual>;
using Dna = System.Collections.Generic.IList<int>;

namespace Algorithm
{

    public static class GeneticOperators
    {
        public static IndividualList ProcessSurviveChance(this IndividualList individuals, FitnessFunction fitnessFunction)
        {
            var max = individuals.Max(individual => fitnessFunction(individual.Path));

            var sequence = individuals
                .Select(individual => new
                    { Individual = individual, Value = -1 * (fitnessFunction(individual.Path) - max) })
                .ToList();
            var total = sequence.Sum(pair => pair.Value);
            
            return sequence.Select(pair => {
                    var individual = pair.Individual;
                    individual.SurviveChance = total != 0 ? (pair.Value / total) : (1.0 / sequence.Count);
                    return individual;
                })
                .ToList();
        }

        private const int RoulletSize = 10000;
        public static IndividualList Selection(this IList<Individual> individuals, IRandom random, int k = -1)
        {
            var roullet = individuals
                .SelectMany(individual => Enumerable.Repeat(individual, (int)(individual.SurviveChance * RoulletSize)))
                .ToList();

            return Enumerable
                .Range(0, k < 0 ? individuals.Count : k)
                .Select(_ => roullet[random.Next(0, roullet.Count)])
                .ToList();
        }

        public static IndividualList Replication(this IList<Individual> individuals, IRandom random
            , PathTable pathTable, double bornPropability) =>
            Enumerable
                .Range(0, individuals.Count)
                .Select(_ => (individuals[random.Next(0, individuals.Count)],
                    individuals[random.Next(0, individuals.Count)]))
                .Select(pair => Born(pair.Item1, pair.Item2, pathTable, random, bornPropability))
                .Where(individual => individual is not Deffictive)
                .Concat(individuals)
                .ToList();


        public static IList<int> Cross(Dna dna1, Dna dna2, PathTable pathTable)
        {
            var path1 = dna1.GetPathsNeighbour(pathTable);
            var path2 = dna2.GetPathsNeighbour(pathTable);

            var result = new List<int> { 1 };

            while (result.Count != dna1.Count)
                result.Add(PathExtensions.GetBestWay(
                    path1.FirstOrDefault(path => path != null && path.From == result.Last() && !result.Contains(path.To)),
                    path2.FirstOrDefault(path => path != null && path.From == result.Last() && !result.Contains(path.To))
                )?.To ?? pathTable.GetBestPath(result.Last(), result).To);

            return result.ConvertToNeighbourhood();
        }

        public static Individual Born(Individual parent1, Individual parent2, PathTable pathTable,  IRandom random, double bornPropability) =>
            bornPropability > random.Next()
                ? new Deffictive()
                : new Individual(Cross(parent1.Path, parent2.Path, pathTable));

        public static IndividualList Reduction(this IList<Individual> individuals, IRandom random, int count,
            FitnessFunction? function = null) =>
            function is null
                ? Enumerable
                    .Range(0, count)
                    .Select(_ => individuals[random.Next(0, individuals.Count)])
                    .ToList()
                : individuals
                    .ProcessSurviveChance(function)
                    .Selection(random, count);


        public static IndividualList Mutation(this IndividualList individuals, IRandom random,
            double mutationPropability) =>
            individuals
                .Select(individual => Mutate(individual, random, mutationPropability))
                .ToList();

        private static Individual Mutate(Individual individual, IRandom random, double mutationPropability) =>
            random.Next() <= mutationPropability
                ? new Individual(Inverse(individual.Path, random))
                : individual;

        private static Dna Inverse(Dna dna, IRandom random)
        {
            var a = random.Next(1, dna.Count-1);
            var b = random.Next(1, dna.Count-1);
            var numa = dna[a];
            var numb = dna[b];
            dna[a] = numb;
            dna[b] = numa;
            return dna;
        }
    }
}

