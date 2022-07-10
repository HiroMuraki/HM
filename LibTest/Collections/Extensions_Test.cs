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
        public void MDArrayComparsionTest()
        {
            int[][] jigArrayA =
            {
                new int[] { 1,  2,  3,  4},
                new int[] { 5,  6,  7,  8},
                new int[] { 9,  10, 11, 12},
                new int[] { 13, 14, 15, 16},
            };
            int[][] jigArrayB =
            {
                new int[] { 1,  2,  3,  4},
                new int[] { 5,  6,  7,  8},
                new int[] { 9,  10, 11, 12},
                new int[] { 13, 14, 15, 16},
            };
            int[][] jigArrayC =
            {
                new int[] { 1,  2,  3,  4},
                new int[] { 5,  5,  7,  8},
                new int[] { 9,  10, 11, 12},
                new int[] { 13, 14, 15, 16},
            };
            int[,] mdArrayA =
            {
                { 1,  2,  3,  4},
                { 5,  6,  7,  8},
                { 9,  10, 11, 12},
                { 13, 14, 15, 16},
            };
            int[,] mdArrayB =
            {
                { 1,  2,  3,  4},
                { 5,  6,  7,  8},
                { 9,  10, 11, 12},
                { 13, 14, 15, 16},
            };
            int[,] mdArrayC =
            {
                { 1,  2,  3,  4},
                { 5,  6,  5,  8},
                { 9,  10, 11, 12},
                { 13, 14, 15, 16},
            };

            var comparer = MDArrayComparsion<int>.Default;

            Assert.IsTrue(comparer.TestEquals(mdArrayA, mdArrayA));
            Assert.IsTrue(comparer.TestEquals(mdArrayA, mdArrayB));
            Assert.IsTrue(!comparer.TestEquals(mdArrayA, mdArrayC));

            Assert.IsTrue(comparer.TestEquals(jigArrayA, jigArrayA));
            Assert.IsTrue(comparer.TestEquals(jigArrayA, jigArrayB));
            Assert.IsTrue(!comparer.TestEquals(jigArrayA, jigArrayC));

            Assert.IsTrue(comparer.TestEquals(mdArrayA, jigArrayA));
            Assert.IsTrue(comparer.TestEquals(jigArrayA, mdArrayA));

            Assert.IsTrue(!comparer.TestEquals(jigArrayA, mdArrayC));
            Assert.IsTrue(!comparer.TestEquals(mdArrayA, jigArrayC));
        }

        [TestMethod]
        public void EnumerableExtensionTest()
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

            // Then
            int[] arrayA = { 1, 2, 3, 4, 5 };
            int[] arrayB = { 6, 7, 8, 9, 10 };
            int[] arrayC = { 11, 12, 13, 14, 15 };
            int[] arrayD = { 16, 17, 18, 19, 20 };

            int[] n = arrayA.Then(arrayB, arrayC, arrayD).ToArray();
            Assert.AreEqual(n.Length, 20);
            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(n[i], i + 1);
            }

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
            Assert.IsTrue(phalanx.ToEnumerable().SequenceEqual(target.ToEnumerable()));
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
            Assert.IsTrue(test2dArray.ToEnumerable().SequenceEqual(target2DArray.ToEnumerable()));
        }

        [TestMethod]
        public void LinkedListExtensionTest()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] odds = { 1, 3, 5, 7, 9 };
            int[] evens = { 2, 4, 6, 8, 10 };
            int[] empty = { };
            var list = new LinkedList<int>(array);

            // RemoveIf
            list.RemoveIf(t => t % 2 == 0);
            Assert.AreEqual(odds.Length, list.Count);
            Assert.IsTrue(list.SequenceEqual(odds));

            list = new(empty);
            list.RemoveIf(_ => true);
            Assert.AreEqual(0, list.Count);

            list = new(array);
            Assert.ThrowsException<ArgumentNullException>(() => list.RemoveIf(null!));
        }

        [TestMethod]
        public void ListExtensionTest()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] odds = { 1, 3, 5, 7, 9 };
            int[] evens = { 2, 4, 6, 8, 10 };
            int[] empty = { };
            var list = new List<int>(array);

            // RemoveIf
            list.RemoveIf(t => t % 2 == 0);
            Assert.AreEqual(odds.Length, list.Count);
            Assert.IsTrue(list.SequenceEqual(odds));

            list = new(empty);
            list.RemoveIf(_ => true);
            Assert.AreEqual(0, list.Count);

            list = new(array);
            Assert.ThrowsException<ArgumentNullException>(() => list.RemoveIf(null!));
        }

        [TestMethod]
        public void MDArrayExtensionTest()
        {
            var comparer = MDArrayComparsion<int>.Default;
            int[,] matrix =
            {
                { 1,2,3,4,5 },
                { 6,7,8,9,10 },
                { 11,12,13,14,15 },
                { 16,17,18,19,20 },
                { 21,22,23,24,25 }
            };
            int[,] tMatrix =
            {
                {1,6,11,16,21},
                {2,7,12,17,22},
                {3,8,13,18,23},
                {4,9,14,19,24},
                {5,10,15,20,25}
            };

            // to enumerable
            Assert.IsTrue(matrix.ToEnumerable().SequenceEqual(new int[]
            {
                 1,2,3,4,5,
                 6,7,8,9,10,
                 11,12,13,14,15,
                 16,17,18,19,20,
                 21,22,23,24,25
            }));

            // take rows
            int[][] jigArrayRows =
            {
                new int[] { 1,2,3,4,5 },
                new int[] { 6,7,8,9,10 },
                new int[] { 11,12,13,14,15 },
                new int[] { 16,17,18,19,20 },
                new int[] { 21,22,23,24,25 }
            };
            int[][] rows = matrix.TakeRows().ToArray();
            Assert.AreEqual(jigArrayRows.Length, rows.Length);
            Assert.IsTrue(comparer.TestEquals(rows, jigArrayRows));

            // take columns
            int[][] jigArrayColumns =
            {
                new int[] {1,6,11,16,21},
                new int[] {2,7,12,17,22},
                new int[] {3,8,13,18,23},
                new int[] {4,9,14,19,24},
                new int[] {5,10,15,20,25},
            };
            int[][] columns = matrix.TakeColumns().ToArray();
            Assert.AreEqual(jigArrayColumns.Length, rows.Length);
            Assert.IsTrue(comparer.TestEquals(columns, jigArrayColumns));

            // to jigsaw array
            int[][] jigArray = matrix.ToJigsawArray();
            Assert.IsTrue(comparer.TestEquals(jigArray, jigArrayRows));

            // (to)transpos(ed)
            int[,] transposed = matrix.ToTransposed();
            Assert.IsTrue(comparer.TestEquals(transposed, tMatrix));
            Assert.IsTrue(comparer.TestEquals(matrix, jigArrayRows));
            matrix.Transpose();
            Assert.IsTrue(comparer.TestEquals(matrix, tMatrix));
            matrix.Transpose();
            Assert.IsTrue(comparer.TestEquals(matrix, jigArrayRows));

            // fill
            matrix.Fill(1);
            Assert.IsTrue(Enumerable.Repeat(1, matrix.Length).SequenceEqual(matrix.ToEnumerable()));
            matrix.Fill(0);
            Assert.IsTrue(Enumerable.Repeat(0, matrix.Length).SequenceEqual(matrix.ToEnumerable()));
            int index = 25;
            matrix.Fill(() => index--);
            Assert.IsTrue(comparer.TestEquals(matrix, new int[,]
            {
                {25,24,23,22,21},
                {20,19,18,17,16},
                {15,14,13,12,11},
                {10,9,8,7,6},
                {5,4,3,2,1},
            }));
            index = 1;
            matrix.Fill(() => index++);
            Assert.IsTrue(comparer.TestEquals(matrix, jigArrayRows));

            // shrink
            var t2 = matrix.Shrink(1);
            Assert.IsTrue(comparer.TestEquals(t2, new int[,]
            {
                {7,8,9},
                {12,13,14},
                {17,18,19},
            }));
            var t3 = matrix.Shrink(2);
            Assert.IsTrue(comparer.TestEquals(t3, new int[,]
            {
                {13},
            }));
            var t4 = matrix.Shrink(4);
            Assert.IsTrue(comparer.TestEquals(t4, new int[,] { }));
            var t1 = matrix.Shrink(0, 0, 0, 0);
            Assert.IsTrue(comparer.TestEquals(t1, matrix));
            var t5 = matrix.Shrink(1, 0, 0, 0);
            Assert.IsTrue(comparer.TestEquals(t5, new int[,]
            {
                {2,3,4,5},
                {7,8,9,10},
                {12,13,14,15},
                {17,18,19,20},
                {22,23,24,25},
            }));
            var t6 = matrix.Shrink(0, 1, 0, 0);
            Assert.IsTrue(comparer.TestEquals(t6, new int[,]
            {
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25},
            }));
            var t7 = matrix.Shrink(0, 0, 1, 0);
            Assert.IsTrue(comparer.TestEquals(t7, new int[,]
            {
                {1,2,3,4},
                {6,7,8,9},
                {11,12,13,14},
                {16,17,18,19},
                {21,22,23,24},
            }));
            var t8 = matrix.Shrink(0, 0, 0, 1);
            Assert.IsTrue(comparer.TestEquals(t8, new int[,]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
            }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => matrix.Shrink(-1));

            // expand
            var e1 = matrix.Expand(0, 0, 0, 0);
            Assert.IsTrue(comparer.TestEquals(e1, jigArrayRows));
            var e2 = matrix.Expand(1, 0, 0, 0);
            Assert.IsTrue(comparer.TestEquals(e2, new int[,]
            {
                {0,1,2,3,4,5},
                {0,6,7,8,9,10},
                {0,11,12,13,14,15},
                {0,16,17,18,19,20},
                {0,21,22,23,24,25}
            }));
            var e3 = matrix.Expand(0, 1, 0, 0);
            Assert.IsTrue(comparer.TestEquals(e3, new int[,]
            {
                {0,0,0,0,0},
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25}
            }));
            var e4 = matrix.Expand(0, 0, 1, 0);
            Assert.IsTrue(comparer.TestEquals(e4, new int[,]
            {
                {1,2,3,4,5,0},
                {6,7,8,9,10,0},
                {11,12,13,14,15,0},
                {16,17,18,19,20,0},
                {21,22,23,24,25,0}
            }));
            var e5 = matrix.Expand(0, 0, 0, 1);
            Assert.IsTrue(comparer.TestEquals(e5, new int[,]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25},
                {0,0,0,0,0}
            }));
            var e6 = matrix.Expand(2);
            Assert.IsTrue(comparer.TestEquals(e6, new int[,]
            {
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,1,2,3,4,5,0,0},
                {0,0,6,7,8,9,10,0,0},
                {0,0,11,12,13,14,15,0,0},
                {0,0,16,17,18,19,20,0,0},
                {0,0,21,22,23,24,25,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0}
            }));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => matrix.Expand(-1));
        }
    }
}
