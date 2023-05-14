using Dane;

namespace Logika
{
    public abstract class AbstractLogicAPI : IObserver<IBall>, IObservable<int>
    {
        public abstract void CreateBoard();
        public abstract void CreateBalls(int num);
        public abstract void ClearBoard();
        public abstract void StartBalls();
        public abstract List<List<double>> GetBallsCoordsAndRadius();

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(IBall value);

        public abstract IDisposable Subscribe(IObserver<int> observerObj);

        public static AbstractLogicAPI CreateNewInstance(AbstractDataAPI? DataAPI = default)
        {
            return new LogicBoard(DataAPI == null ? AbstractDataAPI.CreateNewInstance() : DataAPI);
        }      
    }
}
