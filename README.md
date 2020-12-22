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

|                  Method |         Mean |      Error |     StdDev |
|------------------------ |-------------:|-----------:|-----------:|
| EagerPropsUniqueAccount |   690.114 us | 12.1773 us | 11.3907 us |
|  LazyPropsUniqueAccount | 1,444.103 us | 12.7610 us | 11.9367 us |
| EagerPropsReusedAccount |     8.165 us |  0.1569 us |  0.1927 us |
|  LazyPropsReusedAccount |    20.963 us |  0.1091 us |  0.1020 us |

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

|                  Method |         Mean |        Error |       StdDev |
|------------------------ |-------------:|-------------:|-------------:|
| EagerPropsUniqueAccount |   9,350.2 ns |    185.77 ns |    482.85 ns |
|  LazyPropsUniqueAccount | 798,035.6 ns | 13,307.92 ns | 12,448.24 ns |
| EagerPropsReusedAccount |     119.8 ns |      2.42 ns |      2.14 ns |
|  LazyPropsReusedAccount |   9,384.3 ns |    150.63 ns |    140.90 ns |

# What's missing
The account 5 properties right now, but we read all of them every time. How does
performance change if we only access a couple of them at a time?