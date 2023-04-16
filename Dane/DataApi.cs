namespace Dane
{
    public abstract class DataApi
    {
        private static Data Instance = new Data();

        public static DataApi CreateNewInstance() { return new Data(); }

        public static DataApi instance
        {
            get { return Instance; }
        }

        internal sealed class Data : DataApi
        {
            internal Data() { }
        }
    }
}