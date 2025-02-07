using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
