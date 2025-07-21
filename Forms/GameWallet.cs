using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GameServer_Management.Class;

namespace GameServer_Management.Forms
{
    public partial class GameWallet : Form
    {
        private FlowLayoutPanel walletPanel;
        private Label titleLabel;
        private Label emptyLabel;

        public GameWallet()
        {
            this.Text = "Game Wallet";
            this.BackColor = Color.FromArgb(32, 32, 36);
            this.Size = new Size(1300, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = string.Empty;

            SetupLayout();

            this.Shown += (s, e) => LoadWalletGames();
        }

        private void SetupLayout()
        {
            // Title Label
            titleLabel = new Label();
            titleLabel.Text = "Game Wallet";
            titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(30, 20);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            // Empty Label
            emptyLabel = new Label();
            emptyLabel.Text = "No games in wallet.";
            emptyLabel.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            emptyLabel.ForeColor = Color.LightGray;
            emptyLabel.Location = new Point(50, 80);
            emptyLabel.Visible = false;
            emptyLabel.AutoSize = true;
            this.Controls.Add(emptyLabel);

            // Panel for wallet games
            walletPanel = new FlowLayoutPanel();
            walletPanel.Location = new Point(30, 60);
            walletPanel.Size = new Size(1200, 600);
            walletPanel.AutoScroll = true;
            walletPanel.BackColor = Color.FromArgb(32, 32, 36);
            this.Controls.Add(walletPanel);

            LoadWalletGames();
        }

        public void LoadWalletGames()
        {
            walletPanel.Controls.Clear();

            string query = @"
                SELECT g.gameID, g.gameName
                FROM usergamestbl u
                JOIN gamestbl g ON u.gameID = g.gameID
                WHERE u.userID = @userID AND u.isArchived = 1
            ";

            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userID", Login.userID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            emptyLabel.Visible = true;
                            return;
                        }

                        emptyLabel.Visible = false;

                        foreach (DataRow row in dt.Rows)
                        {
                            string gameName = row["gameName"].ToString();
                            int gameID = Convert.ToInt32(row["gameID"]);

                            Panel panel = new Panel
                            {
                                Size = new Size(300, 100),
                                BackColor = Color.FromArgb(45, 45, 48),
                                Margin = new Padding(10),
                                BorderStyle = BorderStyle.None
                            };

                            Label lbl = new Label
                            {
                                Text = gameName,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 12),
                                AutoSize = true,
                                Location = new Point(10, 10)
                            };

                            Button restoreBtn = new Button
                            {
                                Text = "Restore",
                                Size = new Size(80, 30),
                                Location = new Point(10, 40)
                            };

                            restoreBtn.Click += (s, e) =>
                            {
                                RestoreGame(gameID, panel);
                            };


                            Button sellBtn = new Button
                            {
                                Text = "Sell",
                                Size = new Size(80, 30),
                                Location = new Point(100, 40) // Adjusted to be beside Restore
                            };

                            sellBtn.Click += (s, e) =>
                            {
                                SellGame(gameID, panel);
                            };

                            panel.Controls.Add(sellBtn);


                            panel.Controls.Add(lbl);
                            panel.Controls.Add(restoreBtn);
                            walletPanel.Controls.Add(panel);
                        }
                    }
                }
            }
        }




        public void RefreshWallet()
        {
            LoadWalletGames(); // or whatever method loads the wallet UI
        }

        private void RestoreGame(int gameID, Panel panel)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE usergamestbl
                    SET isArchived = 0
                    WHERE userID = @userID AND gameID = @gameID
                ", con))
                {
                    cmd.Parameters.AddWithValue("@userID", Login.userID);
                    cmd.Parameters.AddWithValue("@gameID", gameID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Game restored to your library.", "Success");
                        walletPanel.Controls.Remove(panel);

                        // ✅ Also update the Download page
                        if (Application.OpenForms["Download"] is Download downloadPage)
                        {
                            downloadPage.RefreshGameViews();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Restore failed.", "Error");
                    }
                }
            }
        }
        private void SellGame(int gameID, Panel panel)
        {
            DialogResult result = MessageBox.Show("Would you like to sell this game to a friend?", "Sell Game", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            using (SellToFriendForm sellForm = new SellToFriendForm())
            {
                if (sellForm.ShowDialog() == DialogResult.OK)
                {
                    int friendID = sellForm.SelectedFriendID;
                    decimal price = sellForm.SalePrice;

                    if (friendID != -1)
                    {
                        SubmitGameOffer(gameID, friendID, price, panel);
                    }
                }
            }
        }

        private void SubmitGameOffer(int gameID, int friendID, decimal price, Panel panel)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();

                string insertQuery = @"
            INSERT INTO game_offers (sellerID, buyerID, gameID, price, status)
            VALUES (@sellerID, @buyerID, @gameID, @price, 'Pending')";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@sellerID", Login.userID);
                    cmd.Parameters.AddWithValue("@buyerID", friendID);
                    cmd.Parameters.AddWithValue("@gameID", gameID);
                    cmd.Parameters.AddWithValue("@price", price);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Sale offer sent to your friend!", "Success");
                        walletPanel.Controls.Remove(panel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to send offer.", "Error");
                    }
                }
            }
        }


    }

}


