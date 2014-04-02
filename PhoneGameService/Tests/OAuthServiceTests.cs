using NUnit.Framework;
using PhoneGameService.Models;
using PhoneGameService.Models.OAuthTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameService.Tests
{
    [TestFixture]
    public class OAuthServiceTests
    {
        public string code = "4/keE9qlVlUqpCoiUwbeV9Ney6xG6m.MvVCr9h3WYAVmmS0T3UFEsPUrX4pigI";
        public EncryptedToken encryptedToken = null;
        public UnEncryptedToken token = null;
        public HashedToken hashedToken = null;
        
        [TestFixtureSetUp]
        public void SetupTests()
        {
          
        }

        [TestFixtureTearDown]
        public void TeardownTests()
        {
        }
        
        [Test]
        public void CheckEncryption()
        {
            //OAuthID id = new OAuthID() { ID=
        }
    }
}
