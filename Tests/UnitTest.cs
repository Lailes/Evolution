using System.Collections.Generic;

using Xunit;

namespace Tests
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {
            ushort a = 0b10010101;
            ushort b = 0b10010101;
            
            var (a1, b1) = Cross(a, b, 4);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 5);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 6);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 7);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 3);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 2);
            Assert.Equal(a1, b1);

            (a1, b1) = Cross(a, b, 1);
            Assert.Equal(a1, b1);

        }

        private static (ushort, ushort) Cross(ushort dna1, ushort dna2, int k) =>
             ((ushort)((dna2 & (0b11111111 << k)) | (dna1 & (0b11111111 >> k))),
              (ushort)((dna1 & (0b11111111 << k)) | (dna2 & (0b11111111 >> k))));

    }
}