using ShakaCoin.Blockchain;

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
    }
}