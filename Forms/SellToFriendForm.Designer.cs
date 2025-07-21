namespace GameServer_Management.Forms
{
    partial class SellToFriendForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox ComboBoxFriends;
        private System.Windows.Forms.TextBox PriceTextBox;
        private System.Windows.Forms.Label friendLabel;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Button OfferSaleButton;
        private System.Windows.Forms.Button CancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ComboBoxFriends = new System.Windows.Forms.ComboBox();
            this.PriceTextBox = new System.Windows.Forms.TextBox();
            this.friendLabel = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.OfferSaleButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // friendLabel
            this.friendLabel.AutoSize = true;
            this.friendLabel.Location = new System.Drawing.Point(25, 20);
            this.friendLabel.Text = "Select Friend:";
            this.friendLabel.ForeColor = System.Drawing.Color.White;

            // ComboBoxFriends
            this.ComboBoxFriends.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxFriends.Location = new System.Drawing.Point(130, 17);
            this.ComboBoxFriends.Size = new System.Drawing.Size(200, 21);

            // priceLabel
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point(25, 60);
            this.priceLabel.Text = "Price ($):";
            this.priceLabel.ForeColor = System.Drawing.Color.White;

            // PriceTextBox
            this.PriceTextBox.Location = new System.Drawing.Point(130, 57);
            this.PriceTextBox.Size = new System.Drawing.Size(200, 20);

            // OfferSaleButton
            this.OfferSaleButton.Text = "Offer Sale";
            this.OfferSaleButton.Location = new System.Drawing.Point(130, 100);
            this.OfferSaleButton.Click += new System.EventHandler(this.OfferSaleButton_Click);

            // CancelButton
            this.CancelButton.Text = "Cancel";
            this.CancelButton.Location = new System.Drawing.Point(230, 100);
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(370, 150);
            this.Controls.Add(this.friendLabel);
            this.Controls.Add(this.ComboBoxFriends);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.PriceTextBox);
            this.Controls.Add(this.OfferSaleButton);
            this.Controls.Add(this.CancelButton);
            this.BackColor = System.Drawing.Color.FromArgb(32, 32, 36);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sell Game to Friend";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}