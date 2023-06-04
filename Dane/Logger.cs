using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace Dane
{
    internal class Logger : ILogger
    {
        private Task? logging;
        private readonly ConcurrentQueue<JObject> queue;
        private readonly object logLocker = new object();
        private readonly object bufLocker = new object();
        private readonly JArray logBalls;
        private readonly string path;

        public override void AddBallToQueue(IBall ball)
        {
            Monitor.Enter(bufLocker);
            try
            {
                JObject serializedObj = JObject.FromObject(ball);
                serializedObj["Czas"] = DateTime.Now.ToString("HH:mm:ss:fff");

                queue.Enqueue(serializedObj);

                if (logging == null || logging.IsCompleted)
                {
                    logging = Task.Factory.StartNew(WriteToFile);
                }
            }
            finally
            {
                Monitor.Exit(bufLocker);
            }
        }

        private void WriteToFile()
        {
            while (queue.TryDequeue(out JObject serializedObj))
            {
                logBalls.Add(serializedObj);
            }

            string JSON = JsonConvert.SerializeObject(logBalls, Formatting.Indented);

            Monitor.Enter(logLocker);
            try
            {
                File.WriteAllText(path, JSON);
            }
            finally
            {
                Monitor.Exit(logLocker);
            }
        }

        public Logger()
        {
            string pathToDir = AppDomain.CurrentDomain.BaseDirectory;
            path = pathToDir + "logs.json";
            queue = new ConcurrentQueue<JObject>();
            if (File.Exists(path))
            {
                try
                {
                    string inputFile = File.ReadAllText(path);
                    logBalls = JArray.Parse(inputFile);
                    return;
                }
                catch (JsonReaderException)
                {
                    logBalls = new JArray();
                }
            }
            logBalls = new JArray();
            FileStream logsFile = File.Create(path);
            logsFile.Close();
        }

        ~Logger()
        {
            Monitor.Enter(logLocker);
            Monitor.Exit(logLocker);
        }
    }
    

}
