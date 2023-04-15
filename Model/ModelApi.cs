using Dane;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelApi
    {
        private static ModelApi Instance = new Model();

        public static ModelApi CreateNewInstance(DataApi dataApi)
        {
            return new Model(dataApi);
        }

        public static ModelApi instance
        {
            get { return Instance; }
        }

        public abstract void ConvertBallsToModelBalls();

        public abstract ObservableCollection<ModelBall> GetModelBallCollection();

        internal sealed class Model : ModelApi
        {
            internal Model()
            {
                dataApi = DataApi.instance;
            }

            internal Model(DataApi dataApi)
            {
                this.dataApi = dataApi;
            }

            DataApi dataApi = DataApi.instance;

            ObservableCollection<ModelBall> ModelBallCollection = new ObservableCollection<ModelBall>();

            public override void ConvertBallsToModelBalls()
            {
                List<Ball> ballList = dataApi.GetBallList(); //to be changed, dataAPI should not be here
                ModelBallCollection.Clear();                 //idk how yet
                foreach (Ball ball in ballList)
                {
                    ModelBallCollection.Add(new ModelBall(ball));
                }
            }

            public override ObservableCollection<ModelBall> GetModelBallCollection()
            {
                return ModelBallCollection;
            }
        }
    }
}
