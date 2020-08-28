using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balda.GameLogic {
	[Serializable]
	public class Player {
		private string name;
		private bool isHuman;
		public bool IsHuman {
			get => isHuman;
			set => isHuman = value;
		}

		public string Name {
			get => name;
			set {
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)) {
					throw new ArgumentException();
				}

				name = value;
			}
		}

		private int plPoints;
		private List<string> plWords;
		public int PlPoints {
			get => plPoints;
			set => plPoints = value;
		}
		public List<string> PlWords {
			get => plWords;
			set => plWords = value;
		}

		public Player(string name, bool isHuman = true) {
			Name = name;
			PlWords = new List<string>();
			PlPoints = 0;
			this.isHuman = isHuman;
		}

		public void AddWord(string word) {
			PlWords.Add(word);
			PlPoints += word.Length;
		}
	}
}
