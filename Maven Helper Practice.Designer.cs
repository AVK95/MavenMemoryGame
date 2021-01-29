
namespace MavenHelper
{
    partial class Maven_Helper_Practice
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pbTimer = new System.Windows.Forms.ProgressBar();
            this.lbLeft = new System.Windows.Forms.Label();
            this.lbRight = new System.Windows.Forms.Label();
            this.lbBottom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbStages = new System.Windows.Forms.TextBox();
            this.btLess = new System.Windows.Forms.Button();
            this.btMore = new System.Windows.Forms.Button();
            this.tBlinker = new System.Windows.Forms.Timer(this.components);
            this.tDisplay = new System.Windows.Forms.Timer(this.components);
            this.tScurry = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MavenHelper.Properties.Resources.MavenArena;
            this.pictureBox1.Location = new System.Drawing.Point(12, 71);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(645, 458);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(12, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(149, 43);
            this.btStart.TabIndex = 1;
            this.btStart.Text = "Start Game";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(397, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Time Left:";
            // 
            // pbTimer
            // 
            this.pbTimer.Location = new System.Drawing.Point(460, 23);
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(197, 23);
            this.pbTimer.TabIndex = 3;
            this.pbTimer.Value = 100;
            // 
            // lbLeft
            // 
            this.lbLeft.BackColor = System.Drawing.Color.Blue;
            this.lbLeft.Location = new System.Drawing.Point(120, 229);
            this.lbLeft.MaximumSize = new System.Drawing.Size(50, 50);
            this.lbLeft.Name = "lbLeft";
            this.lbLeft.Size = new System.Drawing.Size(50, 50);
            this.lbLeft.TabIndex = 4;
            this.lbLeft.Text = "     ";
            this.lbLeft.Click += new System.EventHandler(this.lbLeft_Click);
            // 
            // lbRight
            // 
            this.lbRight.BackColor = System.Drawing.Color.Blue;
            this.lbRight.Location = new System.Drawing.Point(457, 133);
            this.lbRight.MaximumSize = new System.Drawing.Size(50, 50);
            this.lbRight.Name = "lbRight";
            this.lbRight.Size = new System.Drawing.Size(50, 50);
            this.lbRight.TabIndex = 5;
            this.lbRight.Text = "     ";
            this.lbRight.Click += new System.EventHandler(this.lbRight_Click);
            // 
            // lbBottom
            // 
            this.lbBottom.BackColor = System.Drawing.Color.Blue;
            this.lbBottom.Location = new System.Drawing.Point(401, 372);
            this.lbBottom.MaximumSize = new System.Drawing.Size(50, 50);
            this.lbBottom.Name = "lbBottom";
            this.lbBottom.Size = new System.Drawing.Size(50, 50);
            this.lbBottom.TabIndex = 6;
            this.lbBottom.Text = "     ";
            this.lbBottom.Click += new System.EventHandler(this.lbBottom_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Stages:";
            // 
            // tbStages
            // 
            this.tbStages.Location = new System.Drawing.Point(285, 26);
            this.tbStages.Name = "tbStages";
            this.tbStages.ReadOnly = true;
            this.tbStages.Size = new System.Drawing.Size(55, 20);
            this.tbStages.TabIndex = 8;
            this.tbStages.Text = "6";
            // 
            // btLess
            // 
            this.btLess.Location = new System.Drawing.Point(263, 26);
            this.btLess.Name = "btLess";
            this.btLess.Size = new System.Drawing.Size(16, 21);
            this.btLess.TabIndex = 9;
            this.btLess.Text = "<";
            this.btLess.UseVisualStyleBackColor = true;
            this.btLess.Click += new System.EventHandler(this.btLess_Click);
            // 
            // btMore
            // 
            this.btMore.Location = new System.Drawing.Point(346, 26);
            this.btMore.Name = "btMore";
            this.btMore.Size = new System.Drawing.Size(16, 21);
            this.btMore.TabIndex = 10;
            this.btMore.Text = ">";
            this.btMore.UseVisualStyleBackColor = true;
            this.btMore.Click += new System.EventHandler(this.btMore_Click);
            // 
            // tBlinker
            // 
            this.tBlinker.Interval = 500;
            // 
            // tDisplay
            // 
            this.tDisplay.Interval = 2000;
            this.tDisplay.Tick += new System.EventHandler(this.tDisplay_Tick);
            // 
            // tScurry
            // 
            this.tScurry.Interval = 1000;
            this.tScurry.Tick += new System.EventHandler(this.tScurry_Tick);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 553);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(645, 55);
            this.label3.TabIndex = 11;
            this.label3.Text = "Use the speech recognizer to \"memorize\" the zones as they blink and then press th" +
    "e zones in the correct order to win the game!";
            // 
            // Maven_Helper_Practice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 590);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btMore);
            this.Controls.Add(this.btLess);
            this.Controls.Add(this.tbStages);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbBottom);
            this.Controls.Add(this.lbRight);
            this.Controls.Add(this.lbLeft);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Maven_Helper_Practice";
            this.Text = "Maven Helper Practice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Maven_Helper_Practice_FormClosed);
            this.Load += new System.EventHandler(this.Maven_Helper_Practice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pbTimer;
        private System.Windows.Forms.Label lbLeft;
        private System.Windows.Forms.Label lbRight;
        private System.Windows.Forms.Label lbBottom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbStages;
        private System.Windows.Forms.Button btLess;
        private System.Windows.Forms.Button btMore;
        private System.Windows.Forms.Timer tBlinker;
        private System.Windows.Forms.Timer tDisplay;
        private System.Windows.Forms.Timer tScurry;
        private System.Windows.Forms.Label label3;
    }
}