using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class Population
    {
        private readonly IList<Individual> _individuals;
        private readonly IRandom _random;
        private readonly double _bornPropability;
        private readonly double _mutationPropability;
        private readonly PathTable _pathTable;

        private Population(IList<Individual> individuals, IRandom random, PathTable pathTable, double bornPropability, double mutationPropability) => 
            (_individuals, _random, _pathTable, _bornPropability, _mutationPropability) = (individuals, random,pathTable, bornPropability, mutationPropability);

        public static Population CreateInitialPopulation(int cityCount, int size, IRandom random, PathTable pathTable, double bornPropability, double mutationPropability) => 
            new (
                Enumerable
                    .Range(0, size)
                    .Select(_ => GenerateChromosome(cityCount, random))
                    .ToList(), 
                random, pathTable, bornPropability, mutationPropability
                );


        public double Deviation
        {
            get{
                var nums = _individuals.Select(individual => _pathTable.FitnessFunction(individual.Path)).ToList();
                return nums.Max() - nums.Min();
            }
        }

        public IList<int> Result => 
            _individuals
                .OrderBy(individual => _pathTable.FitnessFunction(individual.Path))
                .First().Path;

        public static Individual GenerateChromosome(int cityCount, IRandom random) => 
            new Individual(
                PathExtensions
                    .GenerateForwardPath(cityCount, random)
                    .ConvertToNeighbourhood()
                );

        public Population ProcessNextPopulation() =>
            new (
                _individuals
                    .ProcessSurviveChance(_pathTable.FitnessFunction)
                    .Selection(_random)
                    .Replication(_random, _pathTable, _bornPropability)
                    .Mutation(_random, _mutationPropability)
                    .Reduction(_random, _individuals.Count)
                    .ToList(), 
                _random, _pathTable, _bornPropability, _mutationPropability
                );

    }

}