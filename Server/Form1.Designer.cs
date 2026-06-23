namespace Server
{
    partial class Form1
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
            groupBox1 = new GroupBox();
            btnStartQuiz = new Button();
            btnToggle = new Button();
            txtPort = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtStatus = new TextBox();
            groupBox2 = new GroupBox();
            rtbLog = new RichTextBox();
            groupBox3 = new GroupBox();
            dgvStudents = new DataGridView();
            STT = new DataGridViewTextBoxColumn();
            FullName = new DataGridViewTextBoxColumn();
            IDClient = new DataGridViewTextBoxColumn();
            IPAddress = new DataGridViewTextBoxColumn();
            Grade = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Times = new DataGridViewTextBoxColumn();
            groupBox4 = new GroupBox();
            dgvQuestions = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Question = new DataGridViewTextBoxColumn();
            Answer = new DataGridViewTextBoxColumn();
            btnClearLog = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnStartQuiz);
            groupBox1.Controls.Add(btnToggle);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtStatus);
            groupBox1.Location = new Point(21, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(737, 128);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Server";
            // 
            // btnStartQuiz
            // 
            btnStartQuiz.BackColor = Color.FromArgb(0, 0, 192);
            btnStartQuiz.ForeColor = Color.White;
            btnStartQuiz.Location = new Point(523, 29);
            btnStartQuiz.Name = "btnStartQuiz";
            btnStartQuiz.Size = new Size(126, 73);
            btnStartQuiz.TabIndex = 4;
            btnStartQuiz.Text = "Start ";
            btnStartQuiz.UseVisualStyleBackColor = false;
            // 
            // btnToggle
            // 
            btnToggle.BackColor = Color.FromArgb(0, 192, 0);
            btnToggle.ForeColor = Color.White;
            btnToggle.Location = new Point(317, 30);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new Size(126, 73);
            btnToggle.TabIndex = 3;
            btnToggle.Text = "Start/Stop Server";
            btnToggle.UseVisualStyleBackColor = false;
            btnToggle.CommandCanExecuteChanged += btnToggle_CommandCanExecuteChanged;
            btnToggle.Click += btnToggle_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(99, 76);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 1;
            txtPort.Text = "9999";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 76);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 2;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 29);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 1;
            label1.Text = "Trạng thái:";
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(99, 26);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(125, 27);
            txtStatus.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rtbLog);
            groupBox2.Location = new Point(21, 165);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(737, 401);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log hệ thống";
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.Black;
            rtbLog.Dock = DockStyle.Fill;
            rtbLog.Location = new Point(3, 23);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(731, 375);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dgvStudents);
            groupBox3.Location = new Point(789, 37);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(933, 305);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Danh sách sinh viên";
            // 
            // dgvStudents
            // 
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Columns.AddRange(new DataGridViewColumn[] { STT, FullName, IDClient, IPAddress, Grade, Status, Times });
            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.Location = new Point(3, 23);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.ReadOnly = true;
            dgvStudents.RowHeadersWidth = 51;
            dgvStudents.Size = new Size(927, 279);
            dgvStudents.TabIndex = 0;
            // 
            // STT
            // 
            STT.HeaderText = "STT";
            STT.MinimumWidth = 6;
            STT.Name = "STT";
            STT.ReadOnly = true;
            STT.Width = 125;
            // 
            // FullName
            // 
            FullName.HeaderText = "FullName";
            FullName.MinimumWidth = 6;
            FullName.Name = "FullName";
            FullName.ReadOnly = true;
            FullName.Width = 125;
            // 
            // IDClient
            // 
            IDClient.HeaderText = "ID (Client)";
            IDClient.MinimumWidth = 6;
            IDClient.Name = "IDClient";
            IDClient.ReadOnly = true;
            IDClient.Width = 125;
            // 
            // IPAddress
            // 
            IPAddress.HeaderText = "IP Address";
            IPAddress.MinimumWidth = 6;
            IPAddress.Name = "IPAddress";
            IPAddress.ReadOnly = true;
            IPAddress.Width = 125;
            // 
            // Grade
            // 
            Grade.HeaderText = "Grade";
            Grade.MinimumWidth = 6;
            Grade.Name = "Grade";
            Grade.ReadOnly = true;
            Grade.Width = 125;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Width = 125;
            // 
            // Times
            // 
            Times.HeaderText = "Times";
            Times.MinimumWidth = 6;
            Times.Name = "Times";
            Times.ReadOnly = true;
            Times.Width = 125;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dgvQuestions);
            groupBox4.Location = new Point(790, 359);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(929, 204);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Danh sách câu hỏi";
            // 
            // dgvQuestions
            // 
            dgvQuestions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuestions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuestions.Columns.AddRange(new DataGridViewColumn[] { ID, Question, Answer });
            dgvQuestions.Dock = DockStyle.Fill;
            dgvQuestions.Location = new Point(3, 23);
            dgvQuestions.Name = "dgvQuestions";
            dgvQuestions.ReadOnly = true;
            dgvQuestions.RowHeadersWidth = 51;
            dgvQuestions.Size = new Size(923, 178);
            dgvQuestions.TabIndex = 0;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.MinimumWidth = 6;
            ID.Name = "ID";
            ID.ReadOnly = true;
            // 
            // Question
            // 
            Question.HeaderText = "Question";
            Question.MinimumWidth = 6;
            Question.Name = "Question";
            Question.ReadOnly = true;
            // 
            // Answer
            // 
            Answer.HeaderText = "Answer";
            Answer.MinimumWidth = 6;
            Answer.Name = "Answer";
            Answer.ReadOnly = true;
            // 
            // btnClearLog
            // 
            btnClearLog.Location = new Point(24, 572);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(92, 46);
            btnClearLog.TabIndex = 4;
            btnClearLog.Text = "Xóa Log";
            btnClearLog.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1734, 659);
            Controls.Add(btnClearLog);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnStartQuiz;
        private Button btnToggle;
        private TextBox txtPort;
        private Label label2;
        private Label label1;
        private TextBox txtStatus;
        private GroupBox groupBox2;
        private RichTextBox rtbLog;
        private GroupBox groupBox3;
        private DataGridView dgvStudents;
        private DataGridViewTextBoxColumn STT;
        private DataGridViewTextBoxColumn FullName;
        private DataGridViewTextBoxColumn IDClient;
        private DataGridViewTextBoxColumn IPAddress;
        private DataGridViewTextBoxColumn Grade;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Times;
        private GroupBox groupBox4;
        private DataGridView dgvQuestions;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Question;
        private DataGridViewTextBoxColumn Answer;
        private Button btnClearLog;
    }
}
