using DAO.Repos.Implementations;

namespace FootieForms
{
    public partial class SettingsForm : Form
    {

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            cbLanguage = new ComboBox();
            cbWorldCup = new ComboBox();
            lblWorldCup = new Label();
            lblLanguage = new Label();
            btnApply = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // cbLanguage
            // 
            resources.ApplyResources(cbLanguage, "cbLanguage");
            cbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLanguage.FormattingEnabled = true;
            cbLanguage.Items.AddRange(new object[] { resources.GetString("cbLanguage.Items"), resources.GetString("cbLanguage.Items1") });
            cbLanguage.Name = "cbLanguage";
            // 
            // cbWorldCup
            // 
            resources.ApplyResources(cbWorldCup, "cbWorldCup");
            cbWorldCup.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWorldCup.FormattingEnabled = true;
            cbWorldCup.Items.AddRange(new object[] { resources.GetString("cbWorldCup.Items"), resources.GetString("cbWorldCup.Items1") });
            cbWorldCup.Name = "cbWorldCup";
            // 
            // lblWorldCup
            // 
            resources.ApplyResources(lblWorldCup, "lblWorldCup");
            lblWorldCup.Name = "lblWorldCup";
            // 
            // lblLanguage
            // 
            resources.ApplyResources(lblLanguage, "lblLanguage");
            lblLanguage.Name = "lblLanguage";
            // 
            // btnApply
            // 
            resources.ApplyResources(btnApply, "btnApply");
            btnApply.Name = "btnApply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnExit
            // 
            resources.ApplyResources(btnExit, "btnExit");
            btnExit.Name = "btnExit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnExit);
            Controls.Add(btnApply);
            Controls.Add(lblLanguage);
            Controls.Add(lblWorldCup);
            Controls.Add(cbWorldCup);
            Controls.Add(cbLanguage);
            Name = "SettingsForm";
            FormClosing += SettingsForm_FormClosing;
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbLanguage;
        private ComboBox cbWorldCup;
        private Label lblWorldCup;
        private Label lblLanguage;
        private Button btnApply;
        private Button btnExit;
    }
}
