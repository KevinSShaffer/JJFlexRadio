using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JJTrace
{
    public static partial class Tracing
    {
        public delegate bool awaitExp();
        /// <summary>
        /// Await the specified condition.
        /// </summary>
        /// <param name="exp">function that returns the condition</param>
        /// <param name="ms">milliseconds to wait.</param>
        /// <param name="interval">optional interval to check</param>
        /// <returns>true if condition met.</returns>
        public static bool await(awaitExp exp, int ms, int interval)
        {
            int sanity = ms / interval;
            bool rv = false;
            while (sanity-- > 0)
            {
                rv = exp();
                if (rv) break;
                Thread.Sleep(interval);
            }
            return rv;
        }
        public static bool await(awaitExp exp, int ms)
        {
            return await(exp, ms, 25);
        }
    }
}
