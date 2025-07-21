using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GameServer_Management.Class;

namespace GameServer_Management.Forms
{
    public partial class AddFriend : Form
    {
        private Label titleLabel;
        //private Label noResultsLabel;
        //private FlowLayoutPanel outgoingPanel;
        //private Label outgoingLabel;

        private Panel mainScrollPanel;



        public AddFriend()
        {
            InitializeComponent(); // This sets up everything from the Designer file

            SetupScrollableLayout();

            // Hook up the search button click event
            searchButton.Click += (s, e) => SearchUsers();

            // Load existing outgoing friend requests when the form opens
            LoadOutgoingFriendRequests();
            LoadIncomingFriendRequests();
        }



        private void SetupScrollableLayout()
        {
            mainScrollPanel = new Panel
            {
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 1000), // vertical scrolling only
                Dock = DockStyle.Fill,
                Name = "mainScrollPanel"
            };


            // Remove existing controls from the form and move them into the scroll panel
            Control[] originalControls = new Control[this.Controls.Count];
            this.Controls.CopyTo(originalControls, 0);

            foreach (Control control in originalControls)
            {
                this.Controls.Remove(control);
                mainScrollPanel.Controls.Add(control);
            }

            // Add the scroll panel to the form
            this.Controls.Add(mainScrollPanel);
        }


        private void SearchUsers()
        {
            resultsPanel.Controls.Clear();
            string username = searchBox.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to search.", "Input Required");
                return;
            }

            string query = "SELECT userID, username FROM usertbl WHERE username LIKE @username AND userID != @userID";

            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", "%" + username + "%");
                    cmd.Parameters.AddWithValue("@userID", Login.userID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            noResultsLabel.Visible = true;
                            return;
                        }

                        noResultsLabel.Visible = false;

                        foreach (DataRow row in dt.Rows)
                        {
                            string foundUsername = row["username"].ToString();
                            int foundUserID = Convert.ToInt32(row["userID"]);

                            Panel panel = new Panel
                            {
                                Size = new Size(300, 100),
                                BackColor = Color.FromArgb(45, 45, 48),
                                Margin = new Padding(10),
                                BorderStyle = BorderStyle.None
                            };

                            Label lbl = new Label
                            {
                                Text = foundUsername,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 12),
                                AutoSize = true,
                                Location = new Point(10, 10)
                            };

                            Button inviteBtn = new Button
                            {
                                Text = "Add Friend",
                                Size = new Size(100, 30),
                                Location = new Point(10, 40)
                            };

                            inviteBtn.Click += (s, e) =>
                            {
                                SendFriendRequest(foundUserID);
                            };

                            panel.Controls.Add(lbl);
                            panel.Controls.Add(inviteBtn);
                            resultsPanel.Controls.Add(panel);
                            LoadOutgoingFriendRequests();
                        }
                    }
                }
            }
        }

        private void SendFriendRequest(int targetUserID)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO friendrequesttbl (senderID, receiverID, status)
                    VALUES (@senderID, @receiverID, 'Pending')
                ", con))
                {
                    cmd.Parameters.AddWithValue("@senderID", Login.userID);
                    cmd.Parameters.AddWithValue("@receiverID", targetUserID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Friend request sent!");
                        LoadOutgoingFriendRequests();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error sending request: " + ex.Message);
                    }
                }
            }
        }
        private void LoadOutgoingFriendRequests()
        {
            outgoingPanel.Controls.Clear();

            Label debug = new Label
            {
                Text = "LOADING...",
                ForeColor = Color.Red,
                AutoSize = true
            };
            outgoingPanel.Controls.Add(debug);

            string query = @"
        SELECT u.username 
        FROM friendrequesttbl fr
        JOIN usertbl u ON u.userID = fr.receiverID
        WHERE fr.senderID = @userID AND fr.status = 'Pending'";

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
                            string username = reader.GetString(0);

                            Label pendingLabel = new Label
                            {
                                Text = username + " (Pending)",
                                ForeColor = Color.LightGray,
                                Font = new Font("Segoe UI", 11),
                                AutoSize = true,
                                Margin = new Padding(10)
                            };

                            outgoingPanel.Controls.Add(pendingLabel);
                        }
                    }
                }
            }
        }

        private void AcceptFriendRequest(int senderID)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                string update = "UPDATE friendrequesttbl SET status = 'Accepted' WHERE senderID = @senderID AND receiverID = @receiverID";
                using (SqlCommand cmd = new SqlCommand(update, con))
                {
                    cmd.Parameters.AddWithValue("@senderID", senderID);
                    cmd.Parameters.AddWithValue("@receiverID", Login.userID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Friend request accepted!");
            LoadIncomingFriendRequests();
            LoadOutgoingFriendRequests(); // Optional: refresh both
        }
        private void DeclineFriendRequest(int senderID)
        {
            using (SqlConnection con = DBconnect.GetConnection())
            {
                con.Open();
                string delete = "DELETE FROM friendrequesttbl WHERE senderID = @senderID AND receiverID = @receiverID AND status = 'Pending'";
                using (SqlCommand cmd = new SqlCommand(delete, con))
                {
                    cmd.Parameters.AddWithValue("@senderID", senderID);
                    cmd.Parameters.AddWithValue("@receiverID", Login.userID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Friend request declined.");
            LoadIncomingFriendRequests();
        }





        private void LoadIncomingFriendRequests()
        {
            incomingPanel.Controls.Clear();

            string query = @"
        SELECT u.userID, u.username
        FROM friendrequesttbl fr
        JOIN usertbl u ON u.userID = fr.senderID
        WHERE fr.receiverID = @userID AND fr.status = 'Pending'";

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
                            int senderID = reader.GetInt32(0);
                            string senderUsername = reader.GetString(1);

                            Panel requestPanel = new Panel
                            {
                                Size = new Size(400, 50),
                                BackColor = Color.FromArgb(45, 45, 48),
                                Margin = new Padding(10)
                            };

                            Label usernameLabel = new Label
                            {
                                Text = senderUsername,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 11),
                                Location = new Point(10, 15),
                                AutoSize = true
                            };

                            Button acceptBtn = new Button
                            {
                                Text = "Accept",
                                Location = new Point(200, 10),
                                Size = new Size(80, 30)
                            };
                            acceptBtn.Click += (s, e) => AcceptFriendRequest(senderID);

                            Button declineBtn = new Button
                            {
                                Text = "Decline",
                                Location = new Point(290, 10),
                                Size = new Size(80, 30)
                            };
                            declineBtn.Click += (s, e) => DeclineFriendRequest(senderID);

                            requestPanel.Controls.Add(usernameLabel);
                            requestPanel.Controls.Add(acceptBtn);
                            requestPanel.Controls.Add(declineBtn);
                            incomingPanel.Controls.Add(requestPanel);
                        }
                    }
                }
            }
        }


        private void outgoingPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddFriend_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void outgoingLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
