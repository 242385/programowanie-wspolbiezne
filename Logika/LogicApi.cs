using Dane;

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
        public abstract void ResumeThreads();

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

            public override void GenerateBalls(int number)
            {
                List<Ball> ballList = dataApi.GetBallList();
                ballList.Clear();
                Random random = new Random();

                for (int i = 0; i < number; i++)
                {
                    int x = random.Next(10, 590);
                    int y = random.Next(10, 590);
                    ballList.Add(new Ball(x, y));
                }
            }

            public override void CreateThreads()
            {
                List<Ball> ballList= dataApi.GetBallList();
                stopThreads = false;

                foreach (Ball ball in ballList)
                {
                    Thread thread = new Thread(() =>
                    {
                        Random random = new Random();
                        int counter = 0;
                        int dx = random.Next(-5, 5);
                        int dy = random.Next(-5, 5);
                        while (!stopThreads)
                        {
                            counter+= random.Next(1, 5);
                            if(counter >= 50)
                            {
                                dx = random.Next(-5, 6);
                                dy = random.Next(-5, 6);
                                counter = 0;
                            }
                            if (ball.x < 40)
                            {
                                dx = random.Next(5, 10);
                                dy = random.Next(-5, 5);
                            }
                            if (ball.x > 560)
                            {
                                dx = random.Next(-10, -5);
                                dy = random.Next(-5, 5);
                            }
                            if (ball.y < 40)
                            {
                                dx = random.Next(-5, 5);
                                dy = random.Next(5, 10);
                            }
                            if (ball.y > 560)
                            {
                                dx = random.Next(-5, 5);
                                dy = random.Next(-10, -5);
                            }

                            ball.x += dx;
                            ball.y += dy;

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

            public override void ResumeThreads()
            {
                stopThreads = false; //PLACEHOLDER
            }
        }
    }
}
