using System.Collections.Generic;

namespace archean.mysql.Entities
{
    public class Sim
    {
        public int SimId { get; set; }

        public int SimGroupId { get; set; }
        public virtual SimGroup SimGroup { get; set; }

        public string SimType { get; set; }

        public string Description { get; set; }

        public string SimParams { get; set; }

        public virtual List<Genome> Genomes { get; set; }

    }


}
