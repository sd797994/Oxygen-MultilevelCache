using AppSimple;
#if DEBUG
await BenchmarkTest.Debug();
Console.WriteLine("excuting...");
Console.ReadKey();
#else
BenchmarkTest.Run();
#endif