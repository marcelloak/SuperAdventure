using System.ComponentModel;

namespace Engine
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _details;
        public Item Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("Details");
            }
        }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Description");
            }
        }
        public string ItemType { get { return GetItemType(); } }
        public string Description { get { return Quantity > 1 ? Details.NamePlural : Details.Name; } }
        public int ItemID { get { return Details.ID; } }
        public int Price { get { return Details.Price; } }

        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }

        public string GetItemType()
        {
            if (this.Details is Weapon) return "Weapon";
            if (this.Details is Equipment) return "Equipment";
            if (this.Details is Scroll) return "Scroll";
            if (this.Details is HealingItem) return "Healing Item";
            if (this.Details is StatusItem) return "Status Item";
            if (this.Details is UsableItem) return "Usable Item";

            return "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
