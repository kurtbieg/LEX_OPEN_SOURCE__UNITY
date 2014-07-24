using UnityEngine;
using System.Collections;

public class LetterTileAnim : MonoBehaviour {

	public LetterBox master;

	void warning () {
		master.warning ();
	}

	void gameOver () {
		master.gameOver ();
	}

	void changeSpeed () {
		master.changeSpeed ();
	}
}
