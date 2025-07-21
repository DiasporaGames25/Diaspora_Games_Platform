using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GameServer_Management.Class;

namespace GameServer_Management.Forms
{
    public partial class Marketplace : Form
    {
        private FlowLayoutPanel flpGameOffers;
        private Label lblGameOffersTitle;
        private Button btnClose;

        public Marketplace()
        {
            InitializeComponent();
            this.Text = "Marketplace";
            this.BackColor = Color.FromArgb(32, 32, 36);
            this.Size = new Size(1300, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = string.Empty;

            SetupGameOffersUI();
            LoadGameOffers();
        }

        private void SetupGameOffersUI()
        {
            lblGameOffersTitle = new Label
            {
                Text = "Game Offers",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblGameOffersTitle);

            btnClose = new Button
            {
                Text = "X",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(64, 64, 64),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 30),
                Location = new Point(this.Width - 50, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            flpGameOffers = new FlowLayoutPanel
            {
                Location = new Point(30, 60),
                Size = new Size(1200, 600),
                AutoScroll = true,
                BackColor = Color.FromArgb(32, 32, 36)
            };
            this.Controls.Add(flpGameOffers);
        }

        private void LoadGameOffers()
        {
            flpGameOffers.Controls.Clear();

            string query = @"
                SELECT o.offerID, g.gameName, o.price, u.username AS sellerName
                FROM game_offers o
                JOIN gamestbl g ON o.gameID = g.gameID
                JOIN usertbl u  ON o.sellerID = u.userID
                WHERE o.buyerID = @buyerID AND o.status = 'Pending'
                ORDER BY o.offerDate DESC";

            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@buyerID", Login.userID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int offerID = reader.GetInt32(0);
                            string gameName = reader.GetString(1);
                            decimal price = reader.GetDecimal(2);
                            string seller = reader.GetString(3);

                            string imagePath = null; // Placeholder

                            var panel = CreateGameOfferPanel(offerID, gameName, imagePath, price, seller);
                            flpGameOffers.Controls.Add(panel);
                        }

                        if (flpGameOffers.Controls.Count == 0)
                        {
                            Label emptyLabel = new Label
                            {
                                Text = "You have no incoming game offers.",
                                ForeColor = Color.LightGray,
                                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                                AutoSize = true,
                                Location = new Point(30, 80)
                            };
                            flpGameOffers.Controls.Add(emptyLabel);
                        }
                    }
                }
            }
        }

        private Panel CreateGameOfferPanel(int offerID, string gameName, string imagePath, decimal price, string seller)
        {
            Panel panel = new Panel
            {
                Size = new Size(400, 120),
                BackColor = Color.FromArgb(45, 45, 48),
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                pic.Image = Image.FromFile(imagePath);
            }

            Label lblName = new Label
            {
                Text = $"{gameName} - ${price:N2}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(80, 10),
                AutoSize = true
            };

            Label lblSeller = new Label
            {
                Text = $"From: {seller}",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 9),
                Location = new Point(80, 35),
                AutoSize = true
            };

            Button btnAccept = new Button
            {
                Text = "Accept",
                Location = new Point(80, 65),
                Size = new Size(80, 30)
            };
            btnAccept.Click += (s, e) => AcceptGameOffer(offerID);

            Button btnDecline = new Button
            {
                Text = "Decline",
                Location = new Point(180, 65),
                Size = new Size(80, 30)
            };
            btnDecline.Click += (s, e) => DeclineGameOffer(offerID);

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblSeller);
            panel.Controls.Add(btnAccept);
            panel.Controls.Add(btnDecline);

            return panel;
        }

        private void AcceptGameOffer(int offerID)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                SqlTransaction tx = con.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE game_offers SET status = 'Accepted' WHERE offerID = @id;

                        INSERT INTO usergamestbl (userID, gameID, isArchived)
                        SELECT buyerID, gameID, 0 FROM game_offers WHERE offerID = @id;", con, tx);

                    cmd.Parameters.AddWithValue("@id", offerID);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                    MessageBox.Show("Game offer accepted! The game is now in your library.", "Success");
                }
                catch
                {
                    tx.Rollback();
                    MessageBox.Show("Failed to accept game offer.", "Error");
                }
            }

            LoadGameOffers();
        }

        private void DeclineGameOffer(int offerID)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE game_offers SET status = 'Declined' WHERE offerID = @id", con);
                cmd.Parameters.AddWithValue("@id", offerID);
                cmd.ExecuteNonQuery();
            }

            LoadGameOffers();
        }
    }
}
