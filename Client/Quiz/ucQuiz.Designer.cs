namespace Quiz
{
    partial class ucQuiz
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStudentInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.rdoD = new System.Windows.Forms.RadioButton();
            this.rdoC = new System.Windows.Forms.RadioButton();
            this.rdoB = new System.Windows.Forms.RadioButton();
            this.rdoA = new System.Windows.Forms.RadioButton();
            this.lblQuestionText = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.flpQuestions = new System.Windows.Forms.FlowLayoutPanel();
            this.quizTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTimer);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtStudentInfo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1049, 156);
            this.panel1.TabIndex = 0;
            // 
            // txtTimer
            // 
            this.txtTimer.ForeColor = System.Drawing.Color.Red;
            this.txtTimer.Location = new System.Drawing.Point(188, 89);
            this.txtTimer.Multiline = true;
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.ReadOnly = true;
            this.txtTimer.Size = new System.Drawing.Size(127, 38);
            this.txtTimer.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(14, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time: ";
            // 
            // txtStudentInfo
            // 
            this.txtStudentInfo.Location = new System.Drawing.Point(188, 22);
            this.txtStudentInfo.Multiline = true;
            this.txtStudentInfo.Name = "txtStudentInfo";
            this.txtStudentInfo.ReadOnly = true;
            this.txtStudentInfo.Size = new System.Drawing.Size(281, 38);
            this.txtStudentInfo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(658, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 76);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quiz";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblName.Location = new System.Drawing.Point(14, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(168, 38);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Họ và tên:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.rdoD);
            this.panel2.Controls.Add(this.rdoC);
            this.panel2.Controls.Add(this.rdoB);
            this.panel2.Controls.Add(this.rdoA);
            this.panel2.Controls.Add(this.lblQuestionText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 156);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(738, 615);
            this.panel2.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(588, 542);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(122, 38);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // rdoD
            // 
            this.rdoD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rdoD.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.rdoD.Location = new System.Drawing.Point(20, 457);
            this.rdoD.Name = "rdoD";
            this.rdoD.Size = new System.Drawing.Size(691, 59);
            this.rdoD.TabIndex = 4;
            this.rdoD.TabStop = true;
            this.rdoD.Text = "D";
            this.rdoD.UseVisualStyleBackColor = false;
            // 
            // rdoC
            // 
            this.rdoC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rdoC.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.rdoC.Location = new System.Drawing.Point(20, 367);
            this.rdoC.Name = "rdoC";
            this.rdoC.Size = new System.Drawing.Size(690, 59);
            this.rdoC.TabIndex = 3;
            this.rdoC.TabStop = true;
            this.rdoC.Text = "C";
            this.rdoC.UseVisualStyleBackColor = false;
            // 
            // rdoB
            // 
            this.rdoB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rdoB.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.rdoB.Location = new System.Drawing.Point(20, 279);
            this.rdoB.Name = "rdoB";
            this.rdoB.Size = new System.Drawing.Size(690, 59);
            this.rdoB.TabIndex = 2;
            this.rdoB.TabStop = true;
            this.rdoB.Text = "B";
            this.rdoB.UseVisualStyleBackColor = false;
            // 
            // rdoA
            // 
            this.rdoA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rdoA.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.rdoA.Location = new System.Drawing.Point(21, 188);
            this.rdoA.Name = "rdoA";
            this.rdoA.Size = new System.Drawing.Size(690, 59);
            this.rdoA.TabIndex = 1;
            this.rdoA.TabStop = true;
            this.rdoA.Text = "A";
            this.rdoA.UseVisualStyleBackColor = false;
            // 
            // lblQuestionText
            // 
            this.lblQuestionText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblQuestionText.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblQuestionText.Location = new System.Drawing.Point(16, 22);
            this.lblQuestionText.Name = "lblQuestionText";
            this.lblQuestionText.Size = new System.Drawing.Size(695, 145);
            this.lblQuestionText.TabIndex = 0;
            this.lblQuestionText.Text = "...";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnFinish);
            this.panel3.Controls.Add(this.flpQuestions);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(753, 156);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(296, 615);
            this.panel3.TabIndex = 2;
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnFinish.Location = new System.Drawing.Point(83, 462);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(131, 54);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "Finish ";
            this.btnFinish.UseVisualStyleBackColor = true;
            // 
            // flpQuestions
            // 
            this.flpQuestions.Location = new System.Drawing.Point(17, 19);
            this.flpQuestions.Name = "flpQuestions";
            this.flpQuestions.Size = new System.Drawing.Size(268, 351);
            this.flpQuestions.TabIndex = 0;
            // 
            // quizTimer
            // 
            this.quizTimer.Interval = 1000;
            // 
            // ucQuiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ucQuiz";
            this.Size = new System.Drawing.Size(1049, 771);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStudentInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtTimer;
        private System.Windows.Forms.Label lblQuestionText;
        private System.Windows.Forms.RadioButton rdoD;
        private System.Windows.Forms.RadioButton rdoC;
        private System.Windows.Forms.RadioButton rdoB;
        private System.Windows.Forms.RadioButton rdoA;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.FlowLayoutPanel flpQuestions;
        private System.Windows.Forms.Timer quizTimer;
    }
}