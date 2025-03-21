using ShakaCoin.Blockchain;
using ShakaCoin.PaymentData;

namespace ShakaCoinTests
{
    [TestClass]
    public class DBTest
    {
        [TestMethod]
        public void TestReadDB()
        {
            FileManagement fm = FileManagement.Instance;

            var key = Hasher.GetBytesQuick("TestKey");
            var value = Hasher.GetBytesQuick("TestValue");

            fm.DBAddValue(key, value);

            var retrieved = fm.DBGetValue(key);

            Assert.AreEqual(Hasher.GetStringQuick(value), Hasher.GetStringQuick(retrieved));

            fm.DBRemoveValue(key);


        }

        [TestMethod]
        public void TestReadNull()
        {
            FileManagement fm = FileManagement.Instance;

            var key = Hasher.GetBytesQuick("TestKey123");

            if (fm.DBGetValue(key) != null)
            {
                fm.DBRemoveValue(key);
            }

            Assert.AreEqual(null, fm.DBGetValue(key));

        }

        [TestMethod]
        public void TestGenesisBlock()
        {
            FileManagement fm = FileManagement.Instance;

            fm.CheckGenesisBlock();
            Block rebuild = Parser.ParseBlock(FileManagement.ReadBlock(0));

            Assert.AreEqual(Hasher.GetHexStringQuick(rebuild.GetBlockHash()), "00000044EC8A5140E339D18684DA294FD36106859123468A9CBC9058B3004664");

        }
    }
}