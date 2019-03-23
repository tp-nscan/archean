namespace archean.mysql.Entities
{
    public class GenomeSequence
    {
        public int GenomeSequenceId { get; set; }

        public int SimGroupId { get; set; }
        public virtual SimGroup SimGroup { get; set; }

        public int SequenceId_Before { get; set; }
        public int SequenceId { get; set; }
        public int SequenceId_After { get; set; }
        
    }

}
