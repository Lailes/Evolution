using System;
using System.Collections.Generic;
using System.Linq;

using Console.AlgorithmEntities;

namespace ConsoleEvolution.AlgorithmEntities;

public class Population
{
    private readonly IList<Individual> _individuals;
    private readonly TargetFunction _func;
    private readonly IRandom _random;
    private readonly TranslationFunction _translationFunction;
    private readonly double _bornPropability = 0.5;
    private readonly double _mutationPropability = 0.0001;

    private Population (TargetFunction func, TranslationFunction chromosomeTranslataor, IRandom random, IList<Individual> individuals) => 
        (_func, _individuals, _translationFunction, _random) = (func, individuals, chromosomeTranslataor, random);

    public static Population CreateInitialPopulation
        (
        TargetFunction func, TranslationFunction 
        chromosomeTranslataor, 
        ushort startCount,
        IRandom random
        ) =>
        new(func, chromosomeTranslataor, random, Enumerable
                .Range(0, startCount)
                .Select(_ => new Individual((ushort)new ClassicRandom().Next(0, ushort.MaxValue)))
                .ToList());


    public double Deviation
    {
        get
        {
            var positions = Positions.ToList();
            var max = positions.Max(pos => pos.X);
            var min = positions.Min(position => position.X);
            return (max - min) / positions.Count;
        }
    }

    public IEnumerable<IndividualPosition> Positions =>
        _individuals
            .Select(individ =>
            {
                var x = _translationFunction(individ.Chromosome);
                var y = _func(x);
                return new IndividualPosition(x, y);
            });

    public double Result =>
        _individuals
            .Select(individual => _func(_translationFunction(individual.Chromosome)))
            .Average();

    public Population ProcessNextPopulation() =>
         new Population(
             _func, 
             _translationFunction, 
             _random, 
             _individuals
                .ProcessSurviveChance(ch => _func(_translationFunction(ch)))
                .GetChanceSelection(_random)
                .Replication(_random, _bornPropability)
                .Mutation(_random, _mutationPropability)
                .Reduction(_random, _individuals.Count, ch => _func(_translationFunction(ch)))
             );
}