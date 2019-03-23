using archean.mysql;
using archean.mysql.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;


namespace archean.datasource.Test
{
    [TestClass]
    public class SimGroupFixture
    {
        protected ArcheanContext DbContext;
        protected TransactionScope TransactionScope;

        [TestInitialize]
        public void TestSetup()
        {
            DbContext = new ArcheanContext(ArcheanMySql.TestContextName);
            DbContext.Database.CreateIfNotExists();
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestMethod]
        public void TestAddAndRemoveSimGroup()
        {
            const string simGroupType = "simGroupType";
            const string description = "description";
            const string simgroupParams = "simgroupParams";

            var res = ArcheanMySql.SaveSimGroup(
                simGroupType: simGroupType,
                description: description,
                simgroupParams: simgroupParams,
                contextName: ArcheanMySql.TestContextName);

            var retread = ArcheanMySql.GetSimGroup(
                res.SimGroupId,
                contextName: ArcheanMySql.TestContextName);

            Assert.IsNotNull(retread);
            Assert.IsTrue(res.HasTheSameFields(retread));

            ArcheanMySql.RemoveSimGroup(
                res.SimGroupId,
                contextName: ArcheanMySql.TestContextName);
            retread = ArcheanMySql.GetSimGroup(
                res.SimGroupId,
                contextName: ArcheanMySql.TestContextName);

            Assert.IsNull(retread);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TransactionScope.Dispose();
            DbContext.Database.Delete();

        }

    }
}
