namespace Engine
{
    public class EquipmentSet
    {
        public Equipment Head { get; set; }
        public Equipment Arms { get; set; }
        public Equipment Hands { get; set; }
        public Equipment Legs { get; set; }
        public Equipment Feet { get; set; }

        public EquipmentSet(Equipment head = null, Equipment arms = null, Equipment hands = null, Equipment legs = null, Equipment feet = null)
        {
            Head = head;
            Arms = arms;
            Hands = hands;
            Legs = legs;
            Feet = feet;
        }

        public int GetDefence()
        {
            int defence = 0;
            if (Head != null) defence += Head.Defence;
            if (Arms != null) defence += Arms.Defence;
            if (Hands != null) defence += Hands.Defence;
            if (Legs != null) defence += Legs.Defence;
            if (Feet != null) defence += Feet.Defence;
            return defence;
        }

        public int GetStrengthIncreased()
        {
            int strength = 0;
            if (Head != null) strength += Head.AttributesIncreased.Strength;
            if (Arms != null) strength += Arms.AttributesIncreased.Strength;
            if (Hands != null) strength += Hands.AttributesIncreased.Strength;
            if (Legs != null) strength += Legs.AttributesIncreased.Strength;
            if (Feet != null) strength += Feet.AttributesIncreased.Strength;
            return strength;
        }

        public int GetDexterityIncreased()
        {
            int dexterity = 0;
            if (Head != null) dexterity += Head.AttributesIncreased.Dexterity;
            if (Arms != null) dexterity += Arms.AttributesIncreased.Dexterity;
            if (Hands != null) dexterity += Hands.AttributesIncreased.Dexterity;
            if (Legs != null) dexterity += Legs.AttributesIncreased.Dexterity;
            if (Feet != null) dexterity += Feet.AttributesIncreased.Dexterity;
            return dexterity;
        }

        public int GetIntelligenceIncreased()
        {
            int intelligence = 0;
            if (Head != null) intelligence += Head.AttributesIncreased.Intelligence;
            if (Arms != null) intelligence += Arms.AttributesIncreased.Intelligence;
            if (Hands != null) intelligence += Hands.AttributesIncreased.Intelligence;
            if (Legs != null) intelligence += Legs.AttributesIncreased.Intelligence;
            if (Feet != null) intelligence += Feet.AttributesIncreased.Intelligence;
            return intelligence;
        }

        public int GetVitalityIncreased()
        {
            int vitality = 0;
            if (Head != null) vitality += Head.AttributesIncreased.Vitality;
            if (Arms != null) vitality += Arms.AttributesIncreased.Vitality;
            if (Hands != null) vitality += Hands.AttributesIncreased.Vitality;
            if (Legs != null) vitality += Legs.AttributesIncreased.Vitality;
            if (Feet != null) vitality += Feet.AttributesIncreased.Vitality;
            return vitality;
        }
    }
}
