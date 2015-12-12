namespace DehaxOS
{
    partial class SchedulerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedulerForm));
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.parametersGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.killButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.pidLabel = new System.Windows.Forms.Label();
            this.pidUpDown = new System.Windows.Forms.NumericUpDown();
            this.actionsGroupBox = new System.Windows.Forms.GroupBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.priorityGroupBox = new System.Windows.Forms.GroupBox();
            this.maxPriorityComboBox = new System.Windows.Forms.ComboBox();
            this.minPriorityComboBox = new System.Windows.Forms.ComboBox();
            this.maxPriorityLabel = new System.Windows.Forms.Label();
            this.minPriorityLabel = new System.Windows.Forms.Label();
            this.ioBurstTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.maxIOburstTimeMsLabel = new System.Windows.Forms.Label();
            this.maxIOburstTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxIOburstTimeLabel = new System.Windows.Forms.Label();
            this.minIOburstTimeMsLabel = new System.Windows.Forms.Label();
            this.minIOburstTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.minIOburstTimeLabel = new System.Windows.Forms.Label();
            this.cpuBurstTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.maxCPUburstTimeMsLabel = new System.Windows.Forms.Label();
            this.maxCPUburstTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxCPUburstTimeLabel = new System.Windows.Forms.Label();
            this.minCPUburstTimeMsLabel = new System.Windows.Forms.Label();
            this.minCPUburstTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.minCPUburstTimeLabel = new System.Windows.Forms.Label();
            this.processesNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.proccessesNumberLabel = new System.Windows.Forms.Label();
            this.logLabel = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.parametersGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pidUpDown)).BeginInit();
            this.actionsGroupBox.SuspendLayout();
            this.priorityGroupBox.SuspendLayout();
            this.ioBurstTimeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxIOburstTimeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minIOburstTimeUpDown)).BeginInit();
            this.cpuBurstTimeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxCPUburstTimeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minCPUburstTimeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processesNumberUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logTextBox.Location = new System.Drawing.Point(12, 206);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(560, 194);
            this.logTextBox.TabIndex = 0;
            // 
            // parametersGroupBox
            // 
            this.parametersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parametersGroupBox.Controls.Add(this.groupBox1);
            this.parametersGroupBox.Controls.Add(this.actionsGroupBox);
            this.parametersGroupBox.Controls.Add(this.priorityGroupBox);
            this.parametersGroupBox.Controls.Add(this.ioBurstTimeGroupBox);
            this.parametersGroupBox.Controls.Add(this.cpuBurstTimeGroupBox);
            this.parametersGroupBox.Controls.Add(this.processesNumberUpDown);
            this.parametersGroupBox.Controls.Add(this.proccessesNumberLabel);
            this.parametersGroupBox.Location = new System.Drawing.Point(13, 13);
            this.parametersGroupBox.Name = "parametersGroupBox";
            this.parametersGroupBox.Size = new System.Drawing.Size(559, 171);
            this.parametersGroupBox.TabIndex = 1;
            this.parametersGroupBox.TabStop = false;
            this.parametersGroupBox.Text = "Параметры процессов";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.priorityComboBox);
            this.groupBox1.Controls.Add(this.priorityLabel);
            this.groupBox1.Controls.Add(this.killButton);
            this.groupBox1.Controls.Add(this.applyButton);
            this.groupBox1.Controls.Add(this.pidLabel);
            this.groupBox1.Controls.Add(this.pidUpDown);
            this.groupBox1.Location = new System.Drawing.Point(6, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Изменить параметры";
            // 
            // killButton
            // 
            this.killButton.Location = new System.Drawing.Point(80, 73);
            this.killButton.Name = "killButton";
            this.killButton.Size = new System.Drawing.Size(77, 23);
            this.killButton.TabIndex = 8;
            this.killButton.Text = "Уничтожить";
            this.killButton.UseVisualStyleBackColor = true;
            this.killButton.Click += new System.EventHandler(this.killButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(6, 73);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(68, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Изменить";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // pidLabel
            // 
            this.pidLabel.AutoSize = true;
            this.pidLabel.Location = new System.Drawing.Point(6, 20);
            this.pidLabel.Name = "pidLabel";
            this.pidLabel.Size = new System.Drawing.Size(28, 13);
            this.pidLabel.TabIndex = 7;
            this.pidLabel.Text = "PID:";
            // 
            // pidUpDown
            // 
            this.pidUpDown.Location = new System.Drawing.Point(70, 18);
            this.pidUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pidUpDown.Name = "pidUpDown";
            this.pidUpDown.Size = new System.Drawing.Size(87, 20);
            this.pidUpDown.TabIndex = 6;
            this.pidUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.pidUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // actionsGroupBox
            // 
            this.actionsGroupBox.Controls.Add(this.clearButton);
            this.actionsGroupBox.Controls.Add(this.startButton);
            this.actionsGroupBox.Controls.Add(this.generateButton);
            this.actionsGroupBox.Location = new System.Drawing.Point(367, 90);
            this.actionsGroupBox.Name = "actionsGroupBox";
            this.actionsGroupBox.Size = new System.Drawing.Size(186, 68);
            this.actionsGroupBox.TabIndex = 5;
            this.actionsGroupBox.TabStop = false;
            this.actionsGroupBox.Text = "Действия";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(112, 15);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(68, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Очистить";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(6, 41);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Обработать";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(6, 15);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(100, 23);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Генерировать";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // priorityGroupBox
            // 
            this.priorityGroupBox.Controls.Add(this.maxPriorityComboBox);
            this.priorityGroupBox.Controls.Add(this.minPriorityComboBox);
            this.priorityGroupBox.Controls.Add(this.maxPriorityLabel);
            this.priorityGroupBox.Controls.Add(this.minPriorityLabel);
            this.priorityGroupBox.Location = new System.Drawing.Point(175, 90);
            this.priorityGroupBox.Name = "priorityGroupBox";
            this.priorityGroupBox.Size = new System.Drawing.Size(186, 68);
            this.priorityGroupBox.TabIndex = 4;
            this.priorityGroupBox.TabStop = false;
            this.priorityGroupBox.Text = "Приоритет";
            // 
            // maxPriorityComboBox
            // 
            this.maxPriorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxPriorityComboBox.FormattingEnabled = true;
            this.maxPriorityComboBox.Items.AddRange(new object[] {
            "системный",
            "высокий",
            "обычный",
            "низкий"});
            this.maxPriorityComboBox.Location = new System.Drawing.Point(93, 43);
            this.maxPriorityComboBox.Name = "maxPriorityComboBox";
            this.maxPriorityComboBox.Size = new System.Drawing.Size(87, 21);
            this.maxPriorityComboBox.TabIndex = 6;
            // 
            // minPriorityComboBox
            // 
            this.minPriorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minPriorityComboBox.FormattingEnabled = true;
            this.minPriorityComboBox.Items.AddRange(new object[] {
            "системный",
            "высокий",
            "обычный",
            "низкий"});
            this.minPriorityComboBox.Location = new System.Drawing.Point(93, 17);
            this.minPriorityComboBox.Name = "minPriorityComboBox";
            this.minPriorityComboBox.Size = new System.Drawing.Size(87, 21);
            this.minPriorityComboBox.TabIndex = 5;
            // 
            // maxPriorityLabel
            // 
            this.maxPriorityLabel.AutoSize = true;
            this.maxPriorityLabel.Location = new System.Drawing.Point(7, 46);
            this.maxPriorityLabel.Name = "maxPriorityLabel";
            this.maxPriorityLabel.Size = new System.Drawing.Size(88, 13);
            this.maxPriorityLabel.TabIndex = 3;
            this.maxPriorityLabel.Text = "максимальный:";
            // 
            // minPriorityLabel
            // 
            this.minPriorityLabel.AutoSize = true;
            this.minPriorityLabel.Location = new System.Drawing.Point(7, 20);
            this.minPriorityLabel.Name = "minPriorityLabel";
            this.minPriorityLabel.Size = new System.Drawing.Size(82, 13);
            this.minPriorityLabel.TabIndex = 0;
            this.minPriorityLabel.Text = "минимальный:";
            // 
            // ioBurstTimeGroupBox
            // 
            this.ioBurstTimeGroupBox.Controls.Add(this.maxIOburstTimeMsLabel);
            this.ioBurstTimeGroupBox.Controls.Add(this.maxIOburstTimeUpDown);
            this.ioBurstTimeGroupBox.Controls.Add(this.maxIOburstTimeLabel);
            this.ioBurstTimeGroupBox.Controls.Add(this.minIOburstTimeMsLabel);
            this.ioBurstTimeGroupBox.Controls.Add(this.minIOburstTimeUpDown);
            this.ioBurstTimeGroupBox.Controls.Add(this.minIOburstTimeLabel);
            this.ioBurstTimeGroupBox.Location = new System.Drawing.Point(367, 16);
            this.ioBurstTimeGroupBox.Name = "ioBurstTimeGroupBox";
            this.ioBurstTimeGroupBox.Size = new System.Drawing.Size(186, 68);
            this.ioBurstTimeGroupBox.TabIndex = 3;
            this.ioBurstTimeGroupBox.TabStop = false;
            this.ioBurstTimeGroupBox.Text = "I/O burst time";
            // 
            // maxIOburstTimeMsLabel
            // 
            this.maxIOburstTimeMsLabel.AutoSize = true;
            this.maxIOburstTimeMsLabel.Location = new System.Drawing.Point(159, 46);
            this.maxIOburstTimeMsLabel.Name = "maxIOburstTimeMsLabel";
            this.maxIOburstTimeMsLabel.Size = new System.Drawing.Size(21, 13);
            this.maxIOburstTimeMsLabel.TabIndex = 5;
            this.maxIOburstTimeMsLabel.Text = "мс";
            // 
            // maxIOburstTimeUpDown
            // 
            this.maxIOburstTimeUpDown.Location = new System.Drawing.Point(93, 42);
            this.maxIOburstTimeUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.maxIOburstTimeUpDown.Name = "maxIOburstTimeUpDown";
            this.maxIOburstTimeUpDown.Size = new System.Drawing.Size(60, 20);
            this.maxIOburstTimeUpDown.TabIndex = 4;
            this.maxIOburstTimeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maxIOburstTimeUpDown.ThousandsSeparator = true;
            this.maxIOburstTimeUpDown.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // maxIOburstTimeLabel
            // 
            this.maxIOburstTimeLabel.AutoSize = true;
            this.maxIOburstTimeLabel.Location = new System.Drawing.Point(7, 46);
            this.maxIOburstTimeLabel.Name = "maxIOburstTimeLabel";
            this.maxIOburstTimeLabel.Size = new System.Drawing.Size(86, 13);
            this.maxIOburstTimeLabel.TabIndex = 3;
            this.maxIOburstTimeLabel.Text = "максимальное:";
            // 
            // minIOburstTimeMsLabel
            // 
            this.minIOburstTimeMsLabel.AutoSize = true;
            this.minIOburstTimeMsLabel.Location = new System.Drawing.Point(159, 20);
            this.minIOburstTimeMsLabel.Name = "minIOburstTimeMsLabel";
            this.minIOburstTimeMsLabel.Size = new System.Drawing.Size(21, 13);
            this.minIOburstTimeMsLabel.TabIndex = 2;
            this.minIOburstTimeMsLabel.Text = "мс";
            // 
            // minIOburstTimeUpDown
            // 
            this.minIOburstTimeUpDown.Location = new System.Drawing.Point(93, 16);
            this.minIOburstTimeUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.minIOburstTimeUpDown.Name = "minIOburstTimeUpDown";
            this.minIOburstTimeUpDown.Size = new System.Drawing.Size(60, 20);
            this.minIOburstTimeUpDown.TabIndex = 1;
            this.minIOburstTimeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.minIOburstTimeUpDown.ThousandsSeparator = true;
            this.minIOburstTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // minIOburstTimeLabel
            // 
            this.minIOburstTimeLabel.AutoSize = true;
            this.minIOburstTimeLabel.Location = new System.Drawing.Point(7, 20);
            this.minIOburstTimeLabel.Name = "minIOburstTimeLabel";
            this.minIOburstTimeLabel.Size = new System.Drawing.Size(80, 13);
            this.minIOburstTimeLabel.TabIndex = 0;
            this.minIOburstTimeLabel.Text = "минимальное:";
            // 
            // cpuBurstTimeGroupBox
            // 
            this.cpuBurstTimeGroupBox.Controls.Add(this.maxCPUburstTimeMsLabel);
            this.cpuBurstTimeGroupBox.Controls.Add(this.maxCPUburstTimeUpDown);
            this.cpuBurstTimeGroupBox.Controls.Add(this.maxCPUburstTimeLabel);
            this.cpuBurstTimeGroupBox.Controls.Add(this.minCPUburstTimeMsLabel);
            this.cpuBurstTimeGroupBox.Controls.Add(this.minCPUburstTimeUpDown);
            this.cpuBurstTimeGroupBox.Controls.Add(this.minCPUburstTimeLabel);
            this.cpuBurstTimeGroupBox.Location = new System.Drawing.Point(175, 16);
            this.cpuBurstTimeGroupBox.Name = "cpuBurstTimeGroupBox";
            this.cpuBurstTimeGroupBox.Size = new System.Drawing.Size(186, 68);
            this.cpuBurstTimeGroupBox.TabIndex = 2;
            this.cpuBurstTimeGroupBox.TabStop = false;
            this.cpuBurstTimeGroupBox.Text = "CPU burst time";
            // 
            // maxCPUburstTimeMsLabel
            // 
            this.maxCPUburstTimeMsLabel.AutoSize = true;
            this.maxCPUburstTimeMsLabel.Location = new System.Drawing.Point(159, 46);
            this.maxCPUburstTimeMsLabel.Name = "maxCPUburstTimeMsLabel";
            this.maxCPUburstTimeMsLabel.Size = new System.Drawing.Size(21, 13);
            this.maxCPUburstTimeMsLabel.TabIndex = 5;
            this.maxCPUburstTimeMsLabel.Text = "мс";
            // 
            // maxCPUburstTimeUpDown
            // 
            this.maxCPUburstTimeUpDown.Location = new System.Drawing.Point(93, 42);
            this.maxCPUburstTimeUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.maxCPUburstTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxCPUburstTimeUpDown.Name = "maxCPUburstTimeUpDown";
            this.maxCPUburstTimeUpDown.Size = new System.Drawing.Size(60, 20);
            this.maxCPUburstTimeUpDown.TabIndex = 4;
            this.maxCPUburstTimeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maxCPUburstTimeUpDown.ThousandsSeparator = true;
            this.maxCPUburstTimeUpDown.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // maxCPUburstTimeLabel
            // 
            this.maxCPUburstTimeLabel.AutoSize = true;
            this.maxCPUburstTimeLabel.Location = new System.Drawing.Point(7, 46);
            this.maxCPUburstTimeLabel.Name = "maxCPUburstTimeLabel";
            this.maxCPUburstTimeLabel.Size = new System.Drawing.Size(86, 13);
            this.maxCPUburstTimeLabel.TabIndex = 3;
            this.maxCPUburstTimeLabel.Text = "максимальное:";
            // 
            // minCPUburstTimeMsLabel
            // 
            this.minCPUburstTimeMsLabel.AutoSize = true;
            this.minCPUburstTimeMsLabel.Location = new System.Drawing.Point(159, 20);
            this.minCPUburstTimeMsLabel.Name = "minCPUburstTimeMsLabel";
            this.minCPUburstTimeMsLabel.Size = new System.Drawing.Size(21, 13);
            this.minCPUburstTimeMsLabel.TabIndex = 2;
            this.minCPUburstTimeMsLabel.Text = "мс";
            // 
            // minCPUburstTimeUpDown
            // 
            this.minCPUburstTimeUpDown.Location = new System.Drawing.Point(93, 16);
            this.minCPUburstTimeUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.minCPUburstTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minCPUburstTimeUpDown.Name = "minCPUburstTimeUpDown";
            this.minCPUburstTimeUpDown.Size = new System.Drawing.Size(60, 20);
            this.minCPUburstTimeUpDown.TabIndex = 1;
            this.minCPUburstTimeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.minCPUburstTimeUpDown.ThousandsSeparator = true;
            this.minCPUburstTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // minCPUburstTimeLabel
            // 
            this.minCPUburstTimeLabel.AutoSize = true;
            this.minCPUburstTimeLabel.Location = new System.Drawing.Point(7, 20);
            this.minCPUburstTimeLabel.Name = "minCPUburstTimeLabel";
            this.minCPUburstTimeLabel.Size = new System.Drawing.Size(80, 13);
            this.minCPUburstTimeLabel.TabIndex = 0;
            this.minCPUburstTimeLabel.Text = "минимальное:";
            // 
            // processesNumberUpDown
            // 
            this.processesNumberUpDown.Location = new System.Drawing.Point(9, 32);
            this.processesNumberUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.processesNumberUpDown.Name = "processesNumberUpDown";
            this.processesNumberUpDown.Size = new System.Drawing.Size(160, 20);
            this.processesNumberUpDown.TabIndex = 1;
            this.processesNumberUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.processesNumberUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // proccessesNumberLabel
            // 
            this.proccessesNumberLabel.AutoSize = true;
            this.proccessesNumberLabel.Location = new System.Drawing.Point(6, 16);
            this.proccessesNumberLabel.Name = "proccessesNumberLabel";
            this.proccessesNumberLabel.Size = new System.Drawing.Size(123, 13);
            this.proccessesNumberLabel.TabIndex = 0;
            this.proccessesNumberLabel.Text = "Количество процессов";
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new System.Drawing.Point(13, 187);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(128, 13);
            this.logLabel.TabIndex = 2;
            this.logLabel.Text = "Журнал планировщика:";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // priorityLabel
            // 
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(6, 46);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(64, 13);
            this.priorityLabel.TabIndex = 10;
            this.priorityLabel.Text = "Приоритет:";
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.FormattingEnabled = true;
            this.priorityComboBox.Items.AddRange(new object[] {
            "системный",
            "высокий",
            "обычный",
            "низкий"});
            this.priorityComboBox.Location = new System.Drawing.Point(70, 43);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(87, 21);
            this.priorityComboBox.TabIndex = 7;
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.logLabel);
            this.Controls.Add(this.parametersGroupBox);
            this.Controls.Add(this.logTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SchedulerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Планировщик";
            this.Load += new System.EventHandler(this.SchedulerForm_Load);
            this.parametersGroupBox.ResumeLayout(false);
            this.parametersGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pidUpDown)).EndInit();
            this.actionsGroupBox.ResumeLayout(false);
            this.priorityGroupBox.ResumeLayout(false);
            this.priorityGroupBox.PerformLayout();
            this.ioBurstTimeGroupBox.ResumeLayout(false);
            this.ioBurstTimeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxIOburstTimeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minIOburstTimeUpDown)).EndInit();
            this.cpuBurstTimeGroupBox.ResumeLayout(false);
            this.cpuBurstTimeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxCPUburstTimeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minCPUburstTimeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processesNumberUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.GroupBox parametersGroupBox;
        private System.Windows.Forms.NumericUpDown processesNumberUpDown;
        private System.Windows.Forms.Label proccessesNumberLabel;
        private System.Windows.Forms.Label logLabel;
        private System.Windows.Forms.GroupBox cpuBurstTimeGroupBox;
        private System.Windows.Forms.Label maxCPUburstTimeMsLabel;
        private System.Windows.Forms.NumericUpDown maxCPUburstTimeUpDown;
        private System.Windows.Forms.Label maxCPUburstTimeLabel;
        private System.Windows.Forms.Label minCPUburstTimeMsLabel;
        private System.Windows.Forms.NumericUpDown minCPUburstTimeUpDown;
        private System.Windows.Forms.Label minCPUburstTimeLabel;
        private System.Windows.Forms.GroupBox actionsGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.GroupBox priorityGroupBox;
        private System.Windows.Forms.Label maxPriorityLabel;
        private System.Windows.Forms.Label minPriorityLabel;
        private System.Windows.Forms.GroupBox ioBurstTimeGroupBox;
        private System.Windows.Forms.Label maxIOburstTimeMsLabel;
        private System.Windows.Forms.NumericUpDown maxIOburstTimeUpDown;
        private System.Windows.Forms.Label maxIOburstTimeLabel;
        private System.Windows.Forms.Label minIOburstTimeMsLabel;
        private System.Windows.Forms.NumericUpDown minIOburstTimeUpDown;
        private System.Windows.Forms.Label minIOburstTimeLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ComboBox maxPriorityComboBox;
        private System.Windows.Forms.ComboBox minPriorityComboBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button killButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label pidLabel;
        private System.Windows.Forms.NumericUpDown pidUpDown;
        private System.Windows.Forms.ComboBox priorityComboBox;
        private System.Windows.Forms.Label priorityLabel;
    }
}