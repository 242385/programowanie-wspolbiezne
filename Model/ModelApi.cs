using Logika;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelApi
    {
        private static ModelApi Instance = new Model();

        public static ModelApi instance
        {
            get { return Instance; }
        }

        public abstract void ConvertBallsToModelBalls();

        public abstract ObservableCollection<ModelBall> GetModelBallCollection();

        internal sealed class Model : ModelApi
        {
         
            LogicApi logicApi = LogicApi.instance;

            ObservableCollection<ModelBall> ModelBallCollection = new ObservableCollection<ModelBall>();

            public override void ConvertBallsToModelBalls()         
            {
                List<Ball> ballList = logicApi.GetBallList(); 
                ModelBallCollection.Clear();                 
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
