namespace Engine
{
    public class Equipment : Item
    {
        public int Defence { get; set; }
        public string Slot {  get; set; }
        public Attributes AttributesIncreased { get; set; }

        public Equipment(int id, string name, string namePlural, int price, int defence, string slot, Attributes attributesIncreased = null) : base(id, name, namePlural, price)
        {
            Defence = defence;
            Slot = slot;
            if (attributesIncreased == null) AttributesIncreased = new Attributes(0, 0, 0, 0);
            else AttributesIncreased = attributesIncreased;
        }
    }
}
