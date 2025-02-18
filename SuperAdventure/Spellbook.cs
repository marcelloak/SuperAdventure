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

namespace SuperAdventure
{
    public partial class Spellbook : Form
    {
        private Player _currentPlayer;

        public Spellbook(Player player)
        {
            _currentPlayer = player;
            InitializeComponent();
            bindUI();
        }

        private void dgvSpells_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(dgvSpells.Rows[e.RowIndex].Cells[0].Value.ToString());
            ItemWindow itemWindow = new ItemWindow(World.SpellByID(id));
            itemWindow.StartPosition = FormStartPosition.CenterParent;
            itemWindow.ShowDialog(this);
        }

        private void Spellbook_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void bindUI()
        {
            dgvSpells.RowHeadersVisible = false;
            dgvSpells.AutoGenerateColumns = false;
            dgvSpells.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvSpells.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Name"
            });
            dgvSpells.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mana",
                Width = 50,
                DataPropertyName = "ManaCost"
            });
            dgvSpells.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Target",
                Width = 50,
                DataPropertyName = "Target"
            });
            dgvSpells.DataSource = _currentPlayer.Spellbook;
        }
    }
}
