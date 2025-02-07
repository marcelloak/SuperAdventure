using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class UsableItem : Item
    {
        public int MinimumLevel { get; set; }

        public UsableItem(int id, string name, string namePlural, int price, int minimumLevel = 1) : base(id, name, namePlural, price)
        {
            MinimumLevel = minimumLevel;
        }
    }
}
