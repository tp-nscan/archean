using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.Utils
{
    public static class Enumerables
    {
        public static IEnumerable<T> NullToEnumerable<T>(this IEnumerable<T> possiblyNull)
        {
            if (possiblyNull == null)
                return Enumerable.Empty<T>();

            return possiblyNull;
        }

        public static IEnumerable<Tuple<T,T>> SelfJoin<T>(this IEnumerable<T> items)
        {
            var itemList = items.ToList();

            foreach(var a in itemList)
            {
                foreach (var b in itemList)
                {
                    yield return new Tuple<T, T>(a, b);
                }
            }
        }
    }


    public interface IResidue
    {
        string Name { get; }
    }

    public interface ISequence<T> where T : IResidue
    {
        IResidue this[int dex] { get; }
        int Length { get; }
    }


    public class Sequence<T> : ISequence<T> where T : IResidue
    {
        public Sequence(IEnumerable<T> resudues)
        {
            _residues = resudues.ToList();
           
        }

        List<T> _residues;

        public int Length => _residues.Count();

        public IResidue this[int dex] => _residues[dex];

    }

    public class ResidueFunction<T> where T : IResidue
    {
        public ResidueFunction(IEnumerable<T> residueKeys, Func<T, T, double> tableFunc)
        {
            try
            {
                _residueKeys = residueKeys.ToList();
                var dict = _residueKeys.ToDictionary(k => k);
            }
            catch
            {
                throw new Exception("residueKeys are not unique");
            }
            _lookupTable = _residueKeys.SelfJoin().ToDictionary(k => k, k => tableFunc(k.Item1, k.Item2));
        }

        List<T> _residueKeys;

        public IEnumerable<T> ResidueKeys => _residueKeys;

        Dictionary<Tuple<T, T>, double> _lookupTable;
        public double GetValue(Tuple<T, T> key)
        {
            return _lookupTable[key];
        }

    }

    public interface IResidueFunc
    {
        double GetVal(IResidue a, IResidue b);
    }

    delegate double myMethodDelegate(IResidue a, IResidue b);

    public class Memoizer<Q,T> where Q: IResidueFunc where T : IResidue
    {
        public Memoizer(IEnumerable<T> residueKeys, IResidueFunc tableFunc)
        {
            try
            {
                _residueKeys = residueKeys.ToList();
                var dict = _residueKeys.ToDictionary(k => k);
            }
            catch
            {
                throw new Exception("residueKeys are not unique");
            }
            _lookupTable = _residueKeys.SelfJoin().ToDictionary(k => k, k => tableFunc.GetVal(k.Item1, k.Item2));
        }

        List<T> _residueKeys;

        Dictionary<Tuple<T, T>, double> _lookupTable;
        public double GetValue(Tuple<T, T> key)
        {
            return _lookupTable[key];
        }

    }
}
