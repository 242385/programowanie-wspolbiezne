using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public abstract class ILogger
    {
        public abstract void AddBallToQueue(IBall ball);

        public static ILogger CreateLogger()
        {
            return new Logger();
        }
    }
}
