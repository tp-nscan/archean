namespace archean.mysql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genomes",
                c => new
                    {
                        GenomeId = c.Int(nullable: false, identity: true),
                        SimId = c.Int(nullable: false),
                        GenomeType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GenomeId)
                .ForeignKey("dbo.Sims", t => t.SimId, cascadeDelete: true)
                .Index(t => t.SimId);
            
            CreateTable(
                "dbo.GenomeSequences",
                c => new
                    {
                        GenomeSequenceId = c.Int(nullable: false, identity: true),
                        SimGroupId = c.Int(nullable: false),
                        SequenceId_Before = c.Int(nullable: false),
                        SequenceId = c.Int(nullable: false),
                        SequenceId_After = c.Int(nullable: false),
                        Genome_GenomeId = c.Int(),
                    })
                .PrimaryKey(t => t.GenomeSequenceId)
                .ForeignKey("dbo.SimGroups", t => t.SimGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Genomes", t => t.Genome_GenomeId)
                .Index(t => t.SimGroupId)
                .Index(t => t.Genome_GenomeId);
            
            CreateTable(
                "dbo.SimGroups",
                c => new
                    {
                        SimGroupId = c.Int(nullable: false, identity: true),
                        SimGroupType = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        SimGroupParams = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.SimGroupId);
            
            CreateTable(
                "dbo.Sims",
                c => new
                    {
                        SimId = c.Int(nullable: false, identity: true),
                        SimGroupId = c.Int(nullable: false),
                        SimType = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        SimParams = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.SimId)
                .ForeignKey("dbo.SimGroups", t => t.SimGroupId, cascadeDelete: true)
                .Index(t => t.SimGroupId);
            
            CreateTable(
                "dbo.Sequences",
                c => new
                    {
                        SequenceId = c.Int(nullable: false, identity: true),
                        Chars = c.String(unicode: false),
                        Length = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SequenceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GenomeSequences", "Genome_GenomeId", "dbo.Genomes");
            DropForeignKey("dbo.GenomeSequences", "SimGroupId", "dbo.SimGroups");
            DropForeignKey("dbo.Sims", "SimGroupId", "dbo.SimGroups");
            DropForeignKey("dbo.Genomes", "SimId", "dbo.Sims");
            DropIndex("dbo.Sims", new[] { "SimGroupId" });
            DropIndex("dbo.GenomeSequences", new[] { "Genome_GenomeId" });
            DropIndex("dbo.GenomeSequences", new[] { "SimGroupId" });
            DropIndex("dbo.Genomes", new[] { "SimId" });
            DropTable("dbo.Sequences");
            DropTable("dbo.Sims");
            DropTable("dbo.SimGroups");
            DropTable("dbo.GenomeSequences");
            DropTable("dbo.Genomes");
        }
    }
}
