namespace Balda {
	partial class Form2MainMenu {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2MainMenu));
			this.buttonStart = new System.Windows.Forms.Button();
			this.textBox1Player = new System.Windows.Forms.TextBox();
			this.textBox2Player = new System.Windows.Forms.TextBox();
			this.label1Player = new System.Windows.Forms.Label();
			this.label2Player = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3Cpu = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonStart
			// 
			this.buttonStart.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonStart.Font = new System.Drawing.Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonStart.Location = new System.Drawing.Point(274, 151);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(169, 34);
			this.buttonStart.TabIndex = 0;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = false;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// textBox1Player
			// 
			this.textBox1Player.BackColor = System.Drawing.Color.Bisque;
			this.textBox1Player.Location = new System.Drawing.Point(351, 12);
			this.textBox1Player.Name = "textBox1Player";
			this.textBox1Player.Size = new System.Drawing.Size(169, 20);
			this.textBox1Player.TabIndex = 1;
			// 
			// textBox2Player
			// 
			this.textBox2Player.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.textBox2Player.Location = new System.Drawing.Point(351, 88);
			this.textBox2Player.Name = "textBox2Player";
			this.textBox2Player.Size = new System.Drawing.Size(169, 20);
			this.textBox2Player.TabIndex = 2;
			// 
			// label1Player
			// 
			this.label1Player.AutoSize = true;
			this.label1Player.BackColor = System.Drawing.Color.Bisque;
			this.label1Player.Font = new System.Drawing.Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1Player.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.label1Player.Location = new System.Drawing.Point(90, 10);
			this.label1Player.Name = "label1Player";
			this.label1Player.Size = new System.Drawing.Size(192, 21);
			this.label1Player.TabIndex = 3;
			this.label1Player.Text = "First player name:";
			// 
			// label2Player
			// 
			this.label2Player.AutoSize = true;
			this.label2Player.BackColor = System.Drawing.Color.Bisque;
			this.label2Player.Font = new System.Drawing.Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2Player.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.label2Player.Location = new System.Drawing.Point(65, 86);
			this.label2Player.Name = "label2Player";
			this.label2Player.Size = new System.Drawing.Size(217, 21);
			this.label2Player.TabIndex = 4;
			this.label2Player.Text = "Second player name:";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(351, 52);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(63, 17);
			this.checkBox1.TabIndex = 5;
			this.checkBox1.Text = "Vs CPU";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// label3Cpu
			// 
			this.label3Cpu.AutoSize = true;
			this.label3Cpu.BackColor = System.Drawing.Color.Bisque;
			this.label3Cpu.Font = new System.Drawing.Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3Cpu.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.label3Cpu.Location = new System.Drawing.Point(20, 48);
			this.label3Cpu.Name = "label3Cpu";
			this.label3Cpu.Size = new System.Drawing.Size(262, 21);
			this.label3Cpu.TabIndex = 6;
			this.label3Cpu.Text = "Check to play agains CPU";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Red;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(78, 151);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(169, 34);
			this.button1.TabIndex = 7;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form2MainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(532, 272);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label3Cpu);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label2Player);
			this.Controls.Add(this.label1Player);
			this.Controls.Add(this.textBox2Player);
			this.Controls.Add(this.textBox1Player);
			this.Controls.Add(this.buttonStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Form2MainMenu";
			this.Text = "Form2MainMenu";
			this.Load += new System.EventHandler(this.Form2MainMenu_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.TextBox textBox1Player;
		private System.Windows.Forms.TextBox textBox2Player;
		private System.Windows.Forms.Label label1Player;
		private System.Windows.Forms.Label label2Player;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label3Cpu;
		private System.Windows.Forms.Button button1;
	}
}