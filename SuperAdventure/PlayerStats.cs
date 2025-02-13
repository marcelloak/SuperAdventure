using System.Xml.Linq;
using Engine;

namespace SuperAdventure
{
    public partial class PlayerStats : Form
    {
        private Player _player;
        private Attributes _attributes = new Attributes();
        private int _attributePointsToSpend;
        private List<Button> buttonsWhenLeveling = new List<Button>();
        Dictionary<Keys, Button> _keyBindings;

        public PlayerStats(Player player)
        {
            _player = player;
            _keyBindings = new Dictionary<Keys, Button>();
            _attributePointsToSpend = _player.AttributePointsToSpend;
            _attributes.Strength = _player.Attributes.Strength;
            _attributes.Intelligence = _player.Attributes.Intelligence;
            _attributes.Dexterity = _player.Attributes.Dexterity;
            _attributes.Vitality = _player.Attributes.Vitality;
            InitializeComponent();
            buttonsWhenLeveling.Add(btnApply);
            buttonsWhenLeveling.Add(btnStrengthPlus);
            buttonsWhenLeveling.Add(btnStrengthMinus);
            buttonsWhenLeveling.Add(btnIntelligencePlus);
            buttonsWhenLeveling.Add(btnIntelligenceMinus);
            buttonsWhenLeveling.Add(btnDexterityPlus);
            buttonsWhenLeveling.Add(btnDexterityMinus);
            buttonsWhenLeveling.Add(btnVitalityPlus);
            buttonsWhenLeveling.Add(btnVitalityMinus);
            bindKeys();
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
            _player.Attributes.Strength = _attributes.Strength;
            _player.Attributes.Intelligence = _attributes.Intelligence;
            _player.Attributes.Dexterity = _attributes.Dexterity;
            _player.Attributes.Vitality = _attributes.Vitality;
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

        private void RefreshLevelingButtons()
        {
            if (_attributePointsToSpend > 0 || StillLeveling())
            {
                ShowLevelingButtons();
                DeactivateMinusButtons();
                if (_attributePointsToSpend == 0) DeactivatePlusButtons();
            }
            else HideLevelingButtons();
        }

        private void DeactivateMinusButtons()
        {
            if (_player.Attributes.Strength == _attributes.Strength) btnStrengthMinus.Enabled = false;
            if (_player.Attributes.Intelligence == _attributes.Intelligence) btnIntelligenceMinus.Enabled = false;
            if (_player.Attributes.Dexterity == _attributes.Dexterity) btnDexterityMinus.Enabled = false;
            if (_player.Attributes.Vitality == _attributes.Vitality) btnVitalityMinus.Enabled = false;
        }

        private void DeactivatePlusButtons()
        {
            btnStrengthPlus.Enabled = false;
            btnIntelligencePlus.Enabled = false;
            btnDexterityPlus.Enabled = false;
            btnVitalityPlus.Enabled = false;
        }

        private void ShowLevelingButtons()
        {
            foreach (Button button in buttonsWhenLeveling)
            {
                button.Enabled = true;
                button.Visible = true;
            }
        }

        private void HideLevelingButtons()
        {
            foreach (Button button in buttonsWhenLeveling)
            {
                button.Enabled = false;
                button.Visible = false;
            }
        }

        private bool StillLeveling()
        {
            if (_player.Attributes.Strength != _attributes.Strength) return true;
            if (_player.Attributes.Intelligence != _attributes.Intelligence) return true;
            if (_player.Attributes.Dexterity != _attributes.Dexterity) return true;
            if (_player.Attributes.Vitality != _attributes.Vitality) return true;
            return false;
        }

        private void PlayerStats_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (_keyBindings.ContainsKey(e.KeyCode)) _keyBindings[e.KeyCode].PerformClick();
        }

        private void bindKeys()
        {
            _keyBindings.Add(Keys.Escape, btnClose);
        }

        private void BindUI()
        {
            lblExperiencePoints.DataBindings.Add("Text", _player, "ExperiencePointsDescription");
            lblStrength.DataBindings.Add("Text", _attributes, "Strength");
            lblIntelligence.DataBindings.Add("Text", _attributes, "Intelligence");
            lblDexterity.DataBindings.Add("Text", _attributes, "Dexterity");
            lblVitality.DataBindings.Add("Text", _attributes, "Vitality");
        }
    }
}
