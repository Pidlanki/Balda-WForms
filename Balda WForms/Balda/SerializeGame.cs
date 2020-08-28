using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Balda;

namespace Balda_Vcs {
	class SerializeGame {

		public bool ExistSaves() {
			return File.Exists(@".\Saves\SaveGame.bin");
		}
		public void Serialize(MainGameLogic game) {
			try {
				BinaryFormatter formatter = new BinaryFormatter();
				using (Stream st = File.Create(@".\Saves\SaveGame.bin")) {
					formatter.Serialize(st, game);

				}
				

			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Deserialize(ref MainGameLogic game) {
			game = new MainGameLogic();
			try {
				BinaryFormatter formatter = new BinaryFormatter();
				game = null;
				using (Stream st = File.OpenRead(@".\Saves\SaveGame.bin")) {
					game = (MainGameLogic)formatter.Deserialize(st);
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//public void SerializeAI(AILogic game) {
		//	try {
		//		BinaryFormatter formatter = new BinaryFormatter();
		//		using (Stream st = File.Create(@".\Saves\AISaveGame.bin")) {
		//			formatter.Serialize(st, game);

		//		}
		//		Console.WriteLine("Game was saved...");
		//	}
		//	catch (Exception ex) {
		//		Console.WriteLine(ex.Message);
		//	}
		//}

		//protected void DeserializeAI() {
		//	AILogic game = new AILogic();
		//	try {
		//		BinaryFormatter formatter = new BinaryFormatter();
		//		game = null;
		//		using (Stream st = File.OpenRead(@".\Saves\AISaveGame.bin")) {
		//			game = (AILogic)formatter.Deserialize(st);
		//		}

		//		Console.WriteLine(game);
		//	}
		//	catch (Exception ex) {
		//		Console.WriteLine(ex.Message);
		//	}
		//	game.ContinueMenu();

		//}
	}
}

