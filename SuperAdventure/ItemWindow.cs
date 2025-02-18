using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using static System.Net.Mime.MediaTypeNames;

namespace SuperAdventure
{
    public partial class ItemWindow : Form
    {
        public ItemWindow(Object obj)
        {
            InitializeComponent();
            if (obj is Item) rtbInfo.AppendText(ItemInformation(obj as Item));
            else if (obj is Quest) rtbInfo.AppendText(QuestInformation(obj as Quest));
            else if (obj is Spell) rtbInfo.AppendText(SpellInformation(obj as Spell));
        }

        private string ItemInformation(Item item)
        {
            string text = "";
            lblName.Text = item.Name;
            if (item.Price == World.UNSELLABLE_ITEM_PRICE) text += "Unsellable" + Environment.NewLine;
            else if (item.Price == World.WORTHLESS_ITEM_PRICE) text += "Worthless" + Environment.NewLine;
            else text += item.Price + " gold" + Environment.NewLine;
            if (item is UsableItem)
            {
                text += "Usable" + Environment.NewLine;
                if ((item as UsableItem).MinimumLevel > 1) text += "Minimum level: " + (item as UsableItem).MinimumLevel + Environment.NewLine;
            }
            if (item is Weapon)
            {
                text += "Weapon" + Environment.NewLine;
                text += Environment.NewLine;
                if ((item as Weapon).StrengthRequired > 1) text += "Minimum strength: " + (item as Weapon).StrengthRequired + Environment.NewLine;
                if ((item as Weapon).DexterityRequired > 1) text += "Minimum dexterity: " + (item as Weapon).DexterityRequired + Environment.NewLine;
                text += "Damage: " + (item as Weapon).MinimumDamage + "-" + (item as Weapon).MaximumDamage + Environment.NewLine;
                text += "Hit chance: " + (item as Weapon).HitChance + Environment.NewLine;
            }
            else if (item is Scroll)
            {
                text += "Scroll" + Environment.NewLine;
                text += Environment.NewLine;
                text += SpellInformation((item as Scroll).SpellContained, true);
            }
            else if (item is HealingItem) text += "Heals " + (item as HealingItem).AmountToHeal + " point" + ((item as HealingItem).AmountToHeal == 1 ? "" : "s") + Environment.NewLine;
            else if (item is StatusItem) text += StatusInformation((item as StatusItem).StatusApplied);
            return text;
        }

        private string QuestInformation(Quest quest)
        {
            string text = "";
            lblName.Text = quest.Name;
            text += quest.Description + Environment.NewLine;
            text += "Required to complete:" + Environment.NewLine;
            quest.QuestCompletionItems.ForEach(questItem => text += questItem.Quantity.ToString() + " " + questItem.Description);
            text += Environment.NewLine;
            return text;
        }

        private string SpellInformation(Spell spell, bool fromScroll = false)
        {
            string text = "";
            if (fromScroll) text += spell.Name + Environment.NewLine;
            else
            {
                lblName.Text = spell.Name;
                if (spell.MinimumLevel > 1) text += "Minimum level: " + spell.MinimumLevel + Environment.NewLine;
                if (spell.MinimumIntelligence > 1) text += "Minimum intelligence: " + spell.MinimumIntelligence + Environment.NewLine;
                text += "Mana cost: " + spell.ManaCost + Environment.NewLine;
            }
            text += "Target: " + spell.Target + Environment.NewLine;
            if (spell is HealingSpell) text += "Heals " + (spell as HealingSpell).AmountToHeal + " point" + ((spell as HealingSpell).AmountToHeal == 1 ? "" : "s") + Environment.NewLine;
            else if (spell is StatusSpell) text += StatusInformation((spell as StatusSpell).StatusApplied);
            return text;
        }

        private string StatusInformation(Status status)
        {
            string text = "";
            if (status.Turns == 1) text += "Deals " + status.Value + " damage" + Environment.NewLine;
            else
            {
                text += "Applies " + status.Name + (status.Turns == Int32.MaxValue ? "" : " for " + status.Turns + " turn" + (status.Turns == 1 ? "" : "s")) + Environment.NewLine;
                text += StatusDescription(status.ID);
            }
            return text;
        }

        private string StatusDescription(int statusID)
        {
            if (statusID == World.STATUS_ID_POISON) return "Deals damage each turn for a set amount of turns." + Environment.NewLine;
            if (statusID == World.STATUS_ID_SLEEP) return "Skips turns and has a chance to wear off each turn." + Environment.NewLine;
            if (statusID == World.STATUS_ID_HASTE) return "Increases speed for a set amount of turns." + Environment.NewLine;
            if (statusID == World.STATUS_ID_PARALYZE) return "Chance to skip turn each turn for a set amount of turns." + Environment.NewLine;
            if (statusID == World.STATUS_ID_FROZEN) return "Skips turns and has a chance to wear off each turn." + Environment.NewLine;
            if (statusID == World.STATUS_ID_BURN) return "Deals damage and has a chance to wear off each turn." + Environment.NewLine;
            return "";
        }

        private void ItemWindow_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
