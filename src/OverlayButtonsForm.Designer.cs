namespace ACTTimeline
{
    partial class OverlayButtonsForm
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRewind = new System.Windows.Forms.Button();
            this.buttonPlayPause = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRewind
            // 
            this.buttonRewind.Location = new System.Drawing.Point(0, 0);
            this.buttonRewind.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRewind.Name = "buttonRewind";
            this.buttonRewind.Size = new System.Drawing.Size(27, 20);
            this.buttonRewind.TabIndex = 4;
            this.buttonRewind.Text = "<<";
            this.buttonRewind.UseVisualStyleBackColor = true;
            // 
            // buttonPlayPause
            // 
            this.buttonPlayPause.Location = new System.Drawing.Point(27, 0);
            this.buttonPlayPause.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new System.Drawing.Size(28, 20);
            this.buttonPlayPause.TabIndex = 3;
            this.buttonPlayPause.Text = "▷";
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            // 
            // OverlayButtonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(55, 21);
            this.Controls.Add(this.buttonPlayPause);
            this.Controls.Add(this.buttonRewind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayButtonsForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRewind;
        private System.Windows.Forms.Button buttonPlayPause;

    }
}
