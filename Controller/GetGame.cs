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

        private void getbtn_Click(object sender, EventArgs e)
        {
            int userId = Login.userID; // Replace with how you track current user
            int gameId = this.id;

            string query = "INSERT INTO usergamestbl (userID, gameID) VALUES (@userID, @gameID)";
            Hashtable h = new Hashtable
    {
        { "@userID", userId },
        { "@gameID", gameId }
    };

            if (DBconnect.SQL(query, h) > 0)
            {
                MessageBox.Show("Game is added to your Library");
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
