# How to run

### From Visual Studio
Ensure you're in `Release` configuration, then run *without* debugging (Ctrl + F5).

### From the command line
```
dotnet run -c Release
```

## Filtering out benchmarks
By default, Benchmark.NET runs every benchmark it finds in the executable. To run
specific benchmarks, you can specify the `--filter` argument at the command line.

For example, to run only call benchmarks you can run

```
dotnet run -c Release --filter *CallBench*
```

# Call Benchmark
This benchmark compares synchronous, asynchronous, and mixed method calls.

`AsyncCall` is an async method that `await`s a call to another async method.
`MixedCall` is an async method that `await`s a call to a synchronous method.
`SyncCall` is a synchronous method that directly calls another synchronous method.

### Benchmark results

|       Method |      Mean |     Error |    StdDev |
|------------- |----------:|----------:|----------:|
|    AsyncCall | 34.497 ns | 0.6843 ns | 0.8654 ns |
|    MixedCall | 22.185 ns | 0.2746 ns | 0.2569 ns |
| FullSyncCall |  4.307 ns | 0.0893 ns | 0.0836 ns |

# Async Repo Benchmark
This benchmark simulates a call to repository backed by some form of external storage.

The `AsyncRepo` simulates an external call by `await`ing `Task.Yield()`.

`EagerPropsUniqueAccount` reads eagerly loaded properties from 100 unique accounts.
`LazyPropsUniqueAccount` reads lazily loaded properties from 100 unique accounts.

`EagerPropsReusedAccount` reads eagerly loaded properties 5 times from the same account.
`LazyPropsReusedAccount` reads lazily loaded properties 5 times from the same account.

### Benchmark results

|                  Method |       Mean |     Error |     StdDev |
|------------------------ |-----------:|----------:|-----------:|
| EagerPropsUniqueAccount | 248.269 us | 4.9207 us | 10.6973 us |
|  LazyPropsUniqueAccount | 489.445 us | 2.7930 us |  2.6126 us |
| EagerPropsReusedAccount |   2.544 us | 0.0148 us |  0.0139 us |
|  LazyPropsReusedAccount |   5.165 us | 0.0395 us |  0.0369 us |

# Sync Repo Benchmark
These benchmarks simulate a call to a repository that doesn't need to go out to external
storage, such as when retrieving something that's cached, or a hardcoded database table.

The benchmarks here should always be faster than the Async Repo benchmarks, as the
underlying repository does no `await`ing or `yield`ing, instead simply returning a
completed task.

`EagerPropsUniqueAccount` reads eagerly loaded properties from 100 unique accounts.
`LazyPropsUniqueAccount` reads lazily loaded properties from 100 unique accounts.

`EagerPropsReusedAccount` reads eagerly loaded properties 5 times from the same account.
`LazyPropsReusedAccount` reads lazily loaded properties 5 times from the same account.

### Benchmark results:

|                  Method |          Mean |        Error |       StdDev |
|------------------------ |--------------:|-------------:|-------------:|
| EagerPropsUniqueAccount |   4,249.71 ns |    70.246 ns |    65.708 ns |
|  LazyPropsUniqueAccount | 260,528.39 ns | 2,821.536 ns | 2,639.266 ns |
| EagerPropsReusedAccount |      56.98 ns |     1.096 ns |     0.915 ns |
|  LazyPropsReusedAccount |   3,023.80 ns |    15.869 ns |    14.844 ns |

# What's missing
The account only has 2 properties right now, and we read all of them every time. How does
performance change if we add a bunch more properties, but only access a couple of them?