using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Balda_Vcs;

namespace Balda {
	public partial class Form1 : Form {
		private MainGameLogic gameLogic;
		private SerializeGame serialize;
		private bool isVsAIGame = false;
		public Form1() {
			InitializeComponent();
		}

		bool IsPVP() {
			return gameLogic.Players[0].IsHuman && gameLogic.Players[0].IsHuman;
		}
		private void button1NewGame_Click(object sender, EventArgs e) {
			Form2MainMenu mainMenu = new Form2MainMenu();
			if (mainMenu.ShowDialog() == DialogResult.OK) {
				isVsAIGame = mainMenu.IsVsCPU;
				if (isVsAIGame) {
					gameLogic = new AILogic(mainMenu.FirstPlayer, mainMenu.SecondPlayer);
					gameLogic.Players[1].IsHuman = false;
				}
				else gameLogic = new MainGameLogic(mainMenu.FirstPlayer, mainMenu.SecondPlayer);
				ConfiguratePlayField();
				DrawBackCells();
			}
		}

		void ConfiguratePlayField() {
			button1NewGame.Visible = false;
			buttonContinue.Visible = false;
			ConfigureDataGridView();
			splitContainer1.BackColor = Color.Bisque;
			splitContainer2.Visible = true;
			splitContainer2.BackColor = Color.Bisque;
			splitContainer3.Visible = true;
			splitContainer3.BackColor = Color.Bisque;
			splitContainer4.BackColor = Color.Bisque;
			toolStrip1.Visible = true;
			groupBox1.Visible = true;
			groupBox2.Visible = true;
			groupBox1.Text = gameLogic.Players[0].Name;
			groupBox2.Text = gameLogic.Players[1].Name;
			labelWord.Text = "";
			label1.Visible = true;
			labelPoints.Visible = true;
			Text = gameLogic.Players[0].Name;
			groupBox1.ForeColor = Color.Red;

		}
		void ConfigureDataGridView() {
			for (int i = 0; i < gameLogic.COL; i++) {
				DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
				column.MaxInputLength = 1;
				dataGridView1.Columns.Add(column);
			}
			for (int i = 0; i < gameLogic.ROW; i++) {
				dataGridView1.Rows.Add();
			}

			foreach (DataGridViewRow row in dataGridView1.Rows) {
				row.Height = 60;
			}

			foreach (DataGridViewColumn column in dataGridView1.Columns) {
				column.Width = 60;
			}

			dataGridView1.ColumnHeadersVisible = false;

			foreach (DataGridViewColumn column in dataGridView1.Columns) {
				column.ValueType = typeof(char);

			}

			dataGridView1.Visible = true;
		}
		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			Point curClick = new Point(dataGridView1.CurrentCellAddress.X, dataGridView1.CurrentCellAddress.Y);
			if (gameLogic.Matrix[curClick.Y, curClick.X] == '?') {
				dataGridView1.BeginEdit(false);
			}
			else if (gameLogic.Matrix[curClick.Y, curClick.X] == '#') {
				gameLogic.UpdateLogicState(new Point(curClick.Y, curClick.X));
				labelWord.Text = gameLogic.currentWord;
				DrawBackCells();
			}
			else if (gameLogic.Matrix[curClick.Y, curClick.X] == '@') {
				gameLogic.UndoMoves(new Point(curClick.Y, curClick.X));
				labelWord.Text = gameLogic.currentWord;
				DrawBackCells();
			}
			if (!gameLogic.isNewLetUsed) toolStripButtonEndMove.Enabled = false;



		}

		private void Form1_Load(object sender, EventArgs e) {
			serialize = new SerializeGame();
			buttonContinue.Enabled = serialize.ExistSaves();
		}

		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
			char temp = (char) dataGridView1[e.ColumnIndex, e.RowIndex].Value;
			if (char.IsLetter(temp)) {
				gameLogic.UpdateLogicState(new Point(e.RowIndex, e.ColumnIndex), temp);
				labelWord.Text = gameLogic.currentWord;
				toolStripButtonEndMove.Enabled = true;

			}
			DrawBackCells();
		}

		void DrawBackCells() {
			Refresh();
			for (int i = 0; i < gameLogic.ROW; i++) {
				for (int j = 0; j < gameLogic.COL; j++) {
					dataGridView1[j, i].Value = gameLogic.Field[i, j];

					switch (gameLogic.Matrix[i, j]) {
						case '#':
						case '?':
							dataGridView1[j, i].Style.BackColor = Color.Aquamarine;
							break;
						case '@':
							dataGridView1[j, i].Style.BackColor = Color.Brown;
							break;
						default:
							dataGridView1[j, i].Style.BackColor = Color.White;
							break;
					}
				}
			}
		}

		private void toolStripButtonEndMove_Click(object sender, EventArgs e) {
			var endTurn = gameLogic.EndOfTurn();
			switch (endTurn) {
				case 1:
					MessageBox.Show("This word has already been used, think further!", "Atention", MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
					return;
				case 2:
					MessageBox.Show($"There no such word \"{labelWord.Text}\" in library.", "Atention", MessageBoxButtons.OK,
						MessageBoxIcon.Warning);

					//todo make cleaning params to make word from begining

					return;
				case 3: {
					EndGameDialog();
					break;
				}
				case 4:
					MessageBox.Show("I'm skiping the turn", "Message from PC", MessageBoxButtons.OK,
						MessageBoxIcon.Information);
					toolStripButtonSkip_Click(this, e);
					return;
			}

			labelWord.Text = gameLogic.currentWord;
			ChangeAccentOfPlayer();
			AddingWordsPoints();
			toolStripButtonEndMove.Enabled = false;
			serialize.Serialize(gameLogic);
			DrawBackCells();
			if (gameLogic.StepCount == 1 && isVsAIGame) toolStripButtonEndMove_Click(this, e);
		}

		void EndGameDialog() {
			var winner = gameLogic.CheckForWinner();
			if (winner <= 1) MessageBox.Show($"{gameLogic.Players[winner].Name} wins", "Result", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
			else DialogResult = MessageBox.Show($"It's a draw", "Result", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
			File.Delete(@".\Saves\SaveGame.bin");
			if (DialogResult == DialogResult.Retry) Application.Restart();
			else Close();
		}
		void AddingWordsPoints() {
			labelPoints.Text = $"{gameLogic.Players[0].PlPoints} - {gameLogic.Players[1].PlPoints}";
			if (gameLogic.StepCount == 0) {
				textBox2.Text += gameLogic.Players[1].PlWords.Last() + " - ";
			}
			else {
				textBox1.Text += gameLogic.Players[0].PlWords.Last() + " - ";
			}
		}
		void AddingWordsPointsAfterSerialization() {
			labelPoints.Text = $"{gameLogic.Players[0].PlPoints} - {gameLogic.Players[1].PlPoints}";
			foreach (var plWord in gameLogic.Players[0].PlWords) {
				textBox1.Text += plWord;
			}
			foreach (var plWord in gameLogic.Players[1].PlWords) {
				textBox2.Text += plWord;
			}
		}
		void ChangeAccentOfPlayer() {
			Text = gameLogic.Players[gameLogic.StepCount].Name + " move";
			if (gameLogic.StepCount == 0) {
				groupBox1.ForeColor = Color.Red;
				groupBox2.ForeColor = Color.Black;
			}
			else {
				groupBox1.ForeColor = Color.Black;
				groupBox2.ForeColor = Color.Red;
			}
		}

		private void toolStripButtonSkip_Click(object sender, EventArgs e) {
			gameLogic.SkipMove();
			labelWord.Text = gameLogic.currentWord;
			toolStripButtonEndMove.Enabled = false;
			ChangeAccentOfPlayer();
			serialize.Serialize(gameLogic);
			DrawBackCells();
			if (gameLogic.StepCount == 1 && isVsAIGame) toolStripButtonEndMove_Click(this, e);
		}

		private void buttonContinue_Click(object sender, EventArgs e) {
			serialize.Deserialize(ref gameLogic);
			isVsAIGame = IsPVP();
			ConfiguratePlayField();
			AddingWordsPointsAfterSerialization();
			ChangeAccentOfPlayer();
			gameLogic.FormingFirstMoveMatrix();
			DrawBackCells();

		}

		private void toolStripButtonEndGame_Click(object sender, EventArgs e) {
			EndGameDialog();
		}
	}
}
