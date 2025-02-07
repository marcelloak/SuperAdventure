﻿using System;
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
    public partial class TradingScreen : Form
    {
        private Player _currentPlayer;
        Dictionary<Keys, Button> _keyBindings;

        public TradingScreen(Player player)
        {
            _currentPlayer = player;
            _keyBindings = new Dictionary<Keys, Button>();
            InitializeComponent();
            bindKeys();
            bindUI();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvMyItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4) return;
            var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;
            Item itemBeingSold = World.ItemByID(Convert.ToInt32(itemID));
            if (itemBeingSold.Price == null) MessageBox.Show("You cannot sell the " + itemBeingSold.Name);
            else
            {
                _currentPlayer.RemoveItemFromInventory(itemBeingSold);
                _currentPlayer.Gold += (int)itemBeingSold.Price;
                _currentPlayer.CurrentLocation.VendorWorkingHere.AddItemToInventory(itemBeingSold);
            }
        }

        private void dgvVendorItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4) return;
            var itemID = dgvVendorItems.Rows[e.RowIndex].Cells[0].Value;
            Item itemBeingBought= World.ItemByID(Convert.ToInt32(itemID));
            if (_currentPlayer.Gold >= itemBeingBought.Price)
            {
                _currentPlayer.AddItemToInventory(itemBeingBought);
                _currentPlayer.Gold -= (int)itemBeingBought.Price;
                _currentPlayer.CurrentLocation.VendorWorkingHere.RemoveItemFromInventory(itemBeingBought);
            }
            else MessageBox.Show("You do not have enough gold to buy the " + itemBeingBought.Name);
        }

        private void TradingScreen_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (_keyBindings.ContainsKey(e.KeyCode)) _keyBindings[e.KeyCode].PerformClick();
        }

        private void bindKeys()
        {
            _keyBindings.Add(Keys.Escape, btnClose);
        }

        private void bindUI()
        {
            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvMyItems.RowHeadersVisible = false;
            dgvMyItems.AutoGenerateColumns = false;
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });
            dgvMyItems.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Sell 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });
            dgvMyItems.DataSource = _currentPlayer.Inventory;
            dgvMyItems.CellClick += dgvMyItems_CellClick;

            dgvVendorItems.RowHeadersVisible = false;
            dgvVendorItems.AutoGenerateColumns = false;
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });
            dgvVendorItems.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Buy 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });
            dgvVendorItems.DataSource = _currentPlayer.CurrentLocation.VendorWorkingHere.Inventory;
            dgvVendorItems.CellClick += dgvVendorItems_CellClick;
        }
    }
}
