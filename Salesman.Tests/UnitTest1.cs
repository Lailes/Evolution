using System;
using Xunit;

using Algorithm;
using Xunit.Abstractions;

namespace Salesman.Tests
{

    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            var a = Population.GenerateChromosome(10, new ClassicRandom());
            Assert.Equal(a.Path, a.Path.ConvertToForward().ConvertToNeighbourhood());
        }

        [Fact]
        public void TestCrossover()
        {
            var path = PathLoader.LoadPathTable("C:\\Users\\Auriel\\Desktop\\matrix.txt");
            var a = Population.GenerateChromosome(10, new ClassicRandom()).Path;
            var b = Population.GenerateChromosome(10, new ClassicRandom()).Path;
            var c = GeneticOperators.Cross(a, b, path);
            _testOutputHelper.WriteLine(c.ToString());
        }

        
    }
}