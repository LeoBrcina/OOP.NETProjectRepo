namespace FootieForms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnSettings = new Button();
            cbTeams = new ComboBox();
            label1 = new Label();
            pnlAllPlayers = new Panel();
            pnlFavouritePlayers = new Panel();
            pnlPlayerData = new Panel();
            btnRankLists = new Button();
            SuspendLayout();
            // 
            // btnSettings
            // 
            resources.ApplyResources(btnSettings, "btnSettings");
            btnSettings.Name = "btnSettings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // cbTeams
            // 
            resources.ApplyResources(cbTeams, "cbTeams");
            cbTeams.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTeams.FormattingEnabled = true;
            cbTeams.Name = "cbTeams";
            cbTeams.SelectedIndexChanged += cbTeams_SelectedIndexChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // pnlAllPlayers
            // 
            resources.ApplyResources(pnlAllPlayers, "pnlAllPlayers");
            pnlAllPlayers.AllowDrop = true;
            pnlAllPlayers.BackColor = SystemColors.ControlLightLight;
            pnlAllPlayers.Name = "pnlAllPlayers";
            pnlAllPlayers.DragDrop += pnlAllPlayers_DragDrop;
            pnlAllPlayers.DragEnter += pnlAllPlayers_DragEnter;
            // 
            // pnlFavouritePlayers
            // 
            resources.ApplyResources(pnlFavouritePlayers, "pnlFavouritePlayers");
            pnlFavouritePlayers.AllowDrop = true;
            pnlFavouritePlayers.BackColor = SystemColors.ControlLightLight;
            pnlFavouritePlayers.Name = "pnlFavouritePlayers";
            pnlFavouritePlayers.DragDrop += pnlFavouritePlayers_DragDrop;
            pnlFavouritePlayers.DragEnter += pnlAllPlayers_DragEnter;
            // 
            // pnlPlayerData
            // 
            resources.ApplyResources(pnlPlayerData, "pnlPlayerData");
            pnlPlayerData.BackColor = SystemColors.ControlLightLight;
            pnlPlayerData.Name = "pnlPlayerData";
            // 
            // btnRankLists
            // 
            resources.ApplyResources(btnRankLists, "btnRankLists");
            btnRankLists.Name = "btnRankLists";
            btnRankLists.UseVisualStyleBackColor = true;
            btnRankLists.Click += btnRankLists_Click;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnRankLists);
            Controls.Add(pnlPlayerData);
            Controls.Add(pnlFavouritePlayers);
            Controls.Add(pnlAllPlayers);
            Controls.Add(label1);
            Controls.Add(cbTeams);
            Controls.Add(btnSettings);
            Name = "MainForm";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSettings;
        private ComboBox cbTeams;
        private Label label1;
        private Panel pnlAllPlayers;
        private Panel pnlFavouritePlayers;
        private Panel pnlPlayerData;
        private Button btnRankLists;
    }
}