using Dane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Logika
{
    public abstract class LogicApi
    {
        private static LogicApi Instance = new Logic();

        public static LogicApi CreateNewInstance(DataApi dataApi)
        {
            return new Logic(dataApi);
        }

        public static LogicApi instance
        {
            get { return Instance; }
        }

        public abstract void GenerateBalls(int number);

        public abstract void CreateThreads();

        public abstract void StopThreads();

        internal sealed class Logic : LogicApi
        {
            internal Logic()
            {
                dataApi = DataApi.instance;
            }

            internal Logic(DataApi dataApi)
            {
                this.dataApi = dataApi;
            }

            DataApi dataApi;

            private bool stopThreads = false;

            public override void GenerateBalls(int num)
            {
                List<Ball> listaKulek = dataApi.GetBallList();
                listaKulek.Clear();
                Random rand = new Random();

                for (int i = 0; i < num; i++)
                {
                    int x = rand.Next(10, 590);
                    int y = rand.Next(10, 590);
                    listaKulek.Add(new Ball(x, y));
                }
            }

            public override void CreateThreads()
            {
                List<Ball> ballList = dataApi.GetBallList();
                stopThreads = false;

                foreach (Ball ball in ballList)
                {
                    Thread thread = new Thread(() =>
                    {
                        Random random = new Random();
                        int dx = random.Next(-10, 10);
                        int dy = random.Next(-10, 10);
                        while (!stopThreads)
                        {
                            ball.x += dx;
                            ball.y += dy;
                            while (ball.x < 10) ball.x += 580;
                            while (ball.x > 590) ball.x -= 580;
                            while (ball.y < 10) ball.y += 580;
                            while (ball.y > 590) ball.y -= 580;

                            Thread.Sleep(16);
                        }
                    });
                    thread.Start();
                }
            }

            public override void StopThreads()
            {
                stopThreads = true;
            }
        }
    }
}