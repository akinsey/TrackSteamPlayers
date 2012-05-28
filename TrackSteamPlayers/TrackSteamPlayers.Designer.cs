namespace TrackSteamPlayers
{
    partial class TrackSteamPlayers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackSteamPlayers));
            this.profileInput = new System.Windows.Forms.TextBox();
            this.profileLabel = new System.Windows.Forms.Label();
            this.trackPlayer = new System.Windows.Forms.Button();
            this.removePlayer = new System.Windows.Forms.Button();
            this.playerTable = new System.Windows.Forms.ListView();
            this.playerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.game = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serverIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.steamId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // profileInput
            // 
            this.profileInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profileInput.Location = new System.Drawing.Point(87, 10);
            this.profileInput.Name = "profileInput";
            this.profileInput.Size = new System.Drawing.Size(365, 20);
            this.profileInput.TabIndex = 1;
            this.profileInput.Click += new System.EventHandler(this.profileInput_Click);
            this.profileInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.profileInput_KeyDown);
            // 
            // profileLabel
            // 
            this.profileLabel.AutoSize = true;
            this.profileLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.profileLabel.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.profileLabel.Location = new System.Drawing.Point(10, 12);
            this.profileLabel.Name = "profileLabel";
            this.profileLabel.Size = new System.Drawing.Size(70, 15);
            this.profileLabel.TabIndex = 2;
            this.profileLabel.Text = "Profile URL";
            // 
            // trackPlayer
            // 
            this.trackPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPlayer.Image = global::TrackSteamPlayers.Properties.Resources.find;
            this.trackPlayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trackPlayer.Location = new System.Drawing.Point(458, 8);
            this.trackPlayer.Name = "trackPlayer";
            this.trackPlayer.Size = new System.Drawing.Size(97, 23);
            this.trackPlayer.TabIndex = 3;
            this.trackPlayer.Text = "Track Player ";
            this.trackPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.trackPlayer.UseVisualStyleBackColor = true;
            this.trackPlayer.Click += new System.EventHandler(this.trackPlayer_Click);
            // 
            // removePlayer
            // 
            this.removePlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removePlayer.Image = global::TrackSteamPlayers.Properties.Resources.delete;
            this.removePlayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removePlayer.Location = new System.Drawing.Point(561, 8);
            this.removePlayer.Name = "removePlayer";
            this.removePlayer.Size = new System.Drawing.Size(97, 23);
            this.removePlayer.TabIndex = 5;
            this.removePlayer.Text = "Delete Player";
            this.removePlayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removePlayer.UseVisualStyleBackColor = true;
            this.removePlayer.Click += new System.EventHandler(this.removePlayer_Click);
            // 
            // playerTable
            // 
            this.playerTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playerName,
            this.game,
            this.serverIp,
            this.steamId});
            this.playerTable.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerTable.FullRowSelect = true;
            this.playerTable.GridLines = true;
            this.playerTable.Location = new System.Drawing.Point(12, 37);
            this.playerTable.Name = "playerTable";
            this.playerTable.Size = new System.Drawing.Size(645, 350);
            this.playerTable.TabIndex = 6;
            this.playerTable.UseCompatibleStateImageBehavior = false;
            this.playerTable.View = System.Windows.Forms.View.Details;
            this.playerTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.playerTable_ColumnClick);
            this.playerTable.SelectedIndexChanged += new System.EventHandler(this.playerTable_SelectedIndexChanged);
            this.playerTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playerTable_MouseDoubleClick);
            // 
            // playerName
            // 
            this.playerName.Text = "Player Name";
            this.playerName.Width = 160;
            // 
            // game
            // 
            this.game.Text = "Game";
            this.game.Width = 160;
            // 
            // serverIp
            // 
            this.serverIp.Text = "Server IP";
            this.serverIp.Width = 160;
            // 
            // steamId
            // 
            this.steamId.Text = "Steam ID";
            this.steamId.Width = 160;
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 396);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(646, 18);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 10;
            // 
            // TrackSteamPlayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 422);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.playerTable);
            this.Controls.Add(this.removePlayer);
            this.Controls.Add(this.trackPlayer);
            this.Controls.Add(this.profileLabel);
            this.Controls.Add(this.profileInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrackSteamPlayers";
            this.Text = "Track Steam Players - [made by wicked]";
            this.Load += new System.EventHandler(this.TrackSteamPlayers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox profileInput;
        private System.Windows.Forms.Label profileLabel;
        private System.Windows.Forms.Button trackPlayer;
        private System.Windows.Forms.Button removePlayer;
        private System.Windows.Forms.ListView playerTable;
        private System.Windows.Forms.ColumnHeader playerName;
        private System.Windows.Forms.ColumnHeader game;
        private System.Windows.Forms.ColumnHeader serverIp;
        private System.Windows.Forms.ColumnHeader steamId;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

