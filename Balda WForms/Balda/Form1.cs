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
	public partial class Form1 : Form {
		private MainGameLogic gameLogic;
		public Form1() {
			InitializeComponent();
			gameLogic = new MainGameLogic();
			ConfigureDataGridView();
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



		}

		private void Form1_Load(object sender, EventArgs e) {
			labelWord.Text = "";
			DrawBackCells();
		}

		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
			char temp = (char) dataGridView1[e.ColumnIndex, e.RowIndex].Value;
			if (char.IsLetter(temp)) {
				gameLogic.UpdateLogicState(new Point(e.RowIndex, e.ColumnIndex), temp);
				labelWord.Text = gameLogic.currentWord;
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
	}
}
