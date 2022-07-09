using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HM.Collections.Extensions;
using System.Linq;
using System;

namespace LibTest.Collections
{
    class Foo
    {

    }

    [TestClass]
    public class Extensions_Test
    {
        [TestMethod]
        public void EnumerableExtension()
        {
            // Chunks
            int[] intArray = new int[16]
            {
                1,  2,  3,  4,
                5,  6,  7,  8,
                9,  10, 11, 12,
                13, 14, 15, 16
            };
            var t = new List<int[]>(intArray.Chunks(new int[] { 1, 2, 3, 4, 5, 6 }));
            int[][] target1 = new int[6][]
            {
                new int[]{ 1 },
                new int[]{ 2, 3 },
                new int[]{ 4, 5, 6 },
                new int[]{ 7, 8, 9, 10 },
                new int[]{ 11, 12, 13, 14, 15 },
                new int[]{ 16 }
            };
            for (int i = 0; i < t.Count; i++)
            {
                Assert.IsTrue(t[i].SequenceEqual(target1[i]));
            }

            var t2 = new List<int[]>(intArray.Chunks(new int[] { 4, 4, 4, 4 }));
            int[][] target2 = new int[4][]
            {
                new int[]{ 1,2,3,4 },
                new int[]{ 5,6,7,8 },
                new int[]{ 9,10,11,12 },
                new int[]{ 13,14,15,16 }
            };
            for (int i = 0; i < t2.Count; i++)
            {
                Assert.IsTrue(t2[i].SequenceEqual(target2[i]));
            }

            var t3 = new List<int[]>(intArray.Chunks(new int[] { 5, 4, 3, 2, 1 }));
            int[][] target3 = new int[5][]
            {
                new int[]{ 1, 2, 3, 4, 5 },
                new int[]{ 6, 7, 8, 9 },
                new int[]{ 10,11,12 },
                new int[]{ 13,14 },
                new int[]{ 15 }
            };
            for (int i = 0; i < t3.Count; i++)
            {
                Assert.IsTrue(t3[i].SequenceEqual(target3[i]));
            }

            var t4 = new List<int[]>(intArray.Chunks(new int[] { 0, 0, 16, 0, 0 }));
            int[][] target4 = new int[5][]
            {
                new int[]{ },
                new int[]{ },
                new int[]{ 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 },
                new int[]{ },
                new int[]{ },
            };
            for (int i = 0; i < t4.Count; i++)
            {
                Assert.IsTrue(t4[i].SequenceEqual(target4[i]));
            }

            // ZipWith
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 10, 20, 30, 40, 50 };
            int[] array3 = { 100, 200, 300, 400 };
            int[] array4 = { };

            var zipped1 = array1.ZipWith(array2).ToArray();
            Assert.AreEqual(zipped1.Length, 5);
            for (int i = 0; i < zipped1.Length; i++)
            {
                int[] line;
                switch (i)
                {
                    case 0: line = new int[] { 1, 10 }; break;
                    case 1: line = new int[] { 2, 20 }; break;
                    case 2: line = new int[] { 3, 30 }; break;
                    case 3: line = new int[] { 4, 40 }; break;
                    case 4: line = new int[] { 5, 50 }; break;
                    default: throw new ArgumentException();
                }
                Assert.IsTrue(zipped1[i].SequenceEqual(line));
            }

            var zipped2 = array1.ZipWith(array3).ToArray();
            Assert.AreEqual(zipped2.Length, 4);
            for (int i = 0; i < zipped2.Length; i++)
            {
                int[] line;
                switch (i)
                {
                    case 0: line = new int[] { 1, 100 }; break;
                    case 1: line = new int[] { 2, 200 }; break;
                    case 2: line = new int[] { 3, 300 }; break;
                    case 3: line = new int[] { 4, 400 }; break;
                    default: throw new ArgumentException();
                }
                Assert.IsTrue(zipped2[i].SequenceEqual(line));
            }

            var zipped3 = array1.ZipWith(array4).ToArray();
            Assert.AreEqual(zipped3.Length, 0);
        }

        [TestMethod]
        public void ArrayExtensionTest()
        {
            int[] intArray = new int[16]
            {
                1,  2,  3,  4,
                5,  6,  7,  8,
                9,  10, 11, 12,
                13, 14, 15, 16
            };

            // CopyFrom
            int[] copy = new int[intArray.Length];
            copy.CopyFrom(intArray);
            Assert.IsTrue(intArray.SequenceEqual(copy));
            int[] copy2 = new int[intArray.Length];
            copy2[0] = 255;
            copy2.CopyFrom(intArray, 1, intArray.Length - 1);
            Assert.IsTrue(copy2.SequenceEqual(intArray.SkipLast(1).Prepend(255)));

            // ToPhalanx
            int[,] phalanx = intArray.ToPhalanx();
            int[,] target =
            {
                {1,2,3,4 },
                {5,6,7,8 },
                {9,10,11,12 },
                {13,14,15,16 }
            };
            Assert.AreEqual(phalanx.GetLength(0), 4);
            Assert.IsTrue(phalanx.ElementsEquals(target));
            Assert.ThrowsException<ArgumentException>(() =>
            {
                int[] testArray = { 1, 2, 3, 4, 5, 6 };
                testArray.ToPhalanx();
            });

            // To2DArray
            int[] testArray = { 1, 2, 3, 4, 5, 6 };
            int[,] test2dArray = testArray.To2DArray(2, 3);
            int[,] target2DArray =
            {
                { 1,2,3 },
                { 4,5,6 }
            };
            Assert.AreEqual(test2dArray.GetLength(0), 2);
            Assert.IsTrue(test2dArray.ElementsEquals(target2DArray));
        }
    }
}
