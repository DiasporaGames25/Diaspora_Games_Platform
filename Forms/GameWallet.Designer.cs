namespace GameServer_Management.Forms
{
    partial class GameWallet
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel gamePanelContainer;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gamePanelContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            // 
            // gamePanelContainer
            // 
            this.gamePanelContainer.Location = new System.Drawing.Point(50, 50);
            this.gamePanelContainer.Name = "gamePanelContainer";
            this.gamePanelContainer.Size = new System.Drawing.Size(1200, 550);
            this.gamePanelContainer.TabIndex = 0;
            this.gamePanelContainer.AutoScroll = true;

            // 
            // GameWallet
            // 
            this.ClientSize = new System.Drawing.Size(1320, 680);
            this.Controls.Add(this.gamePanelContainer);
            this.Name = "GameWallet";
            this.Text = "Game Wallet";
            this.ResumeLayout(false);
        }
    }
}
