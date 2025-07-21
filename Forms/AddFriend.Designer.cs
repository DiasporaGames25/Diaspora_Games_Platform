namespace GameServer_Management.Forms
{
    partial class AddFriend
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.FlowLayoutPanel resultsPanel;
        private System.Windows.Forms.Label noResultsLabel;
        private System.Windows.Forms.Label outgoingLabel;
        private System.Windows.Forms.FlowLayoutPanel outgoingPanel;


        private System.Windows.Forms.Label incomingLabel;
        private System.Windows.Forms.FlowLayoutPanel incomingPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.resultsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.noResultsLabel = new System.Windows.Forms.Label();
            this.outgoingLabel = new System.Windows.Forms.Label();
            this.outgoingPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.incomingLabel = new System.Windows.Forms.Label();
            this.incomingPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(30, 70);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(300, 26);
            this.searchBox.TabIndex = 0;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(340, 68);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            // 
            // resultsPanel
            // 
            this.resultsPanel.AutoScroll = true;
            this.resultsPanel.Location = new System.Drawing.Point(30, 140);
            this.resultsPanel.Name = "resultsPanel";
            this.resultsPanel.Size = new System.Drawing.Size(1297, 367);
            this.resultsPanel.TabIndex = 2;
            // 
            // noResultsLabel
            // 
            this.noResultsLabel.AutoSize = true;
            this.noResultsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
            this.noResultsLabel.ForeColor = System.Drawing.Color.LightGray;
            this.noResultsLabel.Location = new System.Drawing.Point(30, 110);
            this.noResultsLabel.Name = "noResultsLabel";
            this.noResultsLabel.Size = new System.Drawing.Size(175, 32);
            this.noResultsLabel.TabIndex = 5;
            this.noResultsLabel.Text = "No users found.";
            this.noResultsLabel.Visible = false;
            // 
            // outgoingLabel
            // 
            this.outgoingLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.outgoingLabel.ForeColor = System.Drawing.Color.White;
            this.outgoingLabel.Location = new System.Drawing.Point(30, 520);
            this.outgoingLabel.Name = "outgoingLabel";
            this.outgoingLabel.Size = new System.Drawing.Size(385, 55);
            this.outgoingLabel.TabIndex = 3;
            this.outgoingLabel.Text = "Outgoing Friend Requests";
            this.outgoingLabel.Click += new System.EventHandler(this.outgoingLabel_Click);
            // 
            // outgoingPanel
            // 
            this.outgoingPanel.AutoScroll = true;
            this.outgoingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(36)))));
            this.outgoingPanel.Location = new System.Drawing.Point(30, 578);
            this.outgoingPanel.Name = "outgoingPanel";
            this.outgoingPanel.Size = new System.Drawing.Size(1200, 150);
            this.outgoingPanel.TabIndex = 4;
            this.outgoingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.outgoingPanel_Paint);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(752, 55);
            this.label1.TabIndex = 6;
            this.label1.Text = "Add Friends (Search By Username)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // incomingLabel
            // 
            this.incomingLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.incomingLabel.ForeColor = System.Drawing.Color.White;
            this.incomingLabel.Location = new System.Drawing.Point(30, 750);
            this.incomingLabel.Name = "incomingLabel";
            this.incomingLabel.Size = new System.Drawing.Size(400, 40);
            this.incomingLabel.TabIndex = 7;
            this.incomingLabel.Text = "Incoming Friend Requests";
            // 
            // incomingPanel
            // 
            this.incomingPanel.AutoScroll = true;
            this.incomingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(36)))));
            this.incomingPanel.Location = new System.Drawing.Point(30, 800);
            this.incomingPanel.Name = "incomingPanel";
            this.incomingPanel.Size = new System.Drawing.Size(1200, 150);
            this.incomingPanel.TabIndex = 8;
            // 
            // AddFriend
            // 
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(2998, 1590);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.resultsPanel);
            this.Controls.Add(this.noResultsLabel);
            this.Controls.Add(this.outgoingLabel);
            this.Controls.Add(this.outgoingPanel);
            this.Controls.Add(this.incomingLabel);
            this.Controls.Add(this.incomingPanel);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddFriend";
            this.Text = "Add Friend";
            this.Load += new System.EventHandler(this.AddFriend_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            

            //
            // Friends
            //


        }

        private System.Windows.Forms.Label label1;
    }
}