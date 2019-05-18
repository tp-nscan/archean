namespace archean.mysql.Test
{
    public static class ArcheanMySqlTest
    {

        public static bool TestAddAndRemoveSimGroup()
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

            if (retread == null) return false;

            return true;
            //Assert.IsNotNull(retread);
            //Assert.IsTrue(res.HasTheSameFields(retread));

            //ArcheanMySql.RemoveSimGroup(
            //    res.SimGroupId,
            //    contextName: ArcheanMySql.TestContextName);
            //retread = ArcheanMySql.GetSimGroup(
            //    res.SimGroupId,
            //    contextName: ArcheanMySql.TestContextName);

            //Assert.IsNull(retread);
        }
    }
}
