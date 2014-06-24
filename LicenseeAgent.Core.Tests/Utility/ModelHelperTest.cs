namespace LicenseeAgent.Core.Tests
{
    using LicenseeAgent.Core.Utility;
    using NUnit.Framework;

    /// <summary>
    /// Model Helper Test class.
    /// </summary>
    [TestFixture]
    public class ModelHelperTest
    {
        [Test]
        public void IsAgentLevel0()
        {
            Assert.IsTrue(ModelHelper.IsAgentLevel0(1), "Agent level 1 should be is agent level 0.");
            Assert.IsFalse(ModelHelper.IsAgentLevel0(2), "Agent level 2 is not agent level 0.");
            Assert.IsFalse(ModelHelper.IsAgentLevel0(-1), "Agent level -1 is incorrect.");
            Assert.IsFalse(ModelHelper.IsAgentLevel0(0), "Agent level 0 is incorrect.");
        }

        [Test]
        public void IsAgentLevel1()
        {
            Assert.IsTrue(ModelHelper.IsAgentLevel1(2), "Agent level = 2 should be is agent level 1.");
            Assert.IsFalse(ModelHelper.IsAgentLevel1(1), "Agent level 1 is not agent level 1.");
            Assert.IsFalse(ModelHelper.IsAgentLevel1(-1), "Agent level -1 is incorrect.");
            Assert.IsFalse(ModelHelper.IsAgentLevel1(0), "Agent level 0 is incorrect.");
        }

        [Test]
        public void IsLastAgent()
        {
            Assert.IsTrue(ModelHelper.IsLastAgent(4, 3), "Agent level = 4 is the last agent for number of level is 3.");
            Assert.IsTrue(ModelHelper.IsLastAgent(3, 2), "Agent level = 3 is the last agent for number of level is 2.");
            Assert.IsFalse(ModelHelper.IsLastAgent(0, 1), "Agent level is incorrect.");
            Assert.IsFalse(ModelHelper.IsLastAgent(3, 3), "Agent level should be greater 1 than no of level.");
            Assert.IsFalse(ModelHelper.IsLastAgent(5, 3), "Agent level should be greater 1 than no of level.");
        }
    }
}