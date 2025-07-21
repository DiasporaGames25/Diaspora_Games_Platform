using GameServer_Management.Class;
using GameServer_Management.Forms;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace GameServer_Management.Controller
{
    public partial class GetGame : UserControl
    {
        public GetGame()
        {
            InitializeComponent();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        //added 4/28
        private void getbtn_Click(object sender, EventArgs e)
        {
            int userId = Login.userID; // Current user
            int gameId = this.id;

            // Generate a license key
            string licenseKey = Guid.NewGuid().ToString();

            string query = "INSERT INTO usergamestbl (userID, gameID, licenseKey) VALUES (@userID, @gameID, @licenseKey)";
            Hashtable h = new Hashtable
    {
        { "@userID", userId },
        { "@gameID", gameId },
        { "@licenseKey", licenseKey }
    };

            //modified 4/29
            if (DBconnect.SQL(query, h) > 0)
            {
                // DEBUG: check what's inside gameprice.Content
                string rawPrice = gameprice.Content.ToString().Replace("$", "").Replace(@"\", "").Trim();
                MessageBox.Show($"DEBUG: gameprice.Content = '{rawPrice}'");

                // Try parsing once
                if (!decimal.TryParse(rawPrice, out decimal parsedPrice))
                {
                    MessageBox.Show($"Invalid price format: '{rawPrice}'", "Parsing Error");
                    return;
                }

                // Deduct balance
                string deductQuery = "UPDATE usertbl SET balance = balance - @price WHERE userID = @userID";
                Hashtable deductParams = new Hashtable {
        { "@price", parsedPrice },
        { "@userID", userId }
    };
                DBconnect.SQL(deductQuery, deductParams);

                // Refresh balance on screen
                Home.Instance?.ShowBalance();

                // Confirm
                MessageBox.Show("Game is added to your Library!");

                if (Application.OpenForms["Download"] is Download downloadForm)
                {
                    downloadForm.RefreshGameViews();
                }

            }
            else
            {
                MessageBox.Show("Failed to add game to Library");
            }





        }


        public int id { get; set; }
        public string desc
        {
            get { return gameDesc.Content; }
            set { gameDesc.Content = value; }
        }
        public string GName
        {
            get { return gameName.Content; }
            set { gameName.Content = value; }
        }
        public string Price
        {
            get { return gameprice.Text; }
            set { gameprice.Content = value; }
        }
        public Image Pic
        {
            get { return gameImg.Image; }
            set { gameImg.Image = value; }
        }
        public string Category
        {
            get { return gamecat.Content; }
            set { gamecat.Content = value; }
        }
    }
}
