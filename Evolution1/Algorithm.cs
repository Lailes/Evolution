using System;
using System.Collections.Generic;

using Console.AlgorithmEntities;
using ConsoleEvolution.AlgorithmEntities;

namespace ConsoleEvolution;

public class Algorithm
{
    private TargetFunction _function;

    private LogFunction _logFunc;
    private (double Start, double End) _range;

    private double _populationDeviationStopFactor;
    private ushort _initialPopulationSize;
    private ushort _populationNumber = 0;

    public Algorithm
        (
        TargetFunction function,
        LogFunction logFunc, 
        (double Start, double End) range,
        double populationDeviationStopFactor,
        ushort initialPopulationSize
        )
    {
        _function = function ?? throw new ArgumentNullException(nameof(function));
        _logFunc = logFunc ?? throw new ArgumentNullException(nameof(logFunc));
        _range = range.End > range.Start ? range : throw new ArgumentException(nameof(range));
        _populationDeviationStopFactor = populationDeviationStopFactor;
        _initialPopulationSize = initialPopulationSize;
    }

    public double Solve() 
    {
        _populationNumber = 0;
        var count = ushort.MaxValue;

        var length = _range.End - _range.Start;
        var scale = length / count;

        TranslationFunction translation = chromosome => chromosome * scale + _range.Start;

        var population = Population.CreateInitialPopulation(_function, translation, _initialPopulationSize, new ClassicRandom());

        _logFunc.Invoke(new LogInfo("Initial", population.Positions, null, "" , population.Deviation));

        while (population.Deviation > _populationDeviationStopFactor)
        {
            double pD = population.Deviation, pR = population.Result;
            population = population.ProcessNextPopulation();
            _populationNumber++;
            _logFunc.Invoke(new LogInfo
                (
                "Population", 
                population.Positions, 
                _populationNumber,
                $"Before: {pD}\t{pR}, After: {population.Deviation}\t{population.Result}", 
                population.Deviation)
                );
        }

        return population.Result;
    }
}

public delegate double TargetFunction(double x);
public delegate double FitnessFunction(ushort x);
public delegate double TranslationFunction(ushort x);
public delegate void   LogFunction(LogInfo x);

public record LogInfo
    (
    string Title,
    IEnumerable<IndividualPosition> Positions,
    uint? PopulationNumber, 
    string Message,
    double Deviation
    );
