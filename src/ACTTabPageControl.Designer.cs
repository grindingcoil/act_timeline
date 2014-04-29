namespace ACTTimeline
{
    partial class ACTTabPageControl
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
            this.buttonResourceDirSelect = new System.Windows.Forms.Button();
            this.textBoxResourceDir = new System.Windows.Forms.TextBox();
            this.labelResourceDir = new System.Windows.Forms.Label();
            this.groupBoxEnvironment = new System.Windows.Forms.GroupBox();
            this.labelResourceDirStatus = new System.Windows.Forms.Label();
            this.buttonResourceDirOpen = new System.Windows.Forms.Button();
            this.groupBoxTimelines = new System.Windows.Forms.GroupBox();
            this.buttonRefreshList = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.listTimelines = new System.Windows.Forms.ListBox();
            this.groupBoxOverlay = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.udOverlayX = new System.Windows.Forms.NumericUpDown();
            this.udOverlayY = new System.Windows.Forms.NumericUpDown();
            this.labelOverlayX = new System.Windows.Forms.Label();
            this.labelOverlayY = new System.Windows.Forms.Label();
            this.labelNumRows = new System.Windows.Forms.Label();
            this.udNumRows = new System.Windows.Forms.NumericUpDown();
            this.groupBoxEnvironment.SuspendLayout();
            this.groupBoxTimelines.SuspendLayout();
            this.groupBoxOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumRows)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonResourceDirSelect
            // 
            this.buttonResourceDirSelect.Location = new System.Drawing.Point(435, 42);
            this.buttonResourceDirSelect.Name = "buttonResourceDirSelect";
            this.buttonResourceDirSelect.Size = new System.Drawing.Size(29, 20);
            this.buttonResourceDirSelect.TabIndex = 0;
            this.buttonResourceDirSelect.Text = "...";
            this.buttonResourceDirSelect.UseVisualStyleBackColor = true;
            this.buttonResourceDirSelect.Click += new System.EventHandler(this.buttonResourceDirSelect_Click);
            // 
            // textBoxResourceDir
            // 
            this.textBoxResourceDir.Location = new System.Drawing.Point(11, 42);
            this.textBoxResourceDir.Name = "textBoxResourceDir";
            this.textBoxResourceDir.Size = new System.Drawing.Size(418, 20);
            this.textBoxResourceDir.TabIndex = 1;
            this.textBoxResourceDir.TextChanged += new System.EventHandler(this.textBoxResourceDir_TextChanged);
            // 
            // labelResourceDir
            // 
            this.labelResourceDir.Location = new System.Drawing.Point(8, 16);
            this.labelResourceDir.Name = "labelResourceDir";
            this.labelResourceDir.Size = new System.Drawing.Size(178, 23);
            this.labelResourceDir.TabIndex = 2;
            this.labelResourceDir.Text = "Resources Directory:";
            this.labelResourceDir.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBoxEnvironment
            // 
            this.groupBoxEnvironment.Controls.Add(this.labelResourceDirStatus);
            this.groupBoxEnvironment.Controls.Add(this.textBoxResourceDir);
            this.groupBoxEnvironment.Controls.Add(this.labelResourceDir);
            this.groupBoxEnvironment.Controls.Add(this.buttonResourceDirOpen);
            this.groupBoxEnvironment.Controls.Add(this.buttonResourceDirSelect);
            this.groupBoxEnvironment.Location = new System.Drawing.Point(16, 271);
            this.groupBoxEnvironment.Name = "groupBoxEnvironment";
            this.groupBoxEnvironment.Size = new System.Drawing.Size(470, 109);
            this.groupBoxEnvironment.TabIndex = 3;
            this.groupBoxEnvironment.TabStop = false;
            this.groupBoxEnvironment.Text = "Environment";
            // 
            // labelResourceDirStatus
            // 
            this.labelResourceDirStatus.AutoSize = true;
            this.labelResourceDirStatus.Location = new System.Drawing.Point(13, 69);
            this.labelResourceDirStatus.Name = "labelResourceDirStatus";
            this.labelResourceDirStatus.Size = new System.Drawing.Size(102, 13);
            this.labelResourceDirStatus.TabIndex = 3;
            this.labelResourceDirStatus.Text = "Resource Dir Status";
            // 
            // buttonResourceDirOpen
            // 
            this.buttonResourceDirOpen.Location = new System.Drawing.Point(379, 68);
            this.buttonResourceDirOpen.Name = "buttonResourceDirOpen";
            this.buttonResourceDirOpen.Size = new System.Drawing.Size(85, 22);
            this.buttonResourceDirOpen.TabIndex = 0;
            this.buttonResourceDirOpen.Text = "Open Folder";
            this.buttonResourceDirOpen.UseVisualStyleBackColor = true;
            this.buttonResourceDirOpen.Click += new System.EventHandler(this.buttonResourceDirOpen_Click);
            // 
            // groupBoxTimelines
            // 
            this.groupBoxTimelines.Controls.Add(this.buttonRefreshList);
            this.groupBoxTimelines.Controls.Add(this.buttonLoad);
            this.groupBoxTimelines.Controls.Add(this.listTimelines);
            this.groupBoxTimelines.Location = new System.Drawing.Point(15, 17);
            this.groupBoxTimelines.Name = "groupBoxTimelines";
            this.groupBoxTimelines.Size = new System.Drawing.Size(470, 242);
            this.groupBoxTimelines.TabIndex = 4;
            this.groupBoxTimelines.TabStop = false;
            this.groupBoxTimelines.Text = "Timelines";
            // 
            // buttonRefreshList
            // 
            this.buttonRefreshList.Location = new System.Drawing.Point(375, 205);
            this.buttonRefreshList.Name = "buttonRefreshList";
            this.buttonRefreshList.Size = new System.Drawing.Size(89, 27);
            this.buttonRefreshList.TabIndex = 1;
            this.buttonRefreshList.Text = "Refresh List";
            this.buttonRefreshList.UseVisualStyleBackColor = true;
            this.buttonRefreshList.Click += new System.EventHandler(this.buttonRefreshList_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(376, 19);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(89, 27);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // listTimelines
            // 
            this.listTimelines.FormattingEnabled = true;
            this.listTimelines.Location = new System.Drawing.Point(12, 20);
            this.listTimelines.Name = "listTimelines";
            this.listTimelines.Size = new System.Drawing.Size(355, 212);
            this.listTimelines.TabIndex = 0;
            // 
            // groupBoxOverlay
            // 
            this.groupBoxOverlay.Controls.Add(this.labelOverlayY);
            this.groupBoxOverlay.Controls.Add(this.labelOverlayX);
            this.groupBoxOverlay.Controls.Add(this.udOverlayY);
            this.groupBoxOverlay.Controls.Add(this.udNumRows);
            this.groupBoxOverlay.Controls.Add(this.udOverlayX);
            this.groupBoxOverlay.Controls.Add(this.labelNumRows);
            this.groupBoxOverlay.Controls.Add(this.label1);
            this.groupBoxOverlay.Location = new System.Drawing.Point(16, 395);
            this.groupBoxOverlay.Name = "groupBoxOverlay";
            this.groupBoxOverlay.Size = new System.Drawing.Size(469, 94);
            this.groupBoxOverlay.TabIndex = 5;
            this.groupBoxOverlay.TabStop = false;
            this.groupBoxOverlay.Text = "Overlay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Overlay Orientation";
            // 
            // udOverlayX
            // 
            this.udOverlayX.Location = new System.Drawing.Point(159, 23);
            this.udOverlayX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udOverlayX.Name = "udOverlayX";
            this.udOverlayX.Size = new System.Drawing.Size(80, 20);
            this.udOverlayX.TabIndex = 1;
            this.udOverlayX.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udOverlayX.ValueChanged += new System.EventHandler(this.udOverlayX_ValueChanged);
            // 
            // udOverlayY
            // 
            this.udOverlayY.Location = new System.Drawing.Point(289, 23);
            this.udOverlayY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udOverlayY.Name = "udOverlayY";
            this.udOverlayY.Size = new System.Drawing.Size(80, 20);
            this.udOverlayY.TabIndex = 1;
            this.udOverlayY.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udOverlayY.ValueChanged += new System.EventHandler(this.udOverlayY_ValueChanged);
            // 
            // labelOverlayX
            // 
            this.labelOverlayX.AutoSize = true;
            this.labelOverlayX.Location = new System.Drawing.Point(139, 25);
            this.labelOverlayX.Name = "labelOverlayX";
            this.labelOverlayX.Size = new System.Drawing.Size(17, 13);
            this.labelOverlayX.TabIndex = 2;
            this.labelOverlayX.Text = "X:";
            // 
            // labelOverlayY
            // 
            this.labelOverlayY.AutoSize = true;
            this.labelOverlayY.Location = new System.Drawing.Point(269, 25);
            this.labelOverlayY.Name = "labelOverlayY";
            this.labelOverlayY.Size = new System.Drawing.Size(17, 13);
            this.labelOverlayY.TabIndex = 2;
            this.labelOverlayY.Text = "Y:";
            // 
            // labelNumRows
            // 
            this.labelNumRows.AutoSize = true;
            this.labelNumRows.Location = new System.Drawing.Point(6, 57);
            this.labelNumRows.Name = "labelNumRows";
            this.labelNumRows.Size = new System.Drawing.Size(131, 13);
            this.labelNumRows.TabIndex = 0;
            this.labelNumRows.Text = "Number of rows to display:";
            // 
            // udNumRows
            // 
            this.udNumRows.Location = new System.Drawing.Point(159, 55);
            this.udNumRows.Name = "udNumRows";
            this.udNumRows.Size = new System.Drawing.Size(40, 20);
            this.udNumRows.TabIndex = 1;
            this.udNumRows.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // ACTTabPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxOverlay);
            this.Controls.Add(this.groupBoxTimelines);
            this.Controls.Add(this.groupBoxEnvironment);
            this.Name = "ACTTabPageControl";
            this.Size = new System.Drawing.Size(500, 500);
            this.groupBoxEnvironment.ResumeLayout(false);
            this.groupBoxEnvironment.PerformLayout();
            this.groupBoxTimelines.ResumeLayout(false);
            this.groupBoxOverlay.ResumeLayout(false);
            this.groupBoxOverlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumRows)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonResourceDirSelect;
        private System.Windows.Forms.TextBox textBoxResourceDir;
        private System.Windows.Forms.Label labelResourceDir;
        private System.Windows.Forms.GroupBox groupBoxEnvironment;
        private System.Windows.Forms.Button buttonResourceDirOpen;
        private System.Windows.Forms.GroupBox groupBoxTimelines;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.ListBox listTimelines;
        private System.Windows.Forms.Button buttonRefreshList;
        private System.Windows.Forms.Label labelResourceDirStatus;
        private System.Windows.Forms.GroupBox groupBoxOverlay;
        private System.Windows.Forms.Label labelOverlayY;
        private System.Windows.Forms.Label labelOverlayX;
        private System.Windows.Forms.NumericUpDown udOverlayY;
        private System.Windows.Forms.NumericUpDown udNumRows;
        private System.Windows.Forms.NumericUpDown udOverlayX;
        private System.Windows.Forms.Label labelNumRows;
        private System.Windows.Forms.Label label1;
    }
}
