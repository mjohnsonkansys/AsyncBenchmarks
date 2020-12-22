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
|    AsyncCall | 35.992 ns | 0.5083 ns | 0.4506 ns |
|    MixedCall | 21.674 ns | 0.1605 ns | 0.1423 ns |
| FullSyncCall |  4.416 ns | 0.1147 ns | 0.1073 ns |

# Async Lazy Benchmark

Compares `await`ing a task directly and `await`ing a task wrapped by `AsyncLazy<T>`.

### Benchmark results

|                  Method |        Mean |     Error |    StdDev |
|------------------------ |------------:|----------:|----------:|
|         AsyncLazySingle | 1,537.24 ns | 30.529 ns | 63.726 ns |
|              TaskSingle |    34.76 ns |  0.449 ns |  0.420 ns |
|          AsyncLazyMulti | 1,895.75 ns | 14.187 ns | 13.270 ns |
| YieldingAsyncLazySingle | 2,354.20 ns | 10.358 ns |  9.689 ns |
|      YieldingTaskSingle | 1,341.59 ns |  4.837 ns |  4.525 ns |
|  YieldingAsyncLazyMulti | 2,844.05 ns | 23.930 ns | 22.385 ns |

# Repository Benchmarks

`AllEagerProps` benchmarks eagerly load Account properties and read all of them.
`AllLazyProps` benchmarks lazily load Account properties and read all of them.

`TwoEagerProps` benchmarks eagerly load Account properties and read 2 of them.
`TwoLazyProps` benchmarks lazily load Account properties and read 2 of them.

`UniqueAccount` benchmarks read from 100 unique accounts.
`ReusedAccount` benchmarks read from 1 account, but 5 separate times.

## Async Repo Benchmark
This benchmark simulates a call to repository backed by some form of external storage.
The `AsyncRepo` simulates an external call by `await`ing `Task.Yield()` - this doesn't
simulate the delay of a database call, but it should be fairly accurate on the overhead
of async/await itself.

### Benchmark results

|                     Method |         Mean |     Error |    StdDev |
|--------------------------- |-------------:|----------:|----------:|
| AllEagerPropsUniqueAccount |   601.037 us | 1.8149 us | 1.5155 us |
|  AllLazyPropsUniqueAccount | 1,250.706 us | 7.2732 us | 6.4475 us |
| TwoEagerPropsUniqueAccount |   600.131 us | 3.3568 us | 3.1399 us |
|  TwoLazyPropsUniqueAccount |   517.754 us | 2.5689 us | 2.4030 us |

|                     Method |         Mean |     Error |    StdDev |
|--------------------------- |-------------:|----------:|----------:|
| AllEagerPropsReusedAccount |     6.233 us | 0.0329 us | 0.0291 us |
|  AllLazyPropsReusedAccount |    17.243 us | 0.1381 us | 0.1292 us |
| TwoEagerPropsReusedAccount |     6.224 us | 0.0396 us | 0.0331 us |
|  TwoLazyPropsReusedAccount |     5.477 us | 0.0381 us | 0.0338 us |

# Sync Repo Benchmark
These benchmarks simulate a call to a repository that doesn't need to go out to external
storage, such as when retrieving something that's cached, or a hardcoded database table.

The benchmarks here should always be faster than the Async Repo benchmarks, as the
underlying repository does no `await`ing or `yield`ing, instead simply returning a
completed task.

### Benchmark results:

|                     Method |          Mean |        Error |       StdDev |
|--------------------------- |--------------:|-------------:|-------------:|
| AllEagerPropsUniqueAccount |   7,549.12 ns |    92.948 ns |    86.944 ns |
|  AllLazyPropsUniqueAccount | 683,726.47 ns | 4,633.499 ns | 4,334.178 ns |
| TwoEagerPropsUniqueAccount |   7,238.91 ns |    62.664 ns |    58.616 ns |
|  TwoLazyPropsUniqueAccount | 276,023.49 ns | 3,258.254 ns | 2,888.357 ns |

|                     Method |          Mean |        Error |       StdDev |
|--------------------------- |--------------:|-------------:|-------------:|
| AllEagerPropsReusedAccount |      87.15 ns |     0.644 ns |     0.602 ns |
|  AllLazyPropsReusedAccount |   7,411.52 ns |    55.369 ns |    51.792 ns |
| TwoEagerPropsReusedAccount |      86.33 ns |     1.657 ns |     1.384 ns |
|  TwoLazyPropsReusedAccount |   3,229.06 ns |    18.189 ns |    17.014 ns |
