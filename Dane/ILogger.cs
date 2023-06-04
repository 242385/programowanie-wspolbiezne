using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public abstract class ILogger
    {
        public abstract void AddBallToSerializationQueue(IBall ball);

        public static ILogger CreateLogger()
        {
            return new Logger();
        }
    }
}
