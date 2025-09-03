namespace Later.App;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        splitContainer1 = new SplitContainer();
        mouseLatencyUnitLabel = new Label();
        knownMouseLatencyLabel = new Label();
        mouseLatencyNumUpDown = new NumericUpDown();
        micLastDetectionLatencyLabel = new Label();
        micInstructionsLabel = new Label();
        inputDevicesLabel = new Label();
        inputDevicesComboBox = new ComboBox();
        clickLatencyButton = new Button();
        micAverageLatencyLabel = new Label();
        outLastDetectionLatencyLabel = new Label();
        outAverageLatencyLabel = new Label();
        startOutputButton = new Button();
        label2 = new Label();
        label1 = new Label();
        knownMicLatencyLabel = new Label();
        micLatencyNumUpDown = new NumericUpDown();
        outputDevicesCombobox = new ComboBox();
        outputDevicesLabel = new Label();
        appDescriptionLabel = new Label();
        startRecordingButton = new Button();
        volumeMeterProgressBar = new PaintableProgressBar();
        panel1 = new Panel();
        panel2 = new Panel();
        bottomPanel = new Panel();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)mouseLatencyNumUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)micLatencyNumUpDown).BeginInit();
        panel1.SuspendLayout();
        panel2.SuspendLayout();
        bottomPanel.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.BorderStyle = BorderStyle.FixedSingle;
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(5, 184);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.AutoScroll = true;
        splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
        splitContainer1.Panel1.Controls.Add(mouseLatencyUnitLabel);
        splitContainer1.Panel1.Controls.Add(knownMouseLatencyLabel);
        splitContainer1.Panel1.Controls.Add(mouseLatencyNumUpDown);
        splitContainer1.Panel1.Controls.Add(micLastDetectionLatencyLabel);
        splitContainer1.Panel1.Controls.Add(micInstructionsLabel);
        splitContainer1.Panel1.Controls.Add(inputDevicesLabel);
        splitContainer1.Panel1.Controls.Add(inputDevicesComboBox);
        splitContainer1.Panel1.Controls.Add(clickLatencyButton);
        splitContainer1.Panel1.Controls.Add(micAverageLatencyLabel);
        splitContainer1.Panel1.Padding = new Padding(5);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.AutoScroll = true;
        splitContainer1.Panel2.Controls.Add(outLastDetectionLatencyLabel);
        splitContainer1.Panel2.Controls.Add(outAverageLatencyLabel);
        splitContainer1.Panel2.Controls.Add(startOutputButton);
        splitContainer1.Panel2.Controls.Add(label2);
        splitContainer1.Panel2.Controls.Add(label1);
        splitContainer1.Panel2.Controls.Add(knownMicLatencyLabel);
        splitContainer1.Panel2.Controls.Add(micLatencyNumUpDown);
        splitContainer1.Panel2.Controls.Add(outputDevicesCombobox);
        splitContainer1.Panel2.Controls.Add(outputDevicesLabel);
        splitContainer1.Panel2.Padding = new Padding(5);
        splitContainer1.Size = new Size(916, 624);
        splitContainer1.SplitterDistance = 455;
        splitContainer1.TabIndex = 2;
        splitContainer1.TabStop = false;
        // 
        // mouseLatencyUnitLabel
        // 
        mouseLatencyUnitLabel.AutoSize = true;
        mouseLatencyUnitLabel.Font = new Font("Segoe UI", 9F);
        mouseLatencyUnitLabel.Location = new Point(84, 470);
        mouseLatencyUnitLabel.Name = "mouseLatencyUnitLabel";
        mouseLatencyUnitLabel.Size = new Size(23, 15);
        mouseLatencyUnitLabel.TabIndex = 10;
        mouseLatencyUnitLabel.Text = "ms";
        // 
        // knownMouseLatencyLabel
        // 
        knownMouseLatencyLabel.AutoSize = true;
        knownMouseLatencyLabel.Font = new Font("Segoe UI", 9F);
        knownMouseLatencyLabel.Location = new Point(8, 450);
        knownMouseLatencyLabel.Name = "knownMouseLatencyLabel";
        knownMouseLatencyLabel.Size = new Size(271, 15);
        knownMouseLatencyLabel.TabIndex = 9;
        knownMouseLatencyLabel.Text = "Known Mouse Latency (leave default if unknown):";
        // 
        // mouseLatencyNumUpDown
        // 
        mouseLatencyNumUpDown.Location = new Point(8, 468);
        mouseLatencyNumUpDown.Name = "mouseLatencyNumUpDown";
        mouseLatencyNumUpDown.Size = new Size(72, 23);
        mouseLatencyNumUpDown.TabIndex = 8;
        mouseLatencyNumUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // micLastDetectionLatencyLabel
        // 
        micLastDetectionLatencyLabel.AutoSize = true;
        micLastDetectionLatencyLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        micLastDetectionLatencyLabel.Location = new Point(8, 577);
        micLastDetectionLatencyLabel.Name = "micLastDetectionLatencyLabel";
        micLastDetectionLatencyLabel.Size = new Size(163, 15);
        micLastDetectionLatencyLabel.TabIndex = 7;
        micLastDetectionLatencyLabel.Text = "Last Detection Latency: 0ms";
        // 
        // micInstructionsLabel
        // 
        micInstructionsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        micInstructionsLabel.Location = new Point(8, 9);
        micInstructionsLabel.Name = "micInstructionsLabel";
        micInstructionsLabel.Size = new Size(439, 384);
        micInstructionsLabel.TabIndex = 6;
        micInstructionsLabel.Text = resources.GetString("micInstructionsLabel.Text");
        // 
        // inputDevicesLabel
        // 
        inputDevicesLabel.AutoSize = true;
        inputDevicesLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        inputDevicesLabel.Location = new Point(8, 393);
        inputDevicesLabel.Name = "inputDevicesLabel";
        inputDevicesLabel.Size = new Size(138, 15);
        inputDevicesLabel.TabIndex = 1;
        inputDevicesLabel.Text = "Choose an input device:";
        // 
        // inputDevicesComboBox
        // 
        inputDevicesComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        inputDevicesComboBox.DropDownHeight = 160;
        inputDevicesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        inputDevicesComboBox.FormattingEnabled = true;
        inputDevicesComboBox.IntegralHeight = false;
        inputDevicesComboBox.Location = new Point(8, 411);
        inputDevicesComboBox.Name = "inputDevicesComboBox";
        inputDevicesComboBox.Size = new Size(439, 23);
        inputDevicesComboBox.TabIndex = 0;
        // 
        // clickLatencyButton
        // 
        clickLatencyButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        clickLatencyButton.Enabled = false;
        clickLatencyButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        clickLatencyButton.Location = new Point(8, 497);
        clickLatencyButton.Name = "clickLatencyButton";
        clickLatencyButton.Size = new Size(437, 79);
        clickLatencyButton.TabIndex = 2;
        clickLatencyButton.Text = "Click me with any button!";
        clickLatencyButton.UseMnemonic = false;
        clickLatencyButton.UseVisualStyleBackColor = true;
        clickLatencyButton.MouseDown += ClickLatencyButton_MouseDown;
        // 
        // micAverageLatencyLabel
        // 
        micAverageLatencyLabel.AutoSize = true;
        micAverageLatencyLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        micAverageLatencyLabel.Location = new Point(8, 592);
        micAverageLatencyLabel.Name = "micAverageLatencyLabel";
        micAverageLatencyLabel.Size = new Size(199, 15);
        micAverageLatencyLabel.TabIndex = 4;
        micAverageLatencyLabel.Text = "Microphone Average Latency: 0ms";
        // 
        // outLastDetectionLatencyLabel
        // 
        outLastDetectionLatencyLabel.AutoSize = true;
        outLastDetectionLatencyLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        outLastDetectionLatencyLabel.Location = new Point(8, 579);
        outLastDetectionLatencyLabel.Name = "outLastDetectionLatencyLabel";
        outLastDetectionLatencyLabel.Size = new Size(163, 15);
        outLastDetectionLatencyLabel.TabIndex = 12;
        outLastDetectionLatencyLabel.Text = "Last Detection Latency: 0ms";
        // 
        // outAverageLatencyLabel
        // 
        outAverageLatencyLabel.AutoSize = true;
        outAverageLatencyLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        outAverageLatencyLabel.Location = new Point(8, 594);
        outAverageLatencyLabel.Name = "outAverageLatencyLabel";
        outAverageLatencyLabel.Size = new Size(172, 15);
        outAverageLatencyLabel.TabIndex = 11;
        outAverageLatencyLabel.Text = "Output Average Latency: 0ms";
        // 
        // startOutputButton
        // 
        startOutputButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        startOutputButton.Enabled = false;
        startOutputButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        startOutputButton.Location = new Point(8, 497);
        startOutputButton.Name = "startOutputButton";
        startOutputButton.Size = new Size(437, 79);
        startOutputButton.TabIndex = 11;
        startOutputButton.Text = "Start sampling";
        startOutputButton.UseMnemonic = false;
        startOutputButton.UseVisualStyleBackColor = true;
        startOutputButton.Click += StartOutputButton_Click;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 9F);
        label2.Location = new Point(86, 470);
        label2.Name = "label2";
        label2.Size = new Size(23, 15);
        label2.TabIndex = 13;
        label2.Text = "ms";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        label1.Location = new Point(8, 9);
        label1.Name = "label1";
        label1.Size = new Size(439, 384);
        label1.TabIndex = 8;
        label1.Text = resources.GetString("label1.Text");
        // 
        // knownMicLatencyLabel
        // 
        knownMicLatencyLabel.AutoSize = true;
        knownMicLatencyLabel.Font = new Font("Segoe UI", 9F);
        knownMicLatencyLabel.Location = new Point(8, 450);
        knownMicLatencyLabel.Name = "knownMicLatencyLabel";
        knownMicLatencyLabel.Size = new Size(315, 15);
        knownMicLatencyLabel.TabIndex = 12;
        knownMicLatencyLabel.Text = "Known Mic Latency (if unknown, use the pane on the left):";
        // 
        // micLatencyNumUpDown
        // 
        micLatencyNumUpDown.Location = new Point(8, 468);
        micLatencyNumUpDown.Name = "micLatencyNumUpDown";
        micLatencyNumUpDown.Size = new Size(72, 23);
        micLatencyNumUpDown.TabIndex = 11;
        // 
        // outputDevicesCombobox
        // 
        outputDevicesCombobox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        outputDevicesCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
        outputDevicesCombobox.FormattingEnabled = true;
        outputDevicesCombobox.Location = new Point(8, 411);
        outputDevicesCombobox.Name = "outputDevicesCombobox";
        outputDevicesCombobox.Size = new Size(439, 23);
        outputDevicesCombobox.TabIndex = 3;
        // 
        // outputDevicesLabel
        // 
        outputDevicesLabel.AutoSize = true;
        outputDevicesLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        outputDevicesLabel.Location = new Point(8, 393);
        outputDevicesLabel.Name = "outputDevicesLabel";
        outputDevicesLabel.Size = new Size(254, 15);
        outputDevicesLabel.TabIndex = 0;
        outputDevicesLabel.Text = "Choose the output device for measurement:";
        // 
        // appDescriptionLabel
        // 
        appDescriptionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        appDescriptionLabel.Location = new Point(9, 4);
        appDescriptionLabel.Name = "appDescriptionLabel";
        appDescriptionLabel.Size = new Size(898, 177);
        appDescriptionLabel.TabIndex = 5;
        appDescriptionLabel.Text = resources.GetString("appDescriptionLabel.Text");
        // 
        // startRecordingButton
        // 
        startRecordingButton.BackColor = Color.White;
        startRecordingButton.FlatStyle = FlatStyle.Flat;
        startRecordingButton.Font = new Font("Segoe UI Black", 9F);
        startRecordingButton.ForeColor = Color.Red;
        startRecordingButton.Location = new Point(3, 10);
        startRecordingButton.Name = "startRecordingButton";
        startRecordingButton.Size = new Size(294, 35);
        startRecordingButton.TabIndex = 1;
        startRecordingButton.Text = "⏺️REC";
        startRecordingButton.UseVisualStyleBackColor = false;
        startRecordingButton.Click += StartRecordingButton_Click;
        // 
        // volumeMeterProgressBar
        // 
        volumeMeterProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        volumeMeterProgressBar.Location = new Point(303, 10);
        volumeMeterProgressBar.Name = "volumeMeterProgressBar";
        volumeMeterProgressBar.Size = new Size(609, 35);
        volumeMeterProgressBar.TabIndex = 3;
        // 
        // panel1
        // 
        panel1.Controls.Add(splitContainer1);
        panel1.Controls.Add(panel2);
        panel1.Controls.Add(bottomPanel);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(0, 0);
        panel1.Margin = new Padding(0);
        panel1.Name = "panel1";
        panel1.Padding = new Padding(5);
        panel1.Size = new Size(926, 861);
        panel1.TabIndex = 3;
        // 
        // panel2
        // 
        panel2.Controls.Add(appDescriptionLabel);
        panel2.Dock = DockStyle.Top;
        panel2.Location = new Point(5, 5);
        panel2.Name = "panel2";
        panel2.Size = new Size(916, 179);
        panel2.TabIndex = 6;
        // 
        // bottomPanel
        // 
        bottomPanel.Controls.Add(volumeMeterProgressBar);
        bottomPanel.Controls.Add(startRecordingButton);
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Location = new Point(5, 808);
        bottomPanel.Name = "bottomPanel";
        bottomPanel.Size = new Size(916, 48);
        bottomPanel.TabIndex = 3;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        ClientSize = new Size(926, 861);
        Controls.Add(panel1);
        MaximizeBox = false;
        MaximumSize = new Size(1000, 1200);
        MinimumSize = new Size(650, 900);
        Name = "MainForm";
        Text = "Later";
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel1.PerformLayout();
        splitContainer1.Panel2.ResumeLayout(false);
        splitContainer1.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)mouseLatencyNumUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)micLatencyNumUpDown).EndInit();
        panel1.ResumeLayout(false);
        panel2.ResumeLayout(false);
        bottomPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private Label inputDevicesLabel;
    private ComboBox inputDevicesComboBox;
    private Button startRecordingButton;
    private Button clickLatencyButton;
    private Label micAverageLatencyLabel;
    private PaintableProgressBar volumeMeterProgressBar;
    private Panel panel1;
    private Label outputDevicesLabel;
    private ComboBox outputDevicesCombobox;
    private Panel bottomPanel;
    private Label appDescriptionLabel;
    private Panel panel2;
    private Label micInstructionsLabel;
    private Label micLastDetectionLatencyLabel;
    private Label label1;
    private Label mouseLatencyUnitLabel;
    private Label knownMouseLatencyLabel;
    private NumericUpDown mouseLatencyNumUpDown;
    private Label label2;
    private Label knownMicLatencyLabel;
    private NumericUpDown micLatencyNumUpDown;
    private Button startOutputButton;
    private Label outLastDetectionLatencyLabel;
    private Label outAverageLatencyLabel;
}
