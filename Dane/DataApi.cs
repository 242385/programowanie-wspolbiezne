namespace Dane
{
    public abstract class DataApi
    {
        private static Data Instance = new Data();

        public static DataApi CreateNewInstance() { return new Data(); }

        public abstract List<Ball> GetBallList();

        public static DataApi instance
        {
            get { return Instance; }
        }

        internal sealed class Data : DataApi
        {
            internal Data() { }

            private List<Ball> ballList = new List<Ball>();

            public override List<Ball> GetBallList()
            {
                return ballList;
            }
        }
    }
}