namespace FootieForms
{
    partial class RankListsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankListsForm));
            pnlPlayerData = new Panel();
            pnlRankedYellow = new Panel();
            pnlRankedGoals = new Panel();
            pnlRankedMatches = new Panel();
            btnPrintPdf = new Button();
            SuspendLayout();
            // 
            // pnlPlayerData
            // 
            pnlPlayerData.BackColor = SystemColors.ControlLightLight;
            resources.ApplyResources(pnlPlayerData, "pnlPlayerData");
            pnlPlayerData.Name = "pnlPlayerData";
            // 
            // pnlRankedYellow
            // 
            pnlRankedYellow.AllowDrop = true;
            resources.ApplyResources(pnlRankedYellow, "pnlRankedYellow");
            pnlRankedYellow.BackColor = SystemColors.ControlLightLight;
            pnlRankedYellow.Name = "pnlRankedYellow";
            // 
            // pnlRankedGoals
            // 
            pnlRankedGoals.AllowDrop = true;
            resources.ApplyResources(pnlRankedGoals, "pnlRankedGoals");
            pnlRankedGoals.BackColor = SystemColors.ControlLightLight;
            pnlRankedGoals.Name = "pnlRankedGoals";
            // 
            // pnlRankedMatches
            // 
            resources.ApplyResources(pnlRankedMatches, "pnlRankedMatches");
            pnlRankedMatches.BackColor = SystemColors.ControlLightLight;
            pnlRankedMatches.Name = "pnlRankedMatches";
            // 
            // btnPrintPdf
            // 
            resources.ApplyResources(btnPrintPdf, "btnPrintPdf");
            btnPrintPdf.Name = "btnPrintPdf";
            btnPrintPdf.UseVisualStyleBackColor = true;
            btnPrintPdf.Click += btnPrintPdf_Click;
            // 
            // RankListsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnPrintPdf);
            Controls.Add(pnlRankedMatches);
            Controls.Add(pnlPlayerData);
            Controls.Add(pnlRankedYellow);
            Controls.Add(pnlRankedGoals);
            Name = "RankListsForm";
            Load += RankListsForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPlayerData;
        private Panel pnlRankedYellow;
        private Panel pnlRankedGoals;
        private Panel pnlRankedMatches;
        private Button btnPrintPdf;
    }
}