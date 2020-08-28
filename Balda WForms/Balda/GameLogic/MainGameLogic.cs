using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Balda.GameLogic;

namespace Balda {
	[Serializable]
	public class MainGameLogic {
		protected Player[] players;
		public Player[] Players => players;
		public readonly int ROW = 5;//variables are responsible for the size of the field
		public readonly int COL = 5;
		protected  Random random = new Random();
		protected char[,] field;
		public char[,] Field {
			get => field;
			private set => field = value;
		}
		[NonSerialized]
		protected Dictionary<char, List<string>> Library;
		
		protected string f_word;
		/// <summary>
		/// A word that is composed at the moment
		/// </summary>
		[NonSerialized]
		public string currentWord;
		readonly string wordsFileName = @".\WordsLibrary\words.txt";
		[NonSerialized]
		/// <summary>
		/// list of current moves that makes in the turn
		/// </summary>
		public List<Point> moves;

		protected int step_count = 0;//counter moves

		public int StepCount => step_count % 2;

		/// <summary>
		/// coordinates of active table cell
		/// </summary>
		[NonSerialized]
		public Point activeCell;
		/// <summary>
		/// coordinates of input letter cell
		/// </summary>
		[NonSerialized]
		public Point inputCell;

		//public bool isFirstCell { get; set; }
		public bool isNewLetUsed { get; set; }

		/// <summary>
		/// adjacency matrix
		/// </summary>
		[NonSerialized]
		protected char[,] matrix;

		public char[,] Matrix {
			get => matrix;
		}

		public MainGameLogic() {
		}
		public MainGameLogic(string fPName, string sPName) {
			Field = new char[ROW, COL];
			matrix = new char[ROW, COL];
			moves = new List<Point>();
			players = new[] { new Player(fPName), new Player(sPName) };
			LoadWords();
			RandomFirstWordInit();
			FormingFirstMoveMatrix();
		}

		private void LoadWords() {
			Library = new Dictionary<char, List<string>>();
			int i = 0;
			for (char j = 'a'; j <= 'z'; j++) {
				Library.Add(j, new List<string>());
			}
			using (FileStream reader = new FileStream(wordsFileName, FileMode.Open, FileAccess.Read)) {
				using (StreamReader sr = new StreamReader(reader, Encoding.Default)) {
					while (!sr.EndOfStream) {
						string word = sr.ReadLine();
						if (Library[word[0]].Contains(word)) continue;
						Library[word[0]].Add(word);
					}
				}
			}
		}
		//to check!!!!!!!!!!
		protected bool CheckingFreePlaces() {
			for (int i = 0; i < ROW; i++) {
				for (int j = 0; j < COL; j++) {
					if (Field[i, j] == 0) return true;
				}
			}
			return false;
		}


		/// <summary>
		/// initialize and putted a first word of the game
		/// </summary>
		protected void RandomFirstWordInit() {
			f_word = "";
			while (f_word.Length != COL) {
				int keyIndex = random.Next(26);
				int valueIndex = random.Next(Library.ToArray()[(char) keyIndex].Value.Count);
				f_word = Library.ToArray()[(char)keyIndex].Value[valueIndex];
			}
			FirstWordInit();
		}

		private void FirstWordInit() {
			for (int i = 0; i < COL; i++) {
				Field[ROW / 2, i] = f_word[i];
			}
		}

		public void UpdateLogicState(Point curClickPos) {
			activeCell = new Point(curClickPos.X, curClickPos.Y);
			currentWord += field[curClickPos.X, curClickPos.Y];
			moves.Add(new Point(curClickPos.X, curClickPos.Y));
			FormingMatrixOfPossibleMove();
		}
		public void UpdateLogicState(Point curClickPos, char newLetter) {
			activeCell = new Point(curClickPos.X, curClickPos.Y);
			Field[curClickPos.X, curClickPos.Y] = newLetter;
			currentWord += field[curClickPos.X, curClickPos.Y];
			moves.Add(new Point(curClickPos.X, curClickPos.Y));
			inputCell = new Point(curClickPos.X, curClickPos.Y);
			isNewLetUsed = true;
			FormingMatrixOfPossibleMove();
		}
		public int CheckForWinner() {
			if (Players[0].PlPoints == Players[1].PlPoints) return 2;
			return Players[0].PlPoints < Players[1].PlPoints ? 1 : 0;
		}

		public string CheckLastLetNeighbors(int _x, int _y) {
			if (_x < COL && _x >= 0 && _y < ROW && _y >= 0) {
				if (!moves.Contains(new Point(_x, _y))) {
					if (Field[_x, _y] == 0 && !isNewLetUsed) return "canInput";
					if (Field[_x, _y] == 0 && isNewLetUsed) return "cantInput";
					return "existLetter";
				}
			}
			return "null";
		}

		public void FormingFirstMoveMatrix() {
			matrix = new char[ROW, COL];
			for (int i = 0; i < ROW; i++) {
				for (int j = 0; j < COL; j++) {
					if (Field[i, j] == 0) {
						if (CheckNeighbors(i, j)) matrix[i, j] = '?';
					}
					else if (Field[i, j] != 0) {
						matrix[i, j] = '#';

					}
				}
			}

		}
		/// <summary>
		/// Check neighbor when forming the adjasency  matrix
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private bool CheckNeighbors(int x, int y) {
			bool r = false, l = false, u = false, d = false;
			if (x + 1 < COL)
				if (Field[x + 1, y] != 0) u = true;
			if (x - 1 >= 0)
				if (Field[x - 1, y] != 0) d = true;
			if (y + 1 < ROW)
				if (Field[x, y + 1] != 0) r = true;
			if (y - 1 >= 0)
				if (Field[x, y - 1] != 0) l = true;
			return u || d || l || r;
		}

		public void FormingMatrixOfPossibleMove() {
			matrix = new char[ROW, COL];
			for (int i = 0; i < ROW; i++) {
				for (int j = 0; j < COL; j++) {
					if (moves.Contains(new Point(i, j))) {
						matrix[i, j] = '@';
					}

				}
				if (CheckLastLetNeighbors(_x: activeCell.X + 1, activeCell.Y) == "existLetter") {
					matrix[activeCell.X + 1, activeCell.Y] = '#';
				}
				else if (CheckLastLetNeighbors(activeCell.X + 1, activeCell.Y) == "canInput") {
					matrix[activeCell.X + 1, activeCell.Y] = '?';
				}
				if (CheckLastLetNeighbors(activeCell.X - 1, activeCell.Y) == "existLetter") {
					matrix[activeCell.X - 1, activeCell.Y] = '#';
				}
				else if (CheckLastLetNeighbors(activeCell.X - 1, activeCell.Y) == "canInput") {
					matrix[activeCell.X - 1, activeCell.Y] = '?';
				}
				if (CheckLastLetNeighbors(activeCell.X, activeCell.Y + 1) == "existLetter") {
					matrix[activeCell.X, activeCell.Y + 1] = '#';
				}
				else if (CheckLastLetNeighbors(activeCell.X, activeCell.Y + 1) == "canInput") {
					matrix[activeCell.X, activeCell.Y + 1] = '?';
				}
				if (CheckLastLetNeighbors(activeCell.X, activeCell.Y - 1) == "existLetter") {
					matrix[activeCell.X, activeCell.Y - 1] = '#';
				}
				else if (CheckLastLetNeighbors(activeCell.X, activeCell.Y - 1) == "canInput") {
					matrix[activeCell.X, activeCell.Y - 1] = '?';
				}
			}
		}

		public void UndoMoves(Point coordinates) {
			if (moves.Contains(coordinates)) {
				var ind = moves.IndexOf(coordinates);
				currentWord = currentWord.Remove(ind);
				if (isNewLetUsed && moves.IndexOf(inputCell) >= ind) {
					field[inputCell.X, inputCell.Y] = '\0';
					isNewLetUsed = false;
					inputCell = new Point();
				}
				moves.RemoveRange(ind, moves.Count - ind);
				if (ind != 0) {
					activeCell = moves[ind - 1];
					FormingMatrixOfPossibleMove();
				}
				else {
					activeCell = new Point();
					FormingFirstMoveMatrix();
				}

			}
		}
		/// <summary>
		/// checking if next move is new and was not used before in this turn
		/// </summary>
		/// <param name="_x">coordinate x</param>
		/// <param name="_y">coordinate y</param>
		/// <returns>true if next move not first in the turn</returns>
		protected bool CheckPastPos(int _x, int _y) {
			return moves.Count != 0 && moves.Any(t => t.X == _x && t.Y == _y);
		}

		public virtual int EndOfTurn() {
			if (Players[0].PlWords.Contains(currentWord) || Players[1].PlWords.Contains(currentWord) || currentWord == f_word) {
				return 1;
			}
			if (!Library[currentWord[0]].Contains(currentWord)) {
				return 2;
			}
			players[StepCount].AddWord(currentWord);
			CleanParams();
			if (!CheckingFreePlaces()) return 3;
			FormingFirstMoveMatrix();
			return 0;
		}

		protected virtual void CleanParams() {
			moves.Clear();
			isNewLetUsed = false;
			currentWord = "";
			inputCell = Point.Empty;
			step_count++;
		}

		public void SkipMove() {
			CleanParams();
			FormingFirstMoveMatrix();
		}

	}

}
