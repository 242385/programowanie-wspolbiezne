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

        public abstract ObservableCollection<IModelBall> GetModelBallCollection();

        internal sealed class Model : ModelApi
        {
         
            LogicApi logicApi = LogicApi.instance;

            ObservableCollection<IModelBall> ModelBallCollection = new ObservableCollection<IModelBall>();

            public override void ConvertBallsToModelBalls()         
            {
                List<IBall> ballList = logicApi.GetBallList(); 
                ModelBallCollection.Clear();                 
                foreach (IBall ball in ballList)
                {
                    IModelBall newModelBall = IModelBall.CreateModelBall(ball);
                    ModelBallCollection.Add(newModelBall);
                }
            }

            public override ObservableCollection<IModelBall> GetModelBallCollection()
            {
                return ModelBallCollection;
            }
        }
    }
}
