using ShakaCoin.Blockchain;
using ShakaCoin.PaymentData;
using System;
using System.Reflection.Metadata.Ecma335;

namespace ShakaCoinTests
{
    [TestClass]
    public class PaymentTest
    {
        [TestMethod]
        public void TestOutput()
        {

            byte[] pk = Hasher.GetBytesFromHexStringQuick("7EA43F65AC22A74D4FDF05CD9D8D9E69A03352443916F88E4890E511D4FC0BA0");

            Output ox = new Output((ulong)123456789, pk);

            byte[] data = {
            0x7E, 0xA4, 0x3F, 0x65, 0xAC, 0x22, 0xA7, 0x4D,
            0x4F, 0xDF, 0x05, 0xCD, 0x9D, 0x8D, 0x9E, 0x69,
            0xA0, 0x33, 0x52, 0x44, 0x39, 0x16, 0xF8, 0x8E,
            0x48, 0x90, 0xE5, 0x11, 0xD4, 0xFC, 0x0B, 0xA0,
            0x15, 0xCD, 0x5B, 0x07, 0x00, 0x00, 0x00, 0x00
            };

            Assert.AreEqual(Hasher.GetHexStringQuick(ox.ExportToBytes()), Hasher.GetHexStringQuick(data));

        }

        [TestMethod]
        public void TestInput()
        {
            byte[] txH = Hasher.Hash256(Hasher.GetBytesQuick("TestTX"));

            Input ix = new Input(txH, 140);

            ix.AddSignature(Hasher.GetBytesFromHexStringQuick("19E3AB7172AADA7CEE278E2F43326A9CA1F12B36439DD6E2F4E0FB131CBD50ACAAC85CA21F94EF417745E0919634FFE7A2D9E982161F928F136C705B168F350B"));

            byte[] expectedArr = {
                0xC9, 0xB6, 0x00, 0xB8, 0xC1, 0xA5, 0x03, 0x85,
                0xB0, 0xC9, 0x48, 0x86, 0x40, 0x87, 0xBF, 0xEE,
                0xE7, 0xDB, 0x33, 0x0B, 0x40, 0x8E, 0x15, 0xFE,
                0x13, 0xEC, 0x7F, 0x02, 0x02, 0x15, 0xA1, 0x1B,
                0x8C,
                0x19, 0xE3, 0xAB, 0x71, 0x72, 0xAA, 0xDA, 0x7C,
                0xEE, 0x27, 0x8E, 0x2F, 0x43, 0x32, 0x6A, 0x9C,
                0xA1, 0xF1, 0x2B, 0x36, 0x43, 0x9D, 0xD6, 0xE2,
                0xF4, 0xE0, 0xFB, 0x13, 0x1C, 0xBD, 0x50, 0xAC,
                0xAA, 0xC8, 0x5C, 0xA2, 0x1F, 0x94, 0xEF, 0x41,
                0x77, 0x45, 0xE0, 0x91, 0x96, 0x34, 0xFF, 0xE7,
                0xA2, 0xD9, 0xE9, 0x82, 0x16, 0x1F, 0x92, 0x8F,
                0x13, 0x6C, 0x70, 0x5B, 0x16, 0x8F, 0x35, 0x0B
            };

            Assert.AreEqual(Hasher.GetHexStringQuick(ix.GetBytes()), Hasher.GetHexStringQuick(expectedArr));
        }

        [TestMethod]
        public void VerifyInputSignature()
        {
            byte[] txH = Hasher.Hash256(Hasher.GetBytesQuick("TestSignature"));

            Input ix = new Input(txH, 140);

            ix.AddSignature(Hasher.GetBytesFromHexStringQuick("7555EA2E17E4BB244CCE45332D1F5E2C832CC15B286C9C9B97BF7E0A9FCF0897BF58BCD8CC46702756FCA18A9B4831B8AF599C6B67E6EEA10DA9B37153872C0F"));

            Assert.IsTrue(ix.VerifySignature(Hasher.GetBytesFromHexStringQuick("7EA43F65AC22A74D4FDF05CD9D8D9E69A03352443916F88E4890E511D4FC0BA0")));
        }

        [TestMethod]
        public void VerifyCoinbase()
        {
            Input ix = new Input(new byte[32], 0xFF);

            Transaction gTransaction = new Transaction(0b10000000);

            Assert.IsTrue((ix.IsCoinbase() && gTransaction.IsCoinbase()));
        }

        //[TestMethod]
        public void TestFeeCalculation()
        {
            byte[] txH = Hasher.Hash256(Hasher.GetBytesFromHexStringQuick("ABC"));

            Input i0 = new Input(txH, 0);
            Input i1 = new Input(txH, 1);
            Input i2 = new Input(txH, 2);
            Input i3 = new Input(txH, 3);
        }

        [TestMethod]
        public void TestReadWriteWallet()
        {
            FileManagement fm = FileManagement.Instance;

            byte[] examplePrivK = Hasher.GetBytesFromHexStringQuick("CB02D49D0E1DFB5900F4084E470A199DB11CFFE6A0187B2D0FA35A957EC7B9FC");

            fm.AddWallet("exampleWallet0", examplePrivK);

            Assert.AreEqual(Hasher.GetHexStringQuick(fm.ReadWallet("exampleWallet0")), Hasher.GetHexStringQuick(examplePrivK));
        
        }
    }
}
