using Logika;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class ModelBall : INotifyPropertyChanged
    {
        public ModelBall(Ball ball)
        {
            ball.PropertyChanged += propertyChanged;
            this.x = ball.x;
            this.y = ball.y;
        }

        private int X;
        private int Y;

        public int x
        {
            get { return X; }
            set
            {
                X = value;
                OnPropertyChanged("x");
            }
        }
        public int y
        {
            get { return Y; }
            set
            {
                Y = value;
                OnPropertyChanged("y");
            }
        }

        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Ball ball = (Ball)sender;
            if (e.PropertyName == "x")
            {
                this.x = ball.x;
            }
            if (e.PropertyName == "y")
            {
                this.y = ball.y;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
