﻿using Logika;
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
            this.SygnalStart = new Signals(Start);
            this.SygnalStop = new Signals(Stop);
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

        public ICommand SygnalStart
        {
            get;
            set;
        }

        public ICommand SygnalStop
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
