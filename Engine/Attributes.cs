namespace Engine
{
    public class Attributes
    {
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }

        public Attributes(int strength = 5, int intelligence = 5, int dexterity = 5, int vitality = 5)
        {
            Strength = strength;
            Intelligence = intelligence;
            Dexterity = dexterity;
            Vitality = vitality;
        }
    }
}
