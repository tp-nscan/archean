using System.Collections.Generic;

namespace archean.mysql.Entities
{
    public class Genome
    {
        public int GenomeId { get; set; }

        public int SimId { get; set; }
        public virtual Sim Sim { get; set; }

        public GenomeType GenomeType { get; set; }

        public virtual List<GenomeSequence> GenomeSequences { get; set; }

    }

    public enum GenomeType
    {
        Sorter,
        Sortable
    }


}
