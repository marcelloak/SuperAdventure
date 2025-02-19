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
        private int _currentMana;
        public int CurrentMana
        {
            get { return _currentMana; }
            set
            {
                _currentMana = value;
                OnPropertyChanged("Mana");
            }
        }
        private int _maximumMana;
        public int MaximumMana
        {
            get { return _maximumMana; }
            set
            {
                _maximumMana = value;
                OnPropertyChanged("Mana");
            }
        }
        public string Mana { get { return _currentMana.ToString() + "/" + _maximumMana.ToString(); } }
        public Attributes Attributes { get; set; }
        public bool IsDead {  get { return CurrentHitPoints <= 0; } }
        public Status CurrentStatus { get; set; }
        public string Status { get { return (CurrentStatus == null) ? "" : CurrentStatus.Name; } }
        public bool HasAStatus { get { return CurrentStatus != null; } }
        public bool HasANegativeStatus { get { return CurrentStatus.ID != World.STATUS_ID_HASTE; } }
        public BindingList<InventoryItem> Inventory { get; set; }
        public BindingList<Spell> Spellbook { get; set; }

        public LivingCreature(int currentHitPoints, int maximumHitPoints, int currentMana = 0, int maximumMana = 0, Status currentStatus = null)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            CurrentMana = currentMana;
            MaximumMana = maximumMana;
            CurrentStatus = currentStatus;
            Attributes = new Attributes();
            Inventory = new BindingList<InventoryItem>();
            Spellbook = new BindingList<Spell>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
