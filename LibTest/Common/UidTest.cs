using HM.Common;
using System.Collections.Concurrent;

namespace LibTest.Common
{
    [TestClass]
    public class UidTest
    {
        [TestMethod]
        public void Common()
        {
            // 直接创建Uid其值应该为0
            Uid uid = new Uid();
            Assert.AreEqual(0ul, uid.Value);
            Assert.AreEqual(uid.ToString(), "00000-00000-00000-00000");

            // 相等性测试
            Uid uidA = new Uid(255);
            Uid uidB = new Uid(255);
            Uid uidC = new Uid(128);
            Assert.IsTrue(uidA == uidB);
            Assert.IsTrue(uidA != uidC);
            Assert.IsTrue(uidA.Equals(uidB));
            Assert.IsFalse(uidA == uidC);
            Assert.IsFalse(uidA != uidB);
            Assert.IsTrue(uidA.GetHashCode() == uidB.GetHashCode());
            Assert.IsTrue(uidA.GetHashCode() != uidC.GetHashCode());
            Assert.IsFalse(uidA.GetHashCode() != uidB.GetHashCode());
            Assert.IsFalse(uidA.GetHashCode() == uidC.GetHashCode());
            Assert.AreEqual((ulong)uidA, 255ul);
            Assert.AreEqual(uidA.Value, 255ul);
            Assert.AreEqual((ulong)uidB, 255ul);
            Assert.AreEqual(uidB.Value, 255ul);
            Assert.AreEqual((ulong)uidC, 128ul);
            Assert.AreEqual(uidC.Value, 128ul);
            Assert.AreEqual(uidA.ToString(), 255ul.ToString("00000-00000-00000-00000"));
            Assert.AreEqual(uidB.ToString(), 255ul.ToString("00000-00000-00000-00000"));
            Assert.AreEqual(uidC.ToString(), 128ul.ToString("00000-00000-00000-00000"));
        }

        [TestMethod]
        public void UidGeneratorTest()
        {
            Uid uid;

            // 默认的Uid生成器
            UidGenerator generator = UidGenerator.Default;
            for (ulong i = 0; i < 100; i++)
            {
                Assert.AreEqual(i, generator.NextIndex);
                uid = generator.Next();
                Assert.AreEqual(i, uid.Value);
            }
            Assert.AreEqual(100ul, generator.NextIndex);

            generator.Fallback(1);
            Assert.AreEqual(99ul, generator.NextIndex);
            uid = generator.Next();
            Assert.AreEqual(99ul, uid.Value);
            Assert.AreEqual(100ul, generator.NextIndex);

            generator.Fallback(2);
            Assert.AreEqual(98ul, generator.NextIndex);
            uid = generator.Next();
            Assert.AreEqual(98ul, uid.Value);
            Assert.AreEqual(99ul, generator.NextIndex);

            generator.Forward(10ul);
            Assert.AreEqual(109ul, generator.NextIndex);
            uid = generator.Next();
            Assert.AreEqual(109ul, uid.Value);
            Assert.AreEqual(110ul, generator.NextIndex);

            generator.Fallback(100);
            Assert.AreEqual(10ul, generator.NextIndex);
            uid = generator.Next();
            Assert.AreEqual(10ul, uid.Value);
            Assert.AreEqual(11ul, generator.NextIndex);

            generator.Fallback(11);
            Assert.AreEqual(0ul, generator.NextIndex);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => generator.Fallback(1));
        }

        [TestMethod]
        public void ConcurrentTest()
        {
            int cycles = 10_0000;
            UidGenerator generator = new UidGenerator(0, true);
            ConcurrentQueue<Uid> queue = new ConcurrentQueue<Uid>();
            Task[] tasks = new Task[cycles];

            for (int i = 0; i < cycles; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var uid = generator.Next();
                    queue.Enqueue(uid);
                });
            }

            Task.WaitAll(tasks);

            Assert.AreEqual(queue.ToHashSet().Count, queue.Count);
        }

        [TestMethod]
        [Timeout(10)]
        public void PerformanceTest()
        {
            // 能10毫秒生成10000个就行
            int size = 10000;
            UidGenerator generator = new UidGenerator();
            Uid[] uids = new Uid[10000];
            for (int i = 0; i < size; i++)
            {
                uids[i] = generator.Next();
            }
        }

        [TestMethod]
        [Timeout(50)]
        public void ConcurrentPerformanceTest()
        {
            // 能50毫秒生成10000个就行
            int size = 10000;
            UidGenerator generator = new UidGenerator();
            Task[] tasks = new Task[size];
            ConcurrentQueue<Uid> queue = new ConcurrentQueue<Uid>();

            for (int i = 0; i < size; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var uid = generator.Next();
                    queue.Enqueue(uid);
                });
            }
        }
    }
}