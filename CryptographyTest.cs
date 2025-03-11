using ShakaCoin.Blockchain;

namespace ShakaCoinTests
{
    [TestClass]
    public class CryptographyTest
    {
        [TestMethod]
        public void TestParsePK()
        {

            byte[] sg = Hasher.GetBytesFromHexStringQuick("F406EABC11A53B7B08DBB1002239D3B5028E5C5574807F0BAA73616B73B06D05");

            Assert.IsTrue(Wallet.VerifyPublicKey(sg));
        }

        [TestMethod]
        public void TestSignature()
        {
            string msg = "Test123";
            string privk = "CA1461B5D5F8FDE586C7BC7EA481E988E90CD68754A495EA39C5552EFA0250C8";
            string sig = "8D4D4266B3C5BB55F8F464B27A981108F1716A8CBA61E56A69C814C957F288CB17A49D03A1F352E156254F1185FCF21860CC6191AA3C0FAE6C48F20128F4A601";

            Wallet hk = new Wallet(Hasher.GetBytesFromHexStringQuick(privk));

            Assert.AreEqual(sig, Hasher.GetHexStringQuick(hk.SignData(Hasher.GetBytesQuick(msg))));
        }


        [TestMethod]
        public void TestSignatureTwo()
        {
            string msg = "Test321";
            string pubk = "D06A1F26DB3BA5AEC6AE0848E48AF75D41484B48C6F8A9EABAA1882E648B3DD8";
            string sig = "8413E337A52E4A58A3D3210F7104A3416427A776EC4B181B01510B67CDC2BF906474C4E3ED6C101BA0B5FA8C10BF11D926C448586D323B2FCC9CCA5385A64004";


            Assert.IsTrue(Wallet.VerifySignature(Hasher.GetBytesFromHexStringQuick(sig), Hasher.GetBytesQuick(msg), Hasher.GetBytesFromHexStringQuick(pubk)));
        }
    }
}
