namespace Engine
{
    public class HealingPotion : UsableItem
    {
        public int AmountToHeal { get; set; }

        public HealingPotion(int id, string name, string namePlural, int amountToHeal, int price, int minimumLevel = 1) : base(id, name, namePlural, price, minimumLevel)
        {
            AmountToHeal = amountToHeal;
        }
    }
}
