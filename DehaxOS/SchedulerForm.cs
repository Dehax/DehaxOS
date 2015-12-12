using DehaxOS.Scheduler;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class SchedulerForm : Form
    {
        private DehaxOS _dehaxOS;

        private Random _random;

        public SchedulerForm(DehaxOS dehaxOS)
        {
            InitializeComponent();

            _dehaxOS = dehaxOS;

            _random = new Random((int)(DateTime.UtcNow.Ticks % int.MaxValue));
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < processesNumberUpDown.Value; i++)
            {
                int pid = _dehaxOS.NextPID;
                int cpuBurstTime = _random.Next((int)minCPUburstTimeUpDown.Value, (int)maxCPUburstTimeUpDown.Value);
                int ioBurstTime = _random.Next((int)minIOburstTimeUpDown.Value, (int)maxIOburstTimeUpDown.Value);
                ProcessPriority priority = (ProcessPriority)_random.Next(maxPriorityComboBox.SelectedIndex, minPriorityComboBox.SelectedIndex);

                _dehaxOS.AddProcess(pid, cpuBurstTime, ioBurstTime, priority);

                logTextBox.AppendText("PID = " + pid + ", CPU = " + cpuBurstTime + ", I/O = " + ioBurstTime + ", Priority = " + priority + "\n");
            }

            logTextBox.AppendText("============================================\n");

            MessageBox.Show(this, "Процессы успешно сгенерированы!", "Процессы созданы!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(this, "Произошла ошибка во время работы планировщика!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(this, "Работа планировщика успешно завершена!", "Завершено!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            startButton.Enabled = true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 14 - processesNumberUpDown.Value - 2; i++)
            {
                logTextBox.AppendText("\n");
            }

            backgroundWorker.RunWorkerAsync();

            startButton.Enabled = false;

            //while (backgroundWorker.IsBusy)
            //{
            //    Application.DoEvents();
            //}
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                _dehaxOS.StartScheduling(worker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SchedulerForm_Load(object sender, EventArgs e)
        {
            priorityComboBox.SelectedIndex = 2;
            minPriorityComboBox.SelectedIndex = 3;
            maxPriorityComboBox.SelectedIndex = 0;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string str = e.UserState as string;

            //StringBuilder sb = new StringBuilder();
            //sb.Append("PID = ");
            //sb.Append(process.PID);
            //sb.Append(", \n");

            logTextBox.AppendText(str);

            for (int i = 0; i < 8; i++)
            {
                logTextBox.AppendText("\n");
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();

            logTextBox.Clear();
            _dehaxOS.ClearScheduler();
        }

        private void killButton_Click(object sender, EventArgs e)
        {
            int pid = (int)pidUpDown.Value;
            _dehaxOS.KillProcess(pid);
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            int pid = (int)pidUpDown.Value;
            ProcessPriority priority = (ProcessPriority)priorityComboBox.SelectedIndex;
            _dehaxOS.ChangeProcessPriority(pid, priority);
        }
    }
}
