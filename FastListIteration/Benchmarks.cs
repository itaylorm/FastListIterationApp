using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace FastListIteration;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    // 80085 is a seed that makes the number the same per iteration
    private static readonly Random Rng = new Random(80085);

    [Params(100, 100_000, 1_000_000)]
    public int Size { get; set; }

    private List<int> _items;

    [GlobalSetup]
    public void Setup()
    {
        _items = Enumerable.Range(0, Size).Select(x => Rng.Next()).ToList();
    }

    [Benchmark]
    public void ForLoop()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
        }
    }

    [Benchmark]
    public void ForEachLoop()
    {
        foreach (var item in _items)
        {
        }
    }

    [Benchmark]
    public void Foreach_Linq()
    {
        _items.ForEach(x => { });
    }

    [Benchmark]
    public void Parallel_ForEach()
    {
        Parallel.ForEach(_items, x => { });
    }

    [Benchmark]
    public void Parallel_Linq()
    {
        _items.AsParallel().ForAll(x => { });
    }

    [Benchmark]
    public void ForeEach_Span()
    {
        foreach(int item in CollectionsMarshal.AsSpan(_items))
        {
        }
    }

    [Benchmark]
    public void For_Span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items);
        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = asSpan[i];
        }
    }
}
