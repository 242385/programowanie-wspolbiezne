using Logika;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelApi : INotifyPropertyChanged
    {
        ModelApi modelApi = ModelApi.instance;
        LogicApi logicApi = LogicApi.instance;

        public ViewModelApi()
        {
            this.SignalStart = new Signals(Start);
            this.SignalStop = new Signals(Stop);
            this.SignalResume = new Signals(Resume);
        }

        public string numberOfModelBalls
        {
            get;
            set;
        }

        private ObservableCollection<ModelBall> ModelBallCollection;

        public ObservableCollection<ModelBall> modelBallCollection
        {
            get { return ModelBallCollection; }
            set
            {
                ModelBallCollection = value;
                OnPropertyChanged("ModelBallCollection");
            }
        }

        public ICommand SignalStart
        {
            get;
            set;
        }

        public ICommand SignalStop
        {
            get;
            set;
        } 
        public ICommand SignalResume
        {
            get;
            set;
        }

        public void Start()
        {
            logicApi.GenerateBalls(Convert.ToInt16(numberOfModelBalls));
            modelApi.ConvertBallsToModelBalls();
            ModelBallCollection = modelApi.GetModelBallCollection();
            foreach (ModelBall modelBall in ModelBallCollection)
            {
                modelBall.PropertyChanged += propertyChanged;
            }
            logicApi.CreateThreads();
        }

        public void Stop()
        {
            logicApi.StopThreads();
        }

        public void Resume()
        {
            logicApi.ResumeThreads();
        }

        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ModelBallCollection");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

//SignalResume is experimental, not working atm