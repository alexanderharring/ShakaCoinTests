using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakaCoin.Blockchain;
using ShakaCoin.PaymentData;

namespace ShakaCoinTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void TestOutput()
        {
            Output ox = new Output(ulong.MaxValue, Hasher.Hash256([0xFF]));

            Output reconstructed = Parser.ParseOutput(ox.ExportToBytes());

            Assert.AreEqual(Hasher.GetHexStringQuick(ox.ExportToBytes()), Hasher.GetHexStringQuick(reconstructed.ExportToBytes()));
        }

        [TestMethod]
        public void TestInput()
        {

            Input ix = new Input(Hasher.Hash256(Hasher.GetBytesFromHexStringQuick("abcdef")), 2);
            ix.AddSignature(Hasher.Hash512([0xAF]));

            Input ixConstructed = Parser.ParseInput(ix.GetBytes());

            Assert.AreEqual(Hasher.GetHexStringQuick(ix.GetBytes()), Hasher.GetHexStringQuick(ixConstructed.GetBytes()));
        }


        [TestMethod]
        public void TestInputSignature()
        {

            Input ix = new Input(Hasher.Hash256(Hasher.GetBytesFromHexStringQuick("abcdef")), 175);
            ix.AddSignature(Hasher.GetBytesFromHexStringQuick("F7DA27961EBEE56D6594FF5DE74A845D6BC484194DEF607254E54367DCAB5DF1E5D87A65B421AA92CB856DFEA2F9B8D66499910F16CAA5611CB60B7CBA6ED803"));

            Input ixConstructed = Parser.ParseInput(ix.GetBytes());

            Assert.IsTrue(ixConstructed.VerifySignature(Hasher.GetBytesFromHexStringQuick("EBA3D8171C6B2EA4CCFEB4810780816751B26DE66659E5F402D2C5704297E821")));
        }

        private static Transaction generateTransaction()
        {
            Transaction tx = new Transaction(0x00);
            Random rnd = new Random();
            int n = rnd.Next(0, int.MaxValue / 2);
            tx.AddOutput(new Output((ulong)n, Hasher.Hash256(Hasher.GetBytesQuick(n.ToString()))));
            return tx;
        }

        [TestMethod]
        public void TestTransaction()
        {
            Transaction newTx = generateTransaction();

            Transaction recon = Parser.ParseTransaction(newTx.GetBytes());

            Assert.AreEqual(Hasher.GetHexStringQuick(newTx.GetBytes()), Hasher.GetHexStringQuick(recon.GetBytes()));
        }

    }
}
