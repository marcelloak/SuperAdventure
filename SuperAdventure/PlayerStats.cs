using System.Xml.Linq;
using Engine;

namespace SuperAdventure
{
    public partial class PlayerStats : Form
    {
        private Player _player;
        private Attributes _attributes = new Attributes();
        private int _attributePointsToSpend;
        private List<Button> levelingButtons = new List<Button>();
        Dictionary<Keys, Button> _keyBindings;

        public PlayerStats(Player player)
        {
            _player = player;
            _keyBindings = new Dictionary<Keys, Button>();
            _attributePointsToSpend = _player.AttributePointsToSpend;
            _attributes.Strength = _player.BaseAttributes.Strength;
            _attributes.Intelligence = _player.BaseAttributes.Intelligence;
            _attributes.Dexterity = _player.BaseAttributes.Dexterity;
            _attributes.Vitality = _player.BaseAttributes.Vitality;
            InitializeComponent();
            AddLevelingButtons();
            BindKeys();
            BindUI();
            RefreshLevelingButtons();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _player.AttributePointsToSpend = _attributePointsToSpend;
            _player.BaseAttributes.Strength = _attributes.Strength;
            _player.BaseAttributes.Intelligence = _attributes.Intelligence;
            _player.BaseAttributes.Dexterity = _attributes.Dexterity;
            _player.BaseAttributes.Vitality = _attributes.Vitality;
            RefreshLevelingButtons();
        }

        private void btnStrengthMinus_Click(object sender, EventArgs e)
        {
            _attributes.Strength--;
            _attributePointsToSpend++;
            RefreshLevelingButtons();
        }

        private void btnStrengthPlus_Click(object sender, EventArgs e)
        {
            _attributes.Strength++;
            _attributePointsToSpend--;
            RefreshLevelingButtons();
        }

        private void btnIntelligenceMinus_Click(object sender, EventArgs e)
        {
            _attributes.Intelligence--;
            _attributePointsToSpend++;
            RefreshLevelingButtons();
        }

        private void btnIntelligencePlus_Click(object sender, EventArgs e)
        {
            _attributes.Intelligence++;
            _attributePointsToSpend--;
            RefreshLevelingButtons();
        }

        private void btnDexterityMinus_Click(object sender, EventArgs e)
        {
            _attributes.Dexterity--;
            _attributePointsToSpend++;
            RefreshLevelingButtons();
        }

        private void btnDexterityPlus_Click(object sender, EventArgs e)
        {
            _attributes.Dexterity++;
            _attributePointsToSpend--;
            RefreshLevelingButtons();
        }

        private void btnVitalityMinus_Click(object sender, EventArgs e)
        {
            _attributes.Vitality--;
            _attributePointsToSpend++;
            RefreshLevelingButtons();
        }

        private void btnVitalityPlus_Click(object sender, EventArgs e)
        {
            _attributes.Vitality++;
            _attributePointsToSpend--;
            RefreshLevelingButtons();
        }
        private void AddLevelingButtons()
        {
            levelingButtons.Add(btnApply);
            levelingButtons.Add(btnStrengthPlus);
            levelingButtons.Add(btnStrengthMinus);
            levelingButtons.Add(btnIntelligencePlus);
            levelingButtons.Add(btnIntelligenceMinus);
            levelingButtons.Add(btnDexterityPlus);
            levelingButtons.Add(btnDexterityMinus);
            levelingButtons.Add(btnVitalityPlus);
            levelingButtons.Add(btnVitalityMinus);
        }

        private void RefreshLevelingButtons()
        {
            if (_attributePointsToSpend > 0 || StillLeveling())
            {
                ToggleLevelingButtons(true);
                DeactivateMinusButtons();
                if (_attributePointsToSpend == 0) DeactivatePlusButtons();
            }
            else ToggleLevelingButtons(false);
        }

        private void DeactivateMinusButtons()
        {
            if (_player.BaseAttributes.Strength == _attributes.Strength) btnStrengthMinus.Enabled = false;
            if (_player.BaseAttributes.Intelligence == _attributes.Intelligence) btnIntelligenceMinus.Enabled = false;
            if (_player.BaseAttributes.Dexterity == _attributes.Dexterity) btnDexterityMinus.Enabled = false;
            if (_player.BaseAttributes.Vitality == _attributes.Vitality) btnVitalityMinus.Enabled = false;
        }

        private void DeactivatePlusButtons()
        {
            btnStrengthPlus.Enabled = false;
            btnIntelligencePlus.Enabled = false;
            btnDexterityPlus.Enabled = false;
            btnVitalityPlus.Enabled = false;
        }

        private void ToggleLevelingButtons(bool enabled)
        {
            foreach (Button button in levelingButtons)
            {
                button.Enabled = enabled;
                button.Visible = enabled;
            }
        }

        private bool StillLeveling()
        {
            if (_player.BaseAttributes.Strength != _attributes.Strength) return true;
            if (_player.BaseAttributes.Intelligence != _attributes.Intelligence) return true;
            if (_player.BaseAttributes.Dexterity != _attributes.Dexterity) return true;
            if (_player.BaseAttributes.Vitality != _attributes.Vitality) return true;
            return false;
        }

        private void PlayerStats_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (_keyBindings.ContainsKey(e.KeyCode)) _keyBindings[e.KeyCode].PerformClick();
        }

        private void BindKeys()
        {
            _keyBindings.Add(Keys.Escape, btnClose);
        }

        private void BindUI()
        {
            lblExperiencePoints.DataBindings.Add("Text", _player, "ExperiencePointsDescription");
            lblDefence.DataBindings.Add("Text", _player.Equipment, "Defence");

            lblStrength.DataBindings.Add("Text", _attributes, "Strength");
            lblStrengthEquip.DataBindings.Add("Text", _player.Equipment, "StrengthIncreasedDescription");
            lblIntelligence.DataBindings.Add("Text", _attributes, "Intelligence");
            lblIntelligenceEquip.DataBindings.Add("Text", _player.Equipment, "IntelligenceIncreasedDescription");
            lblDexterity.DataBindings.Add("Text", _attributes, "Dexterity");
            lblDexterityEquip.DataBindings.Add("Text", _player.Equipment, "DexterityIncreasedDescription");
            lblVitality.DataBindings.Add("Text", _attributes, "Vitality");
            lblVitalityEquip.DataBindings.Add("Text", _player.Equipment, "VitalityIncreasedDescription");

            lblHead.DataBindings.Add("Text", _player.Equipment, "HeadName");
            lblArms.DataBindings.Add("Text", _player.Equipment, "ArmsName");
            lblHands.DataBindings.Add("Text", _player.Equipment, "HandsName");
            lblLegs.DataBindings.Add("Text", _player.Equipment, "LegsName");
            lblFeet.DataBindings.Add("Text", _player.Equipment, "FeetName");
        }

        private void head_Click(object sender, EventArgs e)
        {
            EquippableItems equipScreen = new EquippableItems(_player, "Head");
            equipScreen.StartPosition = FormStartPosition.CenterParent;
            equipScreen.ShowDialog(this);
        }

        private void arms_Click(object sender, EventArgs e)
        {
            EquippableItems equipScreen = new EquippableItems(_player, "Arms");
            equipScreen.StartPosition = FormStartPosition.CenterParent;
            equipScreen.ShowDialog(this);
        }

        private void hands_Click(object sender, EventArgs e)
        {
            EquippableItems equipScreen = new EquippableItems(_player, "Hands");
            equipScreen.StartPosition = FormStartPosition.CenterParent;
            equipScreen.ShowDialog(this);
        }

        private void legs_Click(object sender, EventArgs e)
        {
            EquippableItems equipScreen = new EquippableItems(_player, "Legs");
            equipScreen.StartPosition = FormStartPosition.CenterParent;
            equipScreen.ShowDialog(this);
        }

        private void feet_Click(object sender, EventArgs e)
        {
            EquippableItems equipScreen = new EquippableItems(_player, "Feet");
            equipScreen.StartPosition = FormStartPosition.CenterParent;
            equipScreen.ShowDialog(this);
        }
    }
}
