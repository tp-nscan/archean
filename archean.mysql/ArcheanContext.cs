using System;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Linq;
using archean.mysql.Entities;

namespace archean.mysql
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ArcheanContext : DbContext
    {
        public ArcheanContext(string contextName = ArcheanMySql.TestContextName) : base(contextName)
        {
        }

        public DbSet<SimGroup> SimGroups { get; set; }

        public DbSet<Sim> Sims { get; set; }

        public DbSet<Genome> Genomes { get; set; }

        public DbSet<GenomeSequence> GenomeSequences { get; set; }

        public DbSet<Sequence> Sequences { get; set; }


    }

    public static class ArcheanMySql
    {
        public const string DefaultContextName = "Context1";
        public const string TestContextName = "ContextTest";

        public static SimGroup SaveSimGroup(
            string simGroupType, 
            string description,
            string simgroupParams,
            string contextName = DefaultContextName
            )
        {
            SimGroup simGroup = null;
            using (var db = new ArcheanContext(contextName))
            {

                simGroup = new SimGroup
                {
                    SimGroupType = simGroupType,
                    Description = description,
                    SimGroupParams = simgroupParams
                };
                db.SimGroups.Add(simGroup);
                db.SaveChanges();
            }
            return simGroup;
        }

        public static SimGroup GetSimGroup(
            int simGroupId,
            string contextName = DefaultContextName
        )
        {
            SimGroup simGroup = null;
            using (var db = new ArcheanContext(contextName))
            {
                simGroup = db.SimGroups.Where(sg => sg.SimGroupId == simGroupId)
                                       .FirstOrDefault();
            }
            return simGroup;
        }


        public static void RemoveSimGroup(
            int simGroupId,
            string contextName = DefaultContextName
        )
        {
            SimGroup simGroup = new SimGroup(){SimGroupId = simGroupId};
            using (var db = new ArcheanContext(contextName))
            {
                db.SimGroups.Attach(simGroup);
                db.SimGroups.Remove(simGroup);
                db.SaveChanges();
            }
        }


    }
}
