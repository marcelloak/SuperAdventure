using Engine;

namespace SuperAdventure
{
    public partial class EquippableItems : Form
    {
        private Player _currentPlayer;
        private List<Equipment> _equipment = new List<Equipment>();
        private string _slot;

        public EquippableItems(Player player, string slot)
        {
            _currentPlayer = player;
            _slot = slot;
            _equipment = _currentPlayer.GetEquipmentForSlot(_slot);
            InitializeComponent();
            Text = "Equip to " + _slot;
            GenerateEquipmentList();
        }

        private void dgvEquipment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(dgvEquipment.Rows[e.RowIndex].Cells[0].Value.ToString());
            _currentPlayer.UnequipSlot(_slot);
            _currentPlayer.EquipItem(World.ItemByID(id) as Equipment);
            Close();
        }

        private void EquippableItems_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void GenerateEquipmentList()
        {
            dgvEquipment.ColumnCount = 7;
            dgvEquipment.Columns[0].Name = "ID";
            dgvEquipment.Columns[0].Visible = false;
            dgvEquipment.Columns[1].Name = "Name";
            dgvEquipment.Columns[1].Width = 197;
            dgvEquipment.Columns[2].Name = "Def";
            dgvEquipment.Columns[2].Width = 30;
            dgvEquipment.Columns[3].Name = "Str";
            dgvEquipment.Columns[3].Width = 30;
            dgvEquipment.Columns[4].Name = "Int";
            dgvEquipment.Columns[4].Width = 30;
            dgvEquipment.Columns[5].Name = "Dex";
            dgvEquipment.Columns[5].Width = 30;
            dgvEquipment.Columns[6].Name = "Vit";
            dgvEquipment.Columns[6].Width = 30;

            dgvEquipment.Rows.Clear();
            _equipment.ForEach(item => dgvEquipment.Rows.Add([item.ID, item.Name, item.Defence, item.AttributesIncreased.Strength, item.AttributesIncreased.Intelligence, item.AttributesIncreased.Dexterity, item.AttributesIncreased.Vitality]));
        }
    }
}
