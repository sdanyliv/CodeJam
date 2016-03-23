using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDotNet.NUnit
{
    /// <summary>
    /// Use this to run fast but inaccurate measures
    /// </summary>
    public class FastRunConfig : ManualConfig
    {        
        /// <summary>
        /// Instance of the config
        /// </summary>
        public static readonly IConfig Instance = new FastRunConfig();

        /// <summary> 
        /// Constructor
        /// </summary>
        public FastRunConfig()
        {
            Add(new Job
            {
                IterationTime = 50,
                LaunchCount = 1,
                WarmupCount = 1,
                TargetCount = 10,
                Jit = Jit.RyuJit
            });
        }
    }

    /// <summary>
    /// Use this to run slower but proven-to-be-accurate perf tests
    /// </summary>
    public class TestProofConfig : ManualConfig
    {
        /// <summary>
        /// Instance of the config
        /// </summary>
        public static readonly IConfig Instance = new TestProofConfig();

        /// <summary> 
        /// Constructor
        /// </summary>
        public TestProofConfig()
        {
            Add(new Job
            {
                TargetCount = 10
            });
        }
    }
}
