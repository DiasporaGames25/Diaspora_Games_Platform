using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using GameServer_Management.Class;

namespace GameServer_Management.Forms
{
    public partial class SellToFriendForm : Form
    {
        public int SelectedFriendID { get; private set; } = -1;
        public decimal SalePrice { get; private set; } = 0;

        public SellToFriendForm()
        {
            InitializeComponent();
            LoadFriends();
        }

        private void LoadFriends()
        {
            string query = @"
    SELECT u.userID, u.username
    FROM usertbl u
    WHERE u.userID IN (
        SELECT 
            CASE 
                WHEN senderID = @userID THEN receiverID
                WHEN receiverID = @userID THEN senderID
            END
        FROM friendrequesttbl
        WHERE (senderID = @userID OR receiverID = @userID)
          AND status = 'Accepted'
    )";

            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userID", Login.userID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            ComboBoxFriends.Items.Add(new ComboBoxItem(name, id));
                        }
                    }
                }
            }
        }

        private void OfferSaleButton_Click(object sender, EventArgs e)
        {
            if (ComboBoxFriends.SelectedItem is ComboBoxItem selected &&
                decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                SelectedFriendID = selected.Value;
                SalePrice = price;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a friend and enter a valid price.");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class ComboBoxItem
        {
            public string Text { get; }
            public int Value { get; }

            public ComboBoxItem(string text, int value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
