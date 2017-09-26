namespace AxisPlayer
{
    partial class Player
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.axAxisMediaControl1 = new AxAXISMEDIACONTROLLib.AxAxisMediaControl();
            ((System.ComponentModel.ISupportInitialize)(this.axAxisMediaControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axAxisMediaControl1
            // 
            this.axAxisMediaControl1.Enabled = true;
            this.axAxisMediaControl1.Location = new System.Drawing.Point(13, 27);
            this.axAxisMediaControl1.Name = "axAxisMediaControl1";
            this.axAxisMediaControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAxisMediaControl1.OcxState")));
            this.axAxisMediaControl1.Size = new System.Drawing.Size(329, 287);
            this.axAxisMediaControl1.TabIndex = 0;
            this.axAxisMediaControl1.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.axAxisMediaControl1_OnError);
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 337);
            this.Controls.Add(this.axAxisMediaControl1);
            this.Name = "Player";
            this.Text = "Player";
            ((System.ComponentModel.ISupportInitialize)(this.axAxisMediaControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAXISMEDIACONTROLLib.AxAxisMediaControl axAxisMediaControl1;
    }
}