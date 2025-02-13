using System.ComponentModel;

namespace Engine
{
    public class LivingCreature : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("HitPoints");
            }
        }
        private int _maximumHitPoints;
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            set
            {
                _maximumHitPoints = value;
                OnPropertyChanged("HitPoints");
            }
        }

        public string HitPoints { get { return _currentHitPoints.ToString() + "/" + _maximumHitPoints.ToString(); } }
        public Attributes Attributes { get; set; }
        public bool IsDead {  get { return CurrentHitPoints <= 0; } }
        public Status CurrentStatus { get; set; }
        public bool HasAStatus { get { return CurrentStatus != null; } }

        public LivingCreature(int currentHitPoints, int maximumHitPoints, Status currentStatus = null)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            CurrentStatus = currentStatus;
            Attributes = new Attributes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
