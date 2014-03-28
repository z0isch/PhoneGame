using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PhoneGameService.Models;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
using PhoneGameService.Models.EdgeConditionals;
using PhoneGameService.Models.GameStates;

namespace PhoneGameService.Tests
{
    [TestFixture]
    public class RenderGraphTests
    {
        [TestFixtureSetUp]
        public void SetupTests()
        {
        }

        [TestFixtureTearDown]
        public void TeardownTests()
        {
        }

        [Test]
        public void RenderTwoPlayersOriginal()
        {
            using (var repository = new TelephoneGameRepository())
            {
                GameService.RenderGameTypeDotGraph<TwoPlayersOriginal>("TwoPlayersOriginal.dot");
            }
        }

        [Test]
        public void RenderTestingGameType()
        {
            using (var repository = new TelephoneGameRepository())
            {
                GameService.RenderGameTypeDotGraph<TestingGameType>("TestingGameType.dot");
            }
        }
 
    }
}
