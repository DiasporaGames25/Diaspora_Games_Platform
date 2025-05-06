using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameServer_Management.Class;
using System.Data.SqlClient;

namespace GameServer_Management.Forms
{
    public partial class Download : Form
    {
        private FlowLayoutPanel downloadedGamesPanel;

        public int gameid = 0;

        public Download()
        {
            InitializeComponent();


            // Create and add panel to form
            downloadedGamesPanel = new FlowLayoutPanel();
            downloadedGamesPanel.Location = new Point(50, 50);
            downloadedGamesPanel.Size = new Size(1200, 550);
            downloadedGamesPanel.AutoScroll = true;
            this.Controls.Add(downloadedGamesPanel);

            //added 4/7
            LoadUserLibrary();

            // Attach the Load event handler
            this.Load += new EventHandler(Download_Load);
        }

        private void Download_Load(object sender, EventArgs e)
        {
            string downloadDir = @"C:\GameHub_DownloadedGames";

            if (!Directory.Exists(downloadDir))
                return;

            var gameDirs = Directory.GetDirectories(downloadDir);
            foreach (var gameDir in gameDirs)
            {
                string gameName = Path.GetFileName(gameDir);
                string exePath = Directory.GetFiles(gameDir, "*.exe", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (exePath != null)
                {
                    Panel gamePanel = new Panel();
                    gamePanel.Size = new Size(300, 100);
                    gamePanel.BackColor = Color.FromArgb(45, 45, 48);
                    gamePanel.Margin = new Padding(10);

                    Label lbl = new Label();
                    lbl.Text = gameName;
                    lbl.ForeColor = Color.White;
                    lbl.Font = new Font("Segoe UI", 12);
                    lbl.AutoSize = true;
                    lbl.Location = new Point(10, 10);

                    Button btnPlay = new Button();
                    btnPlay.Text = "Play";
                    btnPlay.Size = new Size(80, 30);
                    btnPlay.Location = new Point(10, 40);
                    btnPlay.Click += (s, ev) => Process.Start(exePath);

                    gamePanel.Controls.Add(lbl);
                    gamePanel.Controls.Add(btnPlay);
                    downloadedGamesPanel.Controls.Add(gamePanel);
                }
            }

            // Hide "Nothing Downloaded Yet" message
            if (downloadedGamesPanel.Controls.Count > 0)
            {
                pictureBox1.Visible = false;
                label1.Visible = false;
            }
        }

        private void LoadUserLibrary()
        {
            System.Diagnostics.Debug.WriteLine("▶ Entering LoadUserLibrary...");


            

            string query = @"
            SELECT g.gameID, g.gameName, g.exePath, g.gameFileDir
            FROM usergamestbl u
            JOIN gamestbl g ON u.gameID = g.gameID
            WHERE u.userID = @userID
            ";

            System.Diagnostics.Debug.WriteLine("▶ Query: " + query);



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
                            label1.Text = "Nothing Downloaded Yet";
                            return;
                        }

                        label1.Visible = false;
                        pictureBox1.Visible = false;
                        downloadedGamesPanel.Controls.Clear();


                        //added 4/24
                        HashSet<string> seenGames = new HashSet<string>();

                        foreach (DataRow row in dt.Rows)
                        {
                            string gameName = row["gameName"].ToString();


                            //added 4/24
                            if (seenGames.Contains(gameName))
                                continue;
                            seenGames.Add(gameName);

                            string originalExePath = row["exePath"].ToString();
                            string targetDir = $@"C:\GameHub_DownloadedGames\{gameName}";
                            //string targetExe = Path.Combine(targetDir, $"{gameName}.exe");


                            string exeName = Path.GetFileName(originalExePath); // From DB
                            string targetExe = Path.Combine(targetDir, "GameFiles", exeName);
                            bool isInstalled = File.Exists(targetExe);

                            Panel gamePanel = new Panel
                            {
                                Size = new Size(300, 100),
                                BackColor = Color.FromArgb(45, 45, 48),
                                Margin = new Padding(10)
                            };

                            Label lbl = new Label
                            {
                                Text = gameName,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 12),
                                AutoSize = true,
                                Location = new Point(10, 10)
                            };

                            Button actionButton = new Button
                            {
                                Size = new Size(80, 30),
                                Location = new Point(10, 40),
                                Text = isInstalled ? "Play" : "Download"
                            };

                            //if (isInstalled)
                            //{
                            //    actionButton.Click += (s, e) =>
                            //    {
                            //        try
                            //        {
                            //            MessageBox.Show($"Launching: {targetExe}");

                            //            Process.Start(new ProcessStartInfo
                            //            {
                            //                FileName = targetExe,
                            //                UseShellExecute = true
                            //            });
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            MessageBox.Show($"Failed to start game:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //        }
                            //    };
                            //}
                            if (isInstalled)
                            {
                                actionButton.Click += (s, e) =>
                                {
                                    try
                                    {
                                        // Fetch user's licenseKey for this game
                                        string licenseKeyFromDB = null;

                                        using (SqlConnection licenseCon = DBconnect.GetConnection())
                                        {
                                            licenseCon.Open();
                                            using (SqlCommand licenseCmd = new SqlCommand(@"
                    SELECT licenseKey FROM usergamestbl
                    WHERE userID = @userID AND gameID = @gameID
                ", licenseCon))
                                            {
                                                licenseCmd.Parameters.AddWithValue("@userID", Login.userID);
                                                licenseCmd.Parameters.AddWithValue("@gameID", row["gameID"]);

                                                object result = licenseCmd.ExecuteScalar();
                                                if (result != null)
                                                {
                                                    licenseKeyFromDB = result.ToString();
                                                }
                                            }
                                        }

                                        if (string.IsNullOrEmpty(licenseKeyFromDB))
                                        {
                                            MessageBox.Show("No valid license found. Please acquire the game first.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        // If license key exists, allow launching
                                        MessageBox.Show($"Launching: {targetExe}");

                                        Process.Start(new ProcessStartInfo
                                        {
                                            FileName = targetExe,
                                            UseShellExecute = true
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Failed to start game:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                };
                            }

                            else
                            {

                          
                                actionButton.Click += (s, e) =>
                                {
                                    try
                                    {
                                        string sourceGameDir = row["gameFileDir"].ToString();
                                        string exeFileName = Path.GetFileName(originalExePath); // renamed from exeName
                                        string targetGameDir = $@"C:\GameHub_DownloadedGames\{gameName}\GameFiles";
                                        string targetExePath = Path.Combine(targetGameDir, exeFileName);

                                        Debug.WriteLine($"Source folder: {sourceGameDir}");
                                        Debug.WriteLine($"Executable: {exeFileName}");
                                        Debug.WriteLine($"Target folder: {targetGameDir}");

                                        if (!Directory.Exists(sourceGameDir))
                                        {
                                            MessageBox.Show($"Source folder does not exist: {sourceGameDir}");
                                            return;
                                        }

                                        if (!File.Exists(Path.Combine(sourceGameDir, exeFileName)))
                                        {
                                            MessageBox.Show($"Executable not found in source folder: {exeFileName}");
                                            return;
                                        }

                                        foreach (var filePath in Directory.GetFiles(sourceGameDir, "*", SearchOption.AllDirectories))
                                        {
                                            string relativePath = filePath.Substring(sourceGameDir.Length + 1);
                                            string destinationPath = Path.Combine(targetGameDir, relativePath);
                                            Debug.WriteLine($"Copying: {filePath} → {destinationPath}");

                                            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                                            File.Copy(filePath, destinationPath, true);
                                        }

                                        MessageBox.Show($"Downloaded {gameName} successfully!", "Success");
                                        LoadUserLibrary(); // Refresh UI to show Play button
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Download failed:\n{ex.Message}", "Error");
                                    }



                                };


                            }

                                gamePanel.Controls.Add(lbl);
                            gamePanel.Controls.Add(actionButton);
                            downloadedGamesPanel.Controls.Add(gamePanel);
                        }
                    }
                }
            }
        }


    }
}
       

