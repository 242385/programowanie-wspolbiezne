using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace Dane
{
    internal class Logger : ILogger
    {
        private readonly ConcurrentQueue<JObject> queue;
        private readonly object Locker = new object();
        private readonly JArray logBalls;
        private readonly string path;
        private bool StopTask;

        public override void AddBallToQueue(IBall ball)
        {
            Monitor.Enter(Locker);
            try
            {
                JObject objToBeSerialized = JObject.FromObject(ball);
                objToBeSerialized["Czas"] = DateTime.Now.ToString("HH:mm:ss:fff");

                queue.Enqueue(objToBeSerialized);
            }
            finally
            {
                Monitor.Exit(Locker);
            }
        }

        private async void WriteToFile()
        {
            string JSON;
            while (!this.StopTask)
            {
                if (!queue.IsEmpty)
                {
                    while (queue.TryDequeue(out JObject objToBeSerialized))
                    {
                        logBalls.Add(objToBeSerialized);
                    }

                    JSON = JsonConvert.SerializeObject(logBalls, Formatting.Indented);
                    logBalls.Clear();

                    await File.AppendAllTextAsync(path, JSON);
                }
            }          
        }

        public override void Dispose()
        {
            this.StopTask = true;
        }

        public Logger()
        {
            string pathToDir = AppDomain.CurrentDomain.BaseDirectory;
            path = pathToDir + "logs.json";
            queue = new ConcurrentQueue<JObject>();
            FileStream logsFile = File.Create(path);
            logsFile.Close();

            logBalls = new JArray();
            this.StopTask = false;
            Task.Run(WriteToFile);                    
        }

        ~Logger()
        {
            Monitor.Enter(Locker);
            Monitor.Exit(Locker);
        }
    }
    

}
