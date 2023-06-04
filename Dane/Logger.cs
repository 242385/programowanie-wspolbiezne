using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace Dane
{
    internal class Logger : ILogger
    {
        private Task? logging;
        private readonly ConcurrentQueue<JObject> queue;
        private readonly object bufLocker = new object();
        private readonly JArray logBalls;
        private readonly string path;
        private bool StopTask;

        public override void AddBallToQueue(IBall ball)
        {
            Monitor.Enter(bufLocker);
            try
            {
                JObject serializedObj = JObject.FromObject(ball);
                serializedObj["Czas"] = DateTime.Now.ToString("HH:mm:ss:fff");

                queue.Enqueue(serializedObj);
            }
            finally
            {
                Monitor.Exit(bufLocker);
            }
        }

        private async void WriteToFile()
        {
            string JSON;
            while (!this.StopTask)
            {
                if (!queue.IsEmpty)
                {
                    while (queue.TryDequeue(out JObject serializedObj))
                    {
                        logBalls.Add(serializedObj);
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
            Monitor.Enter(bufLocker);
            Monitor.Exit(bufLocker);
        }
    }
    

}
