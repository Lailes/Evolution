using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class CoreAlgorithm
    {

        private readonly PathTable _pathTable;
        private readonly IRandom _random;
        private readonly int _populationSize = 0;
        private readonly double _stopDevaiationFactor;
        private readonly double _bornPropability;
        private readonly double _mutationPropability;
        public CoreAlgorithm(PathTable pathTable, IRandom random, int populationSize, double bornPropability, double mutationPropability, double stopDevaiationFactor) => 
            (_pathTable, _random, _populationSize, _stopDevaiationFactor, _bornPropability, _mutationPropability) 
            = (pathTable, random, populationSize, stopDevaiationFactor, bornPropability, mutationPropability);



        public IList<int> FindPath()
        {
            int iteration;

            var current = Population.CreateInitialPopulation(_pathTable.CityCount, _populationSize, _random, _pathTable, _bornPropability, _mutationPropability);

            for (iteration = 0; _stopDevaiationFactor < 1 ? current.Deviation > _stopDevaiationFactor : iteration < _stopDevaiationFactor ; iteration++)
            {
                current = current.ProcessNextPopulation();
                Console.WriteLine("Iteration: [ " + iteration + " ]: "  + _pathTable.FitnessFunction(current.Result));
            }

            Console.WriteLine("Iterations: " + iteration);
            return current.Result;
        }

    }
}