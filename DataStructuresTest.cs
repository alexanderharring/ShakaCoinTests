using ShakaCoin.Blockchain;
using ShakaCoin.Datastructures;
using ShakaCoin.PaymentData;
using System;

namespace ShakaCoinTests
{
    [TestClass]
    public class ShakaCoinTests
    {
        [TestMethod]
        public void TestOutputBFNegative()
        {
            OutputBloomFilter obf = new OutputBloomFilter();
            
            for (int i = 0; i < 100; i++)
            {
                obf.AddItem(i.ToString());
            }

            Assert.IsFalse(obf.ProbablyContains(100.ToString()));
        }

        [TestMethod]
        public void TestOutputBFTruePositive()
        {
            OutputBloomFilter obf = new OutputBloomFilter();

            for (int i = 0; i < 100; i++)
            {
                obf.AddItem(i.ToString());
            }

            Assert.IsTrue(obf.ProbablyContains(99.ToString()));
        }

        private Transaction generateTransaction()
        {
            Transaction tx = new Transaction(0x00);
            Random rnd = new Random();
            int n = rnd.Next(0, int.MaxValue / 2);
            tx.AddOutput(new Output((ulong)n, Hasher.Hash256(Hasher.GetBytesQuick(n.ToString()))));
            return tx;
        }

        [TestMethod]
        public void TestAVLTree()
        {
            TXNodeAVL root = new TXNodeAVL(new Transaction(0x00));

            Transaction tx42 = new Transaction(0x00);

            for (int i = 0;i < 30;i++)
            {
                Transaction tx = generateTransaction();
                if (i==42)
                {
                    tx42 = tx;
                }

                Console.WriteLine(tx.CalculateFeeRate());
                root.Insert(tx);
            }

            Assert.IsTrue(root.Contains(tx42));
        }

        [TestMethod]
        public void TestAVLTreeMaxHeight()
        {
            TXNodeAVL root = new TXNodeAVL(new Transaction(0x00));

            int N = 2500;

            for (int i = 0; i < N; i++)
            {
                Transaction tx = generateTransaction();

                root.Insert(tx);
            }

            Assert.IsTrue(root.Height <= (int)Math.Floor(1.44042009041 * Math.Log2(N)));
        }

        [TestMethod]
        public void TestMerkleRoot()
        {
            Block nb = new Block();

            for (int i = 0; i < 100; i++)
            {
                nb.AddTransaction(generateTransaction());
            }

            WorkingBlock wb = new WorkingBlock(nb);

            byte[][] ab = wb.GenerateMerkleProof(wb.Transactions[60]);

            byte[] currentHash = Hasher.Hash256(wb.Transactions[60].GetBytes());

            for (int i = 0; i < ab.Length; i++)
            {
                byte[] bigArr = new byte[64];
                Buffer.BlockCopy(Hasher.GetSmallerByteArray(currentHash, ab[i]), 0, bigArr, 0, 32);
                Buffer.BlockCopy(Hasher.GetLargerByteArray(currentHash, ab[i]), 0, bigArr, 32, 32);

                currentHash = Hasher.Hash256(bigArr);
            }

            Assert.AreEqual(Hasher.GetHexStringQuick(currentHash), Hasher.GetHexStringQuick(wb.MerkleRoot));

        }
    }
}
