namespace FootieForms
{
    partial class PlayerData
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerData));
            pbPlayerPicture = new PictureBox();
            lblPN = new Label();
            lblSN = new Label();
            lblP = new Label();
            lblC = new Label();
            pbStarIcon = new PictureBox();
            btnChooseImage = new Button();
            lblCaptain = new Label();
            lblPosition = new Label();
            lblShirtNumber = new Label();
            lblPlayerName = new Label();
            ((System.ComponentModel.ISupportInitialize)pbPlayerPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStarIcon).BeginInit();
            SuspendLayout();
            // 
            // pbPlayerPicture
            // 
            resources.ApplyResources(pbPlayerPicture, "pbPlayerPicture");
            pbPlayerPicture.Image = Properties.Resources.WPW_2021_BahrainGP_4;
            pbPlayerPicture.Name = "pbPlayerPicture";
            pbPlayerPicture.TabStop = false;
            // 
            // lblPN
            // 
            resources.ApplyResources(lblPN, "lblPN");
            lblPN.Name = "lblPN";
            // 
            // lblSN
            // 
            resources.ApplyResources(lblSN, "lblSN");
            lblSN.Name = "lblSN";
            // 
            // lblP
            // 
            resources.ApplyResources(lblP, "lblP");
            lblP.Name = "lblP";
            // 
            // lblC
            // 
            resources.ApplyResources(lblC, "lblC");
            lblC.Name = "lblC";
            // 
            // pbStarIcon
            // 
            resources.ApplyResources(pbStarIcon, "pbStarIcon");
            pbStarIcon.Image = Properties.Resources.Liverpool_logo;
            pbStarIcon.Name = "pbStarIcon";
            pbStarIcon.TabStop = false;
            // 
            // btnChooseImage
            // 
            resources.ApplyResources(btnChooseImage, "btnChooseImage");
            btnChooseImage.Name = "btnChooseImage";
            btnChooseImage.UseVisualStyleBackColor = true;
            btnChooseImage.Click += btnChooseImage_Click;
            // 
            // lblCaptain
            // 
            resources.ApplyResources(lblCaptain, "lblCaptain");
            lblCaptain.Name = "lblCaptain";
            // 
            // lblPosition
            // 
            resources.ApplyResources(lblPosition, "lblPosition");
            lblPosition.Name = "lblPosition";
            // 
            // lblShirtNumber
            // 
            resources.ApplyResources(lblShirtNumber, "lblShirtNumber");
            lblShirtNumber.Name = "lblShirtNumber";
            // 
            // lblPlayerName
            // 
            resources.ApplyResources(lblPlayerName, "lblPlayerName");
            lblPlayerName.Name = "lblPlayerName";
            // 
            // PlayerData
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnChooseImage);
            Controls.Add(pbStarIcon);
            Controls.Add(lblCaptain);
            Controls.Add(lblPosition);
            Controls.Add(lblShirtNumber);
            Controls.Add(lblPlayerName);
            Controls.Add(lblC);
            Controls.Add(lblP);
            Controls.Add(lblSN);
            Controls.Add(lblPN);
            Controls.Add(pbPlayerPicture);
            Name = "PlayerData";
            Load += PlayerData_Load;
            ((System.ComponentModel.ISupportInitialize)pbPlayerPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStarIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbPlayerPicture;
        private Label lblPN;
        private Label lblSN;
        private Label lblP;
        private Label lblC;
        private PictureBox pbStarIcon;
        private Button btnChooseImage;
        private Label lblCaptain;
        private Label lblPosition;
        private Label lblShirtNumber;
        private Label lblPlayerName;
    }
}
