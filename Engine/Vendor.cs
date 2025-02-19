﻿using System.ComponentModel;

namespace Engine
{
    public class Vendor : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public BindingList<InventoryItem> Inventory { get; set; }

        public Vendor(int id, string name)
        {
            ID = id;
            Name = name;
            Inventory = new BindingList<InventoryItem>();
        }

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);
            if (item == null) Inventory.Add(new InventoryItem(itemToAdd, quantity));
            else item.Quantity += quantity;
            OnPropertyChanged("Inventory");
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity < 0) item.Quantity = 0; // TODO: Might want to raise an error instead
                if (item.Quantity == 0) Inventory.Remove(item);
                OnPropertyChanged("Inventory");
            }
            // TODO: Might want to raise an error for if item is null
        }

        public void ClearInventory()
        {
            Inventory.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
