using System.Diagnostics;

namespace Solve.Libraries.UnionFind
{
    [DebuggerDisplay("Count of Parents = {" + nameof(ParentsCount) + "}")]
    public class UnionFind
    {
        readonly int[] _parent;
        public int TreeCount { get; private set; }
        public UnionFind(int count)
        {
            _parent = Enumerable.Repeat(-1, count).ToArray();
            TreeCount = count;
        }

        public IEnumerable<int> AllParents =>
            _parent.Select((x, i) => (x, i))
                .Where(x => x.Item1 < 0)
                .Select(x => x.Item2);

        int ParentsCount => _parent.Count(x => x < 0);
        public int Size(int x) => -_parent[Find(x)];
        public int Find(int x) => _parent[x] < 0 ? x : _parent[x] = Find(_parent[x]);
        public bool Connect(int x, int y) {
            int rx = Find(x), ry = Find(y);
            if (rx == ry) return false;
            if (Size(rx) > Size(ry)) (rx, ry) = (ry, rx);
            _parent[rx] += _parent[ry];
            _parent[ry] = rx;

            --TreeCount;
            return true;
        }
        public bool Same(int x, int y) => Find(x) == Find(y);
        public ILookup<int, int> ToLookup() => EnumeratePairs().ToLookup(x => x.par, x => x.val);
        IEnumerable<(int par, int val)> EnumeratePairs() {
            for (int i = 0; i < _parent.Length; i++) yield return (Find(i), i);
        }
    }
}