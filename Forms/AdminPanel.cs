﻿using GameServer_Management.Class;
using GameServer_Management.Controller;
using Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameServer_Management.Forms
{
    public partial class AdminPanel : Form
    {
        private KryptonCheckButton cb = new KryptonCheckButton();
        private GameCatView catView = new GameCatView();
        private GameDB gameDB = new GameDB();
        private Home home = new Home();
        private AdminHome adminHome = new AdminHome();
        private UserDB userDB = new UserDB();
        private AdminDB adminDB = new AdminDB();
        private Download download = new Download();
        private GameArchive gameArchive = new GameArchive();


        private Home adminhome;
        private bool isAdmin;

        public AdminPanel(bool isAdmin)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            cb = btnHome;
            cb.Checked = true;

            this.isAdmin = isAdmin;
            adminhome = new Home(this);
        }
        public AdminPanel(bool isAdmin, string username)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            cb = btnHome;
            cb.Checked = true;

            this.isAdmin = isAdmin;
            usertxt.Text = username;
            adminhome = new Home(this);

            //Utility.ClickEvent(userpanel, userpanel_Click);
        }

        static AdminPanel obj;
        public static AdminPanel Instance(bool isAdmin)
        {
            if (obj == null)
            {
                obj = new AdminPanel(isAdmin);
            }
            return obj;
        }
        public void LoadForm(Form f)
        {
            if (f == null)
            {
                MessageBox.Show("Form is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            //f.Parent = this;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Visible = true;
        }

        private void cuiButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
       
        private void sliderShowTimer_Tick(object sender, EventArgs e)
        {
            if (slidePanel.Size.Width < 222)
            {
                int targetWidth = 230;
                int i = 50;

                if (slidePanel.Size.Width + i > targetWidth)
                {
                    i = targetWidth - slidePanel.Size.Width;
                }
                int x = slidePanel.Size.Width + i;  //increment dynamically
                int y = slidePanel.Size.Height;
                slidePanel.Size = new Size(x, y);

                int a = mainpanel.Size.Width - i;
                int b = mainpanel.Size.Height;
                mainpanel.Size = new Size(a, b);

                int lx = mainpanel.Location.X + i;
                int ly = mainpanel.Location.Y;
                mainpanel.Location = new Point(lx, ly);
            }
            else
            {
                sliderShowTimer.Stop();
            }
        }

        private void sliderHideTimer_Tick(object sender, EventArgs e)
        {
            if (slidePanel.Size.Width > 70)
            {
                int targetWidth = 70;
                int i = 50;

                if (slidePanel.Size.Width - i > targetWidth)
                {
                    i = slidePanel.Size.Width - targetWidth;
                }
                int x = slidePanel.Size.Width - i;  //decrement dynamically
                int y = slidePanel.Size.Height;
                slidePanel.Size = new Size(x, y);

                int a = mainpanel.Size.Width + i;
                int b = mainpanel.Size.Height;
                mainpanel.Size = new Size(a, b);

                int ly = mainpanel.Location.Y;
                int lx = mainpanel.Location.X - i;
                mainpanel.Location = new Point(lx, ly);
            }
            else
            {
                sliderHideTimer.Stop();
            }
        }

        private void menubtn_Click(object sender, EventArgs e)
        {
            if (!sliderShowTimer.Enabled && !sliderHideTimer.Enabled)
            {
                if (slidePanel.Size.Width == 230)   //for minimize
                {
                    sliderHideTimer.Start();

                    string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imgPath = Path.Combine(projectDir, "asset", "menu-4-32.png");
                    menubtn.Image = Image.FromFile(imgPath);
                }
                else if (slidePanel.Size.Width == 70)   //for expand
                {
                    sliderShowTimer.Start();

                    string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imgPath = Path.Combine(projectDir, "asset", "menu-96.png");
                    menubtn.Image = Image.FromFile(imgPath);
                }
            }
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            if(isAdmin)
            {
                LoadForm(adminHome);
            }
            else
            {
                LoadForm(home);
            }
            this.Opacity = 0;
            faddingTimer.Start();
            obj = this;
            downloadbtn.Visible = false;
            userpanel.Visible = false;
            archivebtn.Visible = false;


            if (!isAdmin)
            {
                categoryBtn.Visible = false;
                GameDBbtn.Visible = false;
                adminDBbtn.Visible = false;
                userDBbtn.Visible = false;
                userpanel.Visible = true;
                downloadbtn.Visible = true;
                kryptonPanel1.Location = new Point(0, 318);
                downloadbtn.Location = new Point(3, 62);
            }
        }

        private void faddingTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                faddingTimer.Stop();
            }
        }
        private void Button(KryptonCheckButton button)
        {
            cb.Checked = false;
            cb = button;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (!btnHome.Checked)
            {
                btnHome.Checked = true;
                return;
            }
            if (isAdmin)
            {
                LoadForm(adminHome);
            }
            else
            {
                LoadForm(home);
            }
            //LoadForm(home);
            Button(btnHome);
        }

        private void userDBbtn_Click(object sender, EventArgs e)
        {
            if (!userDBbtn.Checked)
            {
                userDBbtn.Checked = true;
                return;
            }
            LoadForm(userDB);
            Button(userDBbtn);
        }

        private void adminDBbtn_Click(object sender, EventArgs e)
        {
            if (!adminDBbtn.Checked)
            {
                adminDBbtn.Checked = true;
                return;
            }
            LoadForm(adminDB);
            Button(adminDBbtn);
        }

        private void GameDBbtn_Click(object sender, EventArgs e)
        {
            if (!GameDBbtn.Checked)
            {
                GameDBbtn.Checked = true;
                return;
            }
            LoadForm(gameDB);
            Button(GameDBbtn);
        }

        private void categoryBtn_Click(object sender, EventArgs e)
        {
            if (!categoryBtn.Checked)
            {
                categoryBtn.Checked = true;
                return;
            }
            LoadForm(catView);
            Button(categoryBtn);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                Login l = new Login();
                l.Show();
                this.Close();
            }
            else
            {
                AdminLogin adminLogin = new AdminLogin();
                adminLogin.Show();
                this.Close();
            }
        }

        private void downloadbtn_Click(object sender, EventArgs e)
        {
            if (!downloadbtn.Checked)
            {
                downloadbtn.Checked = true;
                return;
            }
            LoadForm(download);
            Button(downloadbtn);
        }

        //added May 1 2025
        private void archivebtn_Click(object sender, EventArgs e)
        {
            if (!archivebtn.Checked)
            {
                archivebtn.Checked = true;
                return;
            }
            LoadForm(gameArchive);
            Button(archivebtn);
        }


        private void LoadInfo()
        {
            SignUp s = new SignUp();
            string user = usertxt.Text;
            string query = @"select userid, firstname, lastname, gender, email, username, upass, dob from usertbl where username = @username";

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(DBconnect.cs))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error! {ex.Message}");
                    return;
                }
            }
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                s.txtFirstName.Text = row["firstname"].ToString();
                s.txtLastName.Text = row["lastname"].ToString();
                s.id = Convert.ToInt32(row["userid"]);
                s.toplabel.Text = $"{s.txtLastName.Text}'s Profile";
                s.txtpass.ReadOnly = true;
                s.hide_pass_btn.Enabled = false;
                s.pictureBox2.Visible = true;

                string gender = row["gender"].ToString();
                if (gender == "Male")
                {
                    s.maleRB.Checked = true;
                }
                else
                {
                    s.femaleRB.Checked = true;
                }

                s.txtEmail.Text = row["email"].ToString();
                s.txtUsername.Text = row["username"].ToString();
                s.txtpass.Text = row["upass"].ToString();

                DateTime dob;
                if (DateTime.TryParse(row["dob"].ToString(), out dob))
                {
                    s.txtDob.Text = dob.ToString("dd-MM-yyyy");
                }
                else
                {
                    s.txtDob.Text = row["dob"].ToString();
                }
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            s.Show();
        }

        private void userpanel_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void usericonbtn_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void usertxt_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }
    }
}
