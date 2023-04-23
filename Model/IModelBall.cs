using System.ComponentModel;
using Logika;

namespace Model
{
     public abstract class IModelBall
    {
        public static IModelBall CreateModelBall(IBall ball)
        {
            return new ModelBall(ball);
        }

        public abstract int x { get; set; }
        public abstract int y { get; set; }

        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}
