using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Balda {
	public partial class Form2MainMenu : Form {
		public string FirstPlayer { get; private set; }
		public string SecondPlayer { get; private set; }
		public bool IsVsCPU { get; private set; }
		public Form2MainMenu() {
			InitializeComponent();

		}

		private void Form2MainMenu_Load(object sender, EventArgs e) {

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			if (checkBox1.Checked) {
				IsVsCPU = true;
				label2Player.Enabled = false;
				textBox2Player.Enabled = false;
				textBox2Player.Text = "Balda";
			}
			else {
				IsVsCPU = false;
				label2Player.Enabled = true;
				textBox2Player.Enabled = true;
				textBox2Player.Text = "";
			}
		}

		private void buttonStart_Click(object sender, EventArgs e) {
			FirstPlayer = textBox1Player.Text;
			SecondPlayer = textBox2Player.Text;
			if (string.IsNullOrEmpty(FirstPlayer) || string.IsNullOrWhiteSpace(FirstPlayer)) {
				MessageBox.Show("Enter some nickname for the first player", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(SecondPlayer) || string.IsNullOrWhiteSpace(SecondPlayer)) {
				MessageBox.Show("Enter some nickname for the second player", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			DialogResult = DialogResult.OK;
			Close();
		}

		private void button1_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
