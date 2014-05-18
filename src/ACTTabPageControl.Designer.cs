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
            this.checkBoxShowOverlayButtons = new System.Windows.Forms.CheckBox();
            this.checkBoxMoveOverlayByDrag = new System.Windows.Forms.CheckBox();
            this.labelOverlayY = new System.Windows.Forms.Label();
            this.buttonFontSelect = new System.Windows.Forms.Button();
            this.labelBar = new System.Windows.Forms.Label();
            this.labelTextWidth = new System.Windows.Forms.Label();
            this.labelOverlayX = new System.Windows.Forms.Label();
            this.udOverlayY = new System.Windows.Forms.NumericUpDown();
            this.udBarWidth = new System.Windows.Forms.NumericUpDown();
            this.udTextWidth = new System.Windows.Forms.NumericUpDown();
            this.udNumRows = new System.Windows.Forms.NumericUpDown();
            this.udOverlayX = new System.Windows.Forms.NumericUpDown();
            this.labelCurrentFont = new System.Windows.Forms.Label();
            this.labelFont = new System.Windows.Forms.Label();
            this.labelColumnSize = new System.Windows.Forms.Label();
            this.labelNumRows = new System.Windows.Forms.Label();
            this.labelOverlayOrientation = new System.Windows.Forms.Label();
            this.groupBoxTracker = new System.Windows.Forms.GroupBox();
            this.labelSlash = new System.Windows.Forms.Label();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonRewind = new System.Windows.Forms.Button();
            this.labelEndPos = new System.Windows.Forms.Label();
            this.labelCurrPos = new System.Windows.Forms.Label();
            this.labelLoadedTimeline = new System.Windows.Forms.Label();
            this.labelLoadedTimelineLabel = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelOpacity = new System.Windows.Forms.Label();
            this.trackBarOpacity = new System.Windows.Forms.TrackBar();
            this.labelCurrOpacity = new System.Windows.Forms.Label();
            this.groupBoxEnvironment.SuspendLayout();
            this.groupBoxTimelines.SuspendLayout();
            this.groupBoxOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udBarWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTextWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayX)).BeginInit();
            this.groupBoxTracker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
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
            this.groupBoxEnvironment.Size = new System.Drawing.Size(470, 100);
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
            this.groupBoxTimelines.Location = new System.Drawing.Point(15, 139);
            this.groupBoxTimelines.Name = "groupBoxTimelines";
            this.groupBoxTimelines.Size = new System.Drawing.Size(470, 126);
            this.groupBoxTimelines.TabIndex = 4;
            this.groupBoxTimelines.TabStop = false;
            this.groupBoxTimelines.Text = "Timelines";
            // 
            // buttonRefreshList
            // 
            this.buttonRefreshList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshList.Location = new System.Drawing.Point(375, 89);
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
            this.listTimelines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTimelines.FormattingEnabled = true;
            this.listTimelines.Location = new System.Drawing.Point(12, 20);
            this.listTimelines.Name = "listTimelines";
            this.listTimelines.Size = new System.Drawing.Size(355, 95);
            this.listTimelines.TabIndex = 0;
            this.listTimelines.DoubleClick += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBoxOverlay
            // 
            this.groupBoxOverlay.Controls.Add(this.trackBarOpacity);
            this.groupBoxOverlay.Controls.Add(this.checkBoxShowOverlayButtons);
            this.groupBoxOverlay.Controls.Add(this.checkBoxMoveOverlayByDrag);
            this.groupBoxOverlay.Controls.Add(this.labelOverlayY);
            this.groupBoxOverlay.Controls.Add(this.buttonFontSelect);
            this.groupBoxOverlay.Controls.Add(this.labelBar);
            this.groupBoxOverlay.Controls.Add(this.labelTextWidth);
            this.groupBoxOverlay.Controls.Add(this.labelOverlayX);
            this.groupBoxOverlay.Controls.Add(this.udOverlayY);
            this.groupBoxOverlay.Controls.Add(this.udBarWidth);
            this.groupBoxOverlay.Controls.Add(this.udTextWidth);
            this.groupBoxOverlay.Controls.Add(this.udNumRows);
            this.groupBoxOverlay.Controls.Add(this.udOverlayX);
            this.groupBoxOverlay.Controls.Add(this.labelCurrentFont);
            this.groupBoxOverlay.Controls.Add(this.labelFont);
            this.groupBoxOverlay.Controls.Add(this.labelColumnSize);
            this.groupBoxOverlay.Controls.Add(this.labelCurrOpacity);
            this.groupBoxOverlay.Controls.Add(this.labelOpacity);
            this.groupBoxOverlay.Controls.Add(this.labelNumRows);
            this.groupBoxOverlay.Controls.Add(this.labelOverlayOrientation);
            this.groupBoxOverlay.Location = new System.Drawing.Point(16, 377);
            this.groupBoxOverlay.Name = "groupBoxOverlay";
            this.groupBoxOverlay.Size = new System.Drawing.Size(469, 243);
            this.groupBoxOverlay.TabIndex = 5;
            this.groupBoxOverlay.TabStop = false;
            this.groupBoxOverlay.Text = "Overlay";
            // 
            // checkBoxShowOverlayButtons
            // 
            this.checkBoxShowOverlayButtons.AutoSize = true;
            this.checkBoxShowOverlayButtons.Checked = true;
            this.checkBoxShowOverlayButtons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOverlayButtons.Location = new System.Drawing.Point(235, 209);
            this.checkBoxShowOverlayButtons.Name = "checkBoxShowOverlayButtons";
            this.checkBoxShowOverlayButtons.Size = new System.Drawing.Size(150, 17);
            this.checkBoxShowOverlayButtons.TabIndex = 3;
            this.checkBoxShowOverlayButtons.Text = "Show mini button controls.";
            this.checkBoxShowOverlayButtons.UseVisualStyleBackColor = true;
            this.checkBoxShowOverlayButtons.CheckedChanged += new System.EventHandler(this.checkBoxShowOverlayButtons_CheckedChanged);
            // 
            // checkBoxMoveOverlayByDrag
            // 
            this.checkBoxMoveOverlayByDrag.AutoSize = true;
            this.checkBoxMoveOverlayByDrag.Checked = true;
            this.checkBoxMoveOverlayByDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMoveOverlayByDrag.Location = new System.Drawing.Point(11, 209);
            this.checkBoxMoveOverlayByDrag.Name = "checkBoxMoveOverlayByDrag";
            this.checkBoxMoveOverlayByDrag.Size = new System.Drawing.Size(218, 17);
            this.checkBoxMoveOverlayByDrag.TabIndex = 3;
            this.checkBoxMoveOverlayByDrag.Text = "Move by drag && right doubleclick to hide.";
            this.checkBoxMoveOverlayByDrag.UseVisualStyleBackColor = true;
            this.checkBoxMoveOverlayByDrag.CheckedChanged += new System.EventHandler(this.checkBoxMoveOverlayByDrag_CheckedChanged);
            // 
            // labelOverlayY
            // 
            this.labelOverlayY.AutoSize = true;
            this.labelOverlayY.Location = new System.Drawing.Point(251, 25);
            this.labelOverlayY.Name = "labelOverlayY";
            this.labelOverlayY.Size = new System.Drawing.Size(17, 13);
            this.labelOverlayY.TabIndex = 2;
            this.labelOverlayY.Text = "Y:";
            // 
            // buttonFontSelect
            // 
            this.buttonFontSelect.Location = new System.Drawing.Point(375, 173);
            this.buttonFontSelect.Name = "buttonFontSelect";
            this.buttonFontSelect.Size = new System.Drawing.Size(85, 22);
            this.buttonFontSelect.TabIndex = 0;
            this.buttonFontSelect.Text = "Select Font";
            this.buttonFontSelect.UseVisualStyleBackColor = true;
            this.buttonFontSelect.Click += new System.EventHandler(this.buttonFontSelect_Click);
            // 
            // labelBar
            // 
            this.labelBar.AutoSize = true;
            this.labelBar.Location = new System.Drawing.Point(251, 89);
            this.labelBar.Name = "labelBar";
            this.labelBar.Size = new System.Drawing.Size(26, 13);
            this.labelBar.TabIndex = 2;
            this.labelBar.Text = "Bar:";
            this.labelBar.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTextWidth
            // 
            this.labelTextWidth.AutoSize = true;
            this.labelTextWidth.Location = new System.Drawing.Point(122, 89);
            this.labelTextWidth.Name = "labelTextWidth";
            this.labelTextWidth.Size = new System.Drawing.Size(31, 13);
            this.labelTextWidth.TabIndex = 2;
            this.labelTextWidth.Text = "Text:";
            this.labelTextWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // udOverlayY
            // 
            this.udOverlayY.Location = new System.Drawing.Point(286, 23);
            this.udOverlayY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udOverlayY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
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
            // udBarWidth
            // 
            this.udBarWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udBarWidth.Location = new System.Drawing.Point(286, 87);
            this.udBarWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udBarWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udBarWidth.Name = "udBarWidth";
            this.udBarWidth.Size = new System.Drawing.Size(80, 20);
            this.udBarWidth.TabIndex = 1;
            this.udBarWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udBarWidth.ValueChanged += new System.EventHandler(this.udBarWidth_ValueChanged);
            // 
            // udTextWidth
            // 
            this.udTextWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udTextWidth.Location = new System.Drawing.Point(159, 87);
            this.udTextWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udTextWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udTextWidth.Name = "udTextWidth";
            this.udTextWidth.Size = new System.Drawing.Size(80, 20);
            this.udTextWidth.TabIndex = 1;
            this.udTextWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udTextWidth.ValueChanged += new System.EventHandler(this.udTextWidth_ValueChanged);
            // 
            // udNumRows
            // 
            this.udNumRows.Location = new System.Drawing.Point(159, 53);
            this.udNumRows.Name = "udNumRows";
            this.udNumRows.Size = new System.Drawing.Size(40, 20);
            this.udNumRows.TabIndex = 1;
            this.udNumRows.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udNumRows.ValueChanged += new System.EventHandler(this.udNumRows_ValueChanged);
            // 
            // udOverlayX
            // 
            this.udOverlayX.Location = new System.Drawing.Point(159, 23);
            this.udOverlayX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udOverlayX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
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
            // labelCurrentFont
            // 
            this.labelCurrentFont.AutoSize = true;
            this.labelCurrentFont.Location = new System.Drawing.Point(45, 178);
            this.labelCurrentFont.Name = "labelCurrentFont";
            this.labelCurrentFont.Size = new System.Drawing.Size(80, 13);
            this.labelCurrentFont.TabIndex = 0;
            this.labelCurrentFont.Text = "CurrentFontInfo";
            // 
            // labelFont
            // 
            this.labelFont.AutoSize = true;
            this.labelFont.Location = new System.Drawing.Point(8, 178);
            this.labelFont.Name = "labelFont";
            this.labelFont.Size = new System.Drawing.Size(31, 13);
            this.labelFont.TabIndex = 0;
            this.labelFont.Text = "Font:";
            // 
            // labelColumnSize
            // 
            this.labelColumnSize.AutoSize = true;
            this.labelColumnSize.Location = new System.Drawing.Point(8, 89);
            this.labelColumnSize.Name = "labelColumnSize";
            this.labelColumnSize.Size = new System.Drawing.Size(73, 13);
            this.labelColumnSize.TabIndex = 0;
            this.labelColumnSize.Text = "Column Width";
            // 
            // labelNumRows
            // 
            this.labelNumRows.AutoSize = true;
            this.labelNumRows.Location = new System.Drawing.Point(8, 55);
            this.labelNumRows.Name = "labelNumRows";
            this.labelNumRows.Size = new System.Drawing.Size(131, 13);
            this.labelNumRows.TabIndex = 0;
            this.labelNumRows.Text = "Number of rows to display:";
            // 
            // labelOverlayOrientation
            // 
            this.labelOverlayOrientation.AutoSize = true;
            this.labelOverlayOrientation.Location = new System.Drawing.Point(8, 25);
            this.labelOverlayOrientation.Name = "labelOverlayOrientation";
            this.labelOverlayOrientation.Size = new System.Drawing.Size(97, 13);
            this.labelOverlayOrientation.TabIndex = 0;
            this.labelOverlayOrientation.Text = "Overlay Orientation";
            // 
            // groupBoxTracker
            // 
            this.groupBoxTracker.Controls.Add(this.labelSlash);
            this.groupBoxTracker.Controls.Add(this.buttonPause);
            this.groupBoxTracker.Controls.Add(this.buttonPlay);
            this.groupBoxTracker.Controls.Add(this.buttonRewind);
            this.groupBoxTracker.Controls.Add(this.labelEndPos);
            this.groupBoxTracker.Controls.Add(this.labelCurrPos);
            this.groupBoxTracker.Controls.Add(this.labelLoadedTimeline);
            this.groupBoxTracker.Controls.Add(this.labelLoadedTimelineLabel);
            this.groupBoxTracker.Controls.Add(this.trackBar);
            this.groupBoxTracker.Location = new System.Drawing.Point(15, 13);
            this.groupBoxTracker.Name = "groupBoxTracker";
            this.groupBoxTracker.Size = new System.Drawing.Size(470, 120);
            this.groupBoxTracker.TabIndex = 6;
            this.groupBoxTracker.TabStop = false;
            this.groupBoxTracker.Text = "Tracker";
            // 
            // labelSlash
            // 
            this.labelSlash.AutoSize = true;
            this.labelSlash.BackColor = System.Drawing.Color.Transparent;
            this.labelSlash.Location = new System.Drawing.Point(423, 39);
            this.labelSlash.Name = "labelSlash";
            this.labelSlash.Size = new System.Drawing.Size(12, 13);
            this.labelSlash.TabIndex = 4;
            this.labelSlash.Text = "/";
            // 
            // buttonPause
            // 
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new System.Drawing.Point(110, 83);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(57, 26);
            this.buttonPause.TabIndex = 5;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(166, 83);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(57, 26);
            this.buttonPlay.TabIndex = 5;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonRewind
            // 
            this.buttonRewind.Location = new System.Drawing.Point(10, 83);
            this.buttonRewind.Name = "buttonRewind";
            this.buttonRewind.Size = new System.Drawing.Size(87, 26);
            this.buttonRewind.TabIndex = 5;
            this.buttonRewind.Text = "<< Rewind";
            this.buttonRewind.UseVisualStyleBackColor = true;
            this.buttonRewind.Click += new System.EventHandler(this.buttonRewind_Click);
            // 
            // labelEndPos
            // 
            this.labelEndPos.AutoSize = true;
            this.labelEndPos.Location = new System.Drawing.Point(433, 47);
            this.labelEndPos.Name = "labelEndPos";
            this.labelEndPos.Size = new System.Drawing.Size(34, 13);
            this.labelEndPos.TabIndex = 3;
            this.labelEndPos.Text = "00:00";
            this.labelEndPos.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelCurrPos
            // 
            this.labelCurrPos.AutoSize = true;
            this.labelCurrPos.Location = new System.Drawing.Point(392, 30);
            this.labelCurrPos.Name = "labelCurrPos";
            this.labelCurrPos.Size = new System.Drawing.Size(34, 13);
            this.labelCurrPos.TabIndex = 3;
            this.labelCurrPos.Text = "00:00";
            // 
            // labelLoadedTimeline
            // 
            this.labelLoadedTimeline.AutoSize = true;
            this.labelLoadedTimeline.Location = new System.Drawing.Point(103, 16);
            this.labelLoadedTimeline.Name = "labelLoadedTimeline";
            this.labelLoadedTimeline.Size = new System.Drawing.Size(109, 13);
            this.labelLoadedTimeline.TabIndex = 2;
            this.labelLoadedTimeline.Text = "-- No active timeline --";
            // 
            // labelLoadedTimelineLabel
            // 
            this.labelLoadedTimelineLabel.AutoSize = true;
            this.labelLoadedTimelineLabel.Location = new System.Drawing.Point(9, 16);
            this.labelLoadedTimelineLabel.Name = "labelLoadedTimelineLabel";
            this.labelLoadedTimelineLabel.Size = new System.Drawing.Size(88, 13);
            this.labelLoadedTimelineLabel.TabIndex = 1;
            this.labelLoadedTimelineLabel.Text = "Loaded Timeline:";
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(6, 39);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(388, 45);
            this.trackBar.TabIndex = 0;
            this.trackBar.TickFrequency = 30;
            this.trackBar.Scroll += new System.EventHandler(this.trackbar_Scroll);
            // 
            // labelOpacity
            // 
            this.labelOpacity.AutoSize = true;
            this.labelOpacity.Location = new System.Drawing.Point(8, 127);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(46, 13);
            this.labelOpacity.TabIndex = 0;
            this.labelOpacity.Text = "Opacity:";
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Location = new System.Drawing.Point(159, 122);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Minimum = 1;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(234, 45);
            this.trackBarOpacity.TabIndex = 4;
            this.trackBarOpacity.TickFrequency = 10;
            this.trackBarOpacity.Value = 1;
            this.trackBarOpacity.Scroll += new System.EventHandler(this.trackBarOpacity_Scroll);
            // 
            // labelCurrOpacity
            // 
            this.labelCurrOpacity.AutoSize = true;
            this.labelCurrOpacity.Location = new System.Drawing.Point(399, 127);
            this.labelCurrOpacity.Name = "labelCurrOpacity";
            this.labelCurrOpacity.Size = new System.Drawing.Size(27, 13);
            this.labelCurrOpacity.TabIndex = 0;
            this.labelCurrOpacity.Text = "??%";
            // 
            // ACTTabPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTracker);
            this.Controls.Add(this.groupBoxOverlay);
            this.Controls.Add(this.groupBoxTimelines);
            this.Controls.Add(this.groupBoxEnvironment);
            this.Name = "ACTTabPageControl";
            this.Size = new System.Drawing.Size(500, 637);
            this.groupBoxEnvironment.ResumeLayout(false);
            this.groupBoxEnvironment.PerformLayout();
            this.groupBoxTimelines.ResumeLayout(false);
            this.groupBoxOverlay.ResumeLayout(false);
            this.groupBoxOverlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udBarWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTextWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOverlayX)).EndInit();
            this.groupBoxTracker.ResumeLayout(false);
            this.groupBoxTracker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
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
        private System.Windows.Forms.Label labelOverlayOrientation;
        private System.Windows.Forms.CheckBox checkBoxMoveOverlayByDrag;
        private System.Windows.Forms.GroupBox groupBoxTracker;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonRewind;
        private System.Windows.Forms.Label labelEndPos;
        private System.Windows.Forms.Label labelCurrPos;
        private System.Windows.Forms.Label labelSlash;
        private System.Windows.Forms.Label labelLoadedTimeline;
        private System.Windows.Forms.Label labelLoadedTimelineLabel;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.CheckBox checkBoxShowOverlayButtons;
        private System.Windows.Forms.Button buttonFontSelect;
        private System.Windows.Forms.Label labelCurrentFont;
        private System.Windows.Forms.Label labelFont;
        private System.Windows.Forms.Label labelColumnSize;
        private System.Windows.Forms.Label labelBar;
        private System.Windows.Forms.Label labelTextWidth;
        private System.Windows.Forms.NumericUpDown udBarWidth;
        private System.Windows.Forms.NumericUpDown udTextWidth;
        private System.Windows.Forms.TrackBar trackBarOpacity;
        private System.Windows.Forms.Label labelOpacity;
        private System.Windows.Forms.Label labelCurrOpacity;
    }
}
