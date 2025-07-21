using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GameServer_Management.Class;

namespace GameServer_Management.Forms
{
    public partial class ViewFriends : Form
    {
        private Panel mainScrollPanel;
        private FlowLayoutPanel friendsPanel;

        public ViewFriends()
        {
            SetupScrollableLayout();
            LoadFriends();
        }

        private void SetupScrollableLayout()
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            mainScrollPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                Name = "mainScrollPanel",
                BackColor = Color.Black
            };

            friendsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                BackColor = Color.Black
            };

            Label header = new Label
            {
                Text = "Your Friends",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(10)
            };

            mainScrollPanel.Controls.Add(header);
            mainScrollPanel.Controls.Add(friendsPanel);
            this.Controls.Add(mainScrollPanel);
        }

        private void LoadFriends()
        {
            friendsPanel.Controls.Clear();

            string query = @"
                SELECT u.username
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
                        if (!reader.HasRows)
                        {
                            friendsPanel.Controls.Add(new Label
                            {
                                Text = "You have no friends yet.",
                                ForeColor = Color.LightGray,
                                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                                AutoSize = true,
                                Margin = new Padding(5)
                            });
                            return;
                        }

                        while (reader.Read())
                        {
                            string friendUsername = reader.GetString(0);

                            Label friendLabel = new Label
                            {
                                Text = friendUsername,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                                AutoSize = true,
                                Margin = new Padding(5)
                            };

                            friendsPanel.Controls.Add(friendLabel);
                        }
                    }
                }
            }
        }
    }
}
