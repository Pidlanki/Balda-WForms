using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balda {
	[Serializable]
	public class MainGameLogic {
		protected (int PlPoints, List<string> PlWords) firstPlayer;
		protected (int PlPoints, List<string> PlWords) secondPlayer;
		public (int PlPoints, List<string> PlWords) FirstPlayer => firstPlayer;
		public (int PlPoints, List<string> PlWords) SecondPlayer => secondPlayer;
		public readonly int ROW = 5;//variables are responsible for the size of the field
		public readonly int COL = 5;
		protected  Random random = new Random();
		protected char[,] field;
		public char[,] Field {
			get => field;
			private set => field = value;
		}

		protected Dictionary<char, List<string>> Library;
		protected string f_word;
		/// <summary>
		/// A word that is composed at the moment
		/// </summary>
		[NonSerialized]
		public string currentWord;
		readonly string wordsFileName = @"D:\Users\pidla\source\repos\Balda-WForms\Balda WForms\Balda\WordsLibrary\words.txt";
		/// <summary>
		/// list of current moves that makes in the turn
		/// </summary>
		public List<Point> moves;

		protected int step_count = 0;//counter moves
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

		public bool isFirstCell { get; set; }
		public bool isNewLetUsed { get; set; }

		/// <summary>
		/// adjacency matrix
		/// </summary>
		[NonSerialized]
		private char[,] matrix;

		public char[,] Matrix {
			get => matrix;
		}
		public MainGameLogic() {
			Field = new char[ROW, COL];
			matrix = new char[ROW, COL];
			moves = new List<Point>();
			firstPlayer.PlWords = new List<string>();
			secondPlayer.PlWords = new List<string>();
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
		/// Main process of the game
		/// </summary>
		protected virtual bool Game() {
			RandomFirstWordInit();
			//SerializeGame serializer = new SerializeGame();

			currentWord = "";
			step_count++;

			//var isEndMoving = Moving();
			//if (isEndMoving) CountingPoints();
			//serializer.SerializePVP(this);
			return CheckingFreePlaces();
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
			currentWord += field[curClickPos.X, curClickPos.Y];
			moves.Add(new Point(curClickPos.X, curClickPos.Y));
			inputCell = new Point(curClickPos.X, curClickPos.Y);
			isNewLetUsed = true;
			Field[curClickPos.X, curClickPos.Y] = newLetter;
			FormingMatrixOfPossibleMove();
		}
		protected int CheckForWinner() {
			if (FirstPlayer.PlPoints == SecondPlayer.PlPoints) return 0;
			return FirstPlayer.PlPoints < SecondPlayer.PlPoints ? 2 : 1;
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
				if (isNewLetUsed && moves.IndexOf(inputCell) > ind) {
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
		public bool CheckPastPos(int _x, int _y) {
			return moves.Count != 0 && moves.Any(t => t.X == _x && t.Y == _y);
		}

		/// <summary>
		/// Process of movement on the table
		/// </summary>
		/// <returns>true if turn ended corectly</returns>
		//	protected bool Moving(int _x, int _y) {

		//		isFirstCell = false;
		//		isNewLetUsed = false;

		//		//чи не занята клітинка
		//		if (isFirstCell) {
		//			isNewLetUsed = EnterFirstLetter();
		//			isFirstCell = true;
		//		}
		//		else if (MakeMove(_x, _y)) {
		//			isFirstCell = true;
		//			x = _x;
		//			y = _y;
		//		}



		//	}

		//}

		//public bool EndTurnPress() {
		//	if (EndTurn(ref isNewLatUsed)) {
		//		count = 0;
		//		moves = new List<int>();
		//		return true;
		//	}
		//	else {
		//		count = 0;
		//		moves = new List<int>();
		//		step_count--;
		//		return false;
		//	}
	}

}
