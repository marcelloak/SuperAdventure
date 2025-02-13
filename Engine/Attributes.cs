using System.ComponentModel;

namespace Engine
{
    public class Attributes : INotifyPropertyChanged
    {
        private int _strength { get; set; }
        public int Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                OnPropertyChanged("Strength");
            }
        }
        private int _intelligence { get; set; }
        public int Intelligence
        {
            get { return _intelligence; }
            set
            {
                _intelligence = value;
                OnPropertyChanged("Intelligence");
            }
        }
        private int _dexterity { get; set; }
        public int Dexterity
        {
            get { return _dexterity; }
            set
            {
                _dexterity = value;
                OnPropertyChanged("Dexterity");
            }
        }
        private int _vitality { get; set; }
        public int Vitality
        {
            get { return _vitality; }
            set
            {
                _vitality = value;
                OnPropertyChanged("Vitality");
            }
        }

        public Attributes(int strength = 5, int intelligence = 5, int dexterity = 5, int vitality = 5)
        {
            Strength = strength;
            Intelligence = intelligence;
            Dexterity = dexterity;
            Vitality = vitality;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
