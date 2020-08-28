using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balda {
	[Serializable]
	public class AILogic : MainGameLogic {

		private class TempAiWords {
			/// <summary>
			/// unfiltered combinations of letters
			/// </summary>
			public List<string> words = new List<string>();
			/// <summary>
			/// coordinates of new letter in word in list words
			/// </summary>
			public List<(int x, int y)> coordinates = new List<(int x, int y)>();
			/// <summary>
			/// list of current moves that makes in the turn
			/// </summary>
			public List<(int x, int y)> AiMoves = new List<(int x, int y)>();
			/// <summary>
			/// Max length of word that can be combined by AI
			/// </summary>
			public int MaxLength = 10;
			public int CurrentMaxLength;
		}
		private class AiWord {
			public List<string> Words = new List<string>();
			public List<(int x, int y)> LetterCoordinates = new List<(int x, int y)>();
			public List<char> Letters = new List<char>();
		}


		//private bool difficulty;
		[NonSerialized]
		private TempAiWords tempAiWords;
		[NonSerialized]
		private AiWord aiWord;

		public AILogic() {
		}
		public AILogic(string fPName, string sPName) : base(fPName, sPName) {
		}

		/// <summary>
		/// Clean AI data before turn
		/// </summary>
		protected override void CleanParams() {
			matrix = new char[ROW, COL];
			tempAiWords = new TempAiWords();
			aiWord = new AiWord();
			base.CleanParams();
		}

		/// <summary>
		/// Process of ai movement
		/// </summary>
		/// <returns>true when end turn</returns>
		public override int EndOfTurn() {
			if (StepCount == 0) {
				return base.EndOfTurn();
			}
			if (!CheckingFreePlaces()) return 3;
			FormingMatrix();
			string tempWord;
			int newX, newY;
			for (int i = 0; i < ROW; i++) {
				for (int j = 0; j < COL; j++) {
					if (matrix[i, j] == 48) continue; //if 0 continue
					newX = -1; newY = -1; // coordinates of new letter -1 - mean new letter dosn't used yet
					tempWord = "";
					tempAiWords.AiMoves.Clear();

					FindWords(i, j, newX, newY, tempWord);
				}
			}

			Filter();
			if (aiWord.Words.Count > 0) ChooseWord();
			else {
				return 4;
			}
			players[1].AddWord(currentWord);
			CleanParams();
			FormingFirstMoveMatrix();
			return 0;
		}
		/// <summary>
		/// Choose word in filtered list of word that AI make
		/// </summary>
		private void ChooseWord() {


			int bigestInd = default;
			for (int i = 0; i < aiWord.Words.Count; i++) {
				if (aiWord.Words[i].Length > currentWord.Length) {
					currentWord = aiWord.Words[i];
					bigestInd = i;
				}
			}
			Field[aiWord.LetterCoordinates[bigestInd].x, aiWord.LetterCoordinates[bigestInd].y] =
				aiWord.Letters[bigestInd];

			aiWord.Words = new List<string>();
			aiWord.LetterCoordinates = new List<(int x, int y)>();
			aiWord.Letters = new List<char>();
		}
		/// <summary>
		/// Filtered list of words in new list by comparing them with Library
		/// </summary>
		private void Filter() {
			string tempWord;
			char letter;
			while (tempAiWords.words.Count > 0) {
				if (!tempAiWords.words[0].Contains("?")) {
					tempAiWords.words.RemoveAt(0);
					tempAiWords.coordinates.RemoveAt(0);
					continue;
				}
				for (int i = 97; i <= 122; i++) {
					letter = (char)i;
					tempWord = tempAiWords.words[0];
					tempWord = tempWord.Replace('?', letter);

					if (!Players[0].PlWords.Contains(tempWord) &&
						!Players[1].PlWords.Contains(tempWord) && tempWord != f_word) {
						if (!aiWord.Words.Contains(tempWord) && Library[tempWord[0]].Contains(tempWord)) {
							aiWord.Words.Add(tempWord);
							aiWord.Letters.Add((char)i);
							aiWord.LetterCoordinates.Add((tempAiWords.coordinates[0]
								.x, tempAiWords.coordinates[0].y));
						}
					}
				}
				tempAiWords.words.RemoveAt(0);
				tempAiWords.coordinates.RemoveAt(0);
			}
		}

		void MakeRecursiveCall(int x, int y, int newX, int newY, string tempWord) {
			var nextPos = CheckNextPosition(x + 1, y, newX);
			if (nextPos == 1 || nextPos == 2)
				FindWords(x + 1, y, newX, newY, tempWord);
			nextPos = CheckNextPosition(x - 1, y, newX);
			if (nextPos == 1 || nextPos == 2)
				FindWords(x - 1, y, newX, newY, tempWord);
			nextPos = CheckNextPosition(x + 1, y, newX);
			if (nextPos == 1 || nextPos == 2)
				FindWords(x, y + 1, newX, newY, tempWord);
			nextPos = CheckNextPosition(x + 1, y, newX);
			if (nextPos == 1 || nextPos == 2)
				FindWords(x, y - 1, newX, newY, tempWord);
		}
		/// <summary>
		/// find all possible combination of letter
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="newX"></param>
		/// <param name="newY"></param>
		/// <param name="tempWord"></param>
		private void FindWords(int x, int y, int newX, int newY, string tempWord) {
			if (tempWord.Length >= tempAiWords.CurrentMaxLength) return;
			int nextPos = CheckNextPosition(x, y, newX);
			if (nextPos == 0) return;
			if (nextPos == 2) {
				newX = x;
				newY = y;
				tempWord += matrix[x, y];
			}
			else tempWord += matrix[x, y];

			if (tempWord.Length > 1) {
				tempAiWords.words.Add(tempWord);
				tempAiWords.coordinates.Add((newX, newY));
			}
			tempAiWords.AiMoves.Add((x, y));
			MakeRecursiveCall(x, y, newX, newY, tempWord);
		}

		private int CheckNextPosition(int x, int y, int newX) {
			if (x < ROW && y < COL && x >= 0 && y >= 0) {
				if (matrix[x, y] != (char)63 && matrix[x, y] != (char)48 &&
					!tempAiWords.AiMoves.Contains((x, y))) return 1;
				if (matrix[x, y] == (char)63 && newX == -1 &&
					!tempAiWords.AiMoves.Contains((x, y))) return 2;
			}
			return 0;
		}

		private void FormingMatrix() {
			for (int i = 0; i < ROW; i++) {
				for (int j = 0; j < COL; j++) {
					if (Field[i, j] == '\0')
						if (!CheckNeighbors(i, j)) matrix[i, j] = (char)48;
						else matrix[i, j] = (char)63;
					else if (Field[i, j] != '\0') {
						matrix[i, j] = Field[i, j];
						//max length of word can't be more then number of letters in the field + 1 or 10
						if (tempAiWords.CurrentMaxLength < tempAiWords.MaxLength - 1) tempAiWords.CurrentMaxLength++;
					}
				}
			}
			tempAiWords.CurrentMaxLength++;
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
				if (Field[x + 1, y] != '\0') u = true;
			if (x - 1 >= 0)
				if (Field[x - 1, y] != '\0') d = true;
			if (y + 1 < ROW)
				if (Field[x, y + 1] != '\0') r = true;
			if (y - 1 >= 0)
				if (Field[x, y - 1] != '\0') l = true;
			return u || d || l || r;
		}
	}
}
