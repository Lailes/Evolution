using Console.AlgorithmEntities;

using ConsoleEvolution.AlgorithmEntities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleEvolution
{
    public class Algorithm
    {
        public TargetFunction Function { get; init; }
        public Target Target { get; init; }

        public Action<LogInfo> LogFunc { get; init; }
        public (double Start, double End) Range { get; init; }
        public double PopulationDeviationStopFactor { get; init; } = 0.1;

        public ushort InitialPopulationSize { get; init; } = 20;
         
        public Task<double> Solve() =>
            Task.Run(() =>
            {
                var count = ushort.MaxValue;

                var length = Range.End - Range.Start;
                var scale = length / count;

                TranslationFunction translation = chromosome => chromosome * scale + Range.Start;

                var population = Population.CreateInitialPopulation(Function, translation, InitialPopulationSize, new ClassicRandom());

                LogFunc.Invoke(new LogInfo("Initial", population.Positions, "Initial Weigths of population (func results)", population.Deviation));

                while (population.Deviation > PopulationDeviationStopFactor)
                {
                    System.Console.Write($"Before: {population.Deviation}\t{population.Result}, ");
                    population = population.ProcessNextPopulation();
                    System.Console.WriteLine($"After: {population.Deviation}\t{population.Result}");
                    LogFunc.Invoke(new LogInfo("Population", population.Positions, "", population.Deviation));

                }

                return population.Result;
            });
    }

    public enum Target : byte { Minimization, Maximization }

    public delegate double TargetFunction(double x);
    public delegate double FitnessFunction(ushort x);
    public delegate double TranslationFunction(ushort x);

    public record LogInfo(string Title, IEnumerable<IndividualPosition> Positions, string Message, double Deviation);
}