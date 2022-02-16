using AppSimple;
#if DEBUG
await AppSimple.WebHostBuilder.Run();
#else
BenchmarkTest.Run();
#endif