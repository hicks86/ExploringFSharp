using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking
{
    static class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            //For debugging
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, new DebugInProcessConfig());
#else
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args);
#endif
        }
    }
}
