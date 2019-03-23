using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace archean.mysql.Entities
{
    public class SimGroup
    {
        public int SimGroupId { get; set; }

        public string SimGroupType { get; set; }

        public string Description { get; set; }

        public string SimGroupParams { get; set; }

        public virtual List<Sim> Sims { get; set; }

    }

    public static class SimGroupExt
    {
        public static bool HasTheSameFields(this SimGroup simGroup, SimGroup other)
        {
            if (simGroup.SimGroupParams != other.SimGroupParams) return false;
            if (simGroup.SimGroupType != other.SimGroupType) return false;
            if (simGroup.Description != other.Description) return false;
            return true;
        }
    }
}
