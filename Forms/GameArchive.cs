using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameServer_Management.Forms
{
    public class GameArchive : Form
    {
        private FlowLayoutPanel archivedGamesPanel;
        private Label titleLabel;
        private Label emptyLabel;

        public GameArchive()
        {
            this.Text = "Game Archive";
            this.BackColor = Color.FromArgb(32, 32, 36);
            this.Size = new Size(1300, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupLayout();
        }

        private void SetupLayout()
        {
            // Title Label
            titleLabel = new Label();
            titleLabel.Text = "Game Archive";
            titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(30, 20);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            // Empty label
            emptyLabel = new Label();
            emptyLabel.Text = "No games archived.";
            emptyLabel.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            emptyLabel.ForeColor = Color.LightGray;
            emptyLabel.Location = new Point(50, 80);
            emptyLabel.Visible = false;
            emptyLabel.AutoSize = true;
            this.Controls.Add(emptyLabel);

            // FlowLayoutPanel for archived games
            archivedGamesPanel = new FlowLayoutPanel();
            archivedGamesPanel.Location = new Point(30, 60);
            archivedGamesPanel.Size = new Size(1200, 600);
            archivedGamesPanel.AutoScroll = true;
            archivedGamesPanel.BackColor = Color.FromArgb(45, 45, 48);
            this.Controls.Add(archivedGamesPanel);

            // Sample data (replace this with actual DB results)
            LoadArchivedGames();
        }

        private void LoadArchivedGames()
        {
            archivedGamesPanel.Controls.Clear();

            // TODO: Replace with actual database query
            List<string> archivedGameNames = new List<string> { "Portal 2", "Celeste", "Stardew Valley" };

            if (archivedGameNames.Count == 0)
            {
                emptyLabel.Visible = true;
                return;
            }

            emptyLabel.Visible = false;

            foreach (var gameName in archivedGameNames)
            {
                Panel gamePanel = new Panel
                {
                    Size = new Size(300, 100),
                    BackColor = Color.FromArgb(60, 60, 65),
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

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Size = new Size(80, 30),
                    Location = new Point(10, 40)
                };

                btnDelete.Click += (s, e) =>
                {
                    // TODO: Add actual DB deletion logic here
                    MessageBox.Show($"Deleted {gameName}");
                    archivedGamesPanel.Controls.Remove(gamePanel);
                };

                gamePanel.Controls.Add(lbl);
                gamePanel.Controls.Add(btnDelete);
                archivedGamesPanel.Controls.Add(gamePanel);
            }
        }
    }
}
