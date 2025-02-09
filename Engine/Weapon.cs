namespace Engine
{
    public class Weapon : UsableItem
    {
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int HitChance { get; set; }

        public Weapon(int id, string name, string namePlural, int minimumDamage, int maximumDamage, int price, int hitChance, int minimumLevel = 1) : base(id, name, namePlural, price, minimumLevel)
        {
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
            HitChance = hitChance;
        }
    }
}
