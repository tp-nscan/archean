using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;
using Sorting.Stages;

namespace SorterControls.View.Common
{
    public static class StageLayout
    {
        public static IEnumerable<Tuple<int, T>> ToStageLayouts<T>(this ISorterStage<T> sorterStage)
            where T : IKeyPair
        {
            var stageLayoutImpl = new StageLayoutImpl<T>();

            foreach (var keyPair in sorterStage.KeyPairs.OrderBy(kp=>kp.Index))
            {
                stageLayoutImpl.AddKeyPair(keyPair);
            }

            return stageLayoutImpl.StageLayouts;
        }
    }

    public class StageLayoutImpl<T> where T : IKeyPair
    {
        readonly List<LayoutGroup<T>> _layoutGroups = new List<LayoutGroup<T>>();
        public IEnumerable<LayoutGroup<T>> LayoutGroups { get { return _layoutGroups; } }

        public bool AddKeyPair(T keyPair)
        {
            if (LayoutGroups.Any(layoutGroup => layoutGroup.TryAdd(keyPair)))
            {
                return true;
            }
            var newGroup = new LayoutGroup<T>(_layoutGroups.Count);
            _layoutGroups.Add(newGroup);
            return newGroup.TryAdd(keyPair);
        }

        public IEnumerable<Tuple<int, T>> StageLayouts
        {
            get {
                    return from layoutGroup in _layoutGroups.OrderBy(g=>g.Position) 
                           from keyPair in layoutGroup.KeyPairs 
                           select new Tuple<int, T>(layoutGroup.Position, keyPair);
            }
        }
    }

    public class LayoutGroup<T> where T : IKeyPair
    {
        public LayoutGroup(int position)
        {
            _position = position;
        }

        private readonly int _position;
        public int Position
        {
            get { return _position; }
        }

        readonly List<T> _keyPairs = new List<T>();
        public IEnumerable<T> KeyPairs { get { return _keyPairs; } }

        public bool TryAdd(T keyPair)
        {
            if (KeyPairs.Any(member => member.SpanOverlaps(keyPair)))
            {
                return false;
            }
            _keyPairs.Add(keyPair);
            return true;
        }
    }

}
