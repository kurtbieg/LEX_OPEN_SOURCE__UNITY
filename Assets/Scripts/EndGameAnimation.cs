using UnityEngine;
using System.Collections;

public class EndGameAnimation : MonoBehaviour {
	public static EndGameAnimation ega;

	public TextMesh topScore, mostValuableWord;
	public GameObject resetBtn, twitterBtn, fbBtn;
	
	void Start () {
		ega = this;
		enableBtns (false);
	}

	public void startAnims () {
		if (Master.master.myScore == PlayerInfo.bestScore) {
			topScore.text = "New High Score : ";
			Master.master.topScore.gameObject.SetActive (false);
		}
		else
			topScore.text = "Score : ";
		
		topScore.text += Master.master.myScore.ToString ();
		mostValuableWord.text = "Longest Word : " + PlayerInfo.currentMostValuableWord + " (" + PlayerInfo.currentMostValuableWordValue.ToString () + ")";
		enableBtns (true);
		animateButtons ();
		animateText();
	}
	
	public void resetAnim () {
		iTween.ScaleTo (resetBtn, iTween.Hash ("scale", new Vector3 (0,0,0), "time", 0.25f, "easetype", "easeOutBounce", "delay", 0.3f));

		iTween.MoveBy (topScore.gameObject, iTween.Hash ("y", 10f, "time", 0.25f, "easetype", "easeOutQuad", "delay", 0.4f));
		iTween.MoveBy (mostValuableWord.gameObject, iTween.Hash ("y", 10f, "time", 0.25f, "easetype", "easeOutQuad", "delay", 0.45f, "oncomplete", "enableStuff", "oncompletetarget", gameObject));
	}
	
	void animateButtons () {
		iTween.ScaleTo (resetBtn, iTween.Hash ("scale", new Vector3 (1f,1f,1f), "easeType", "easeInOutBounce", "time", 0.2f, "delay", 1.5f));
	}
	
	void animateText () {
		iTween.MoveBy (mostValuableWord.gameObject, iTween.Hash ("y", -10f, "easeType", "easeInOutElastic", "time", 1f, "delay", 0.4f));
		iTween.MoveBy (topScore.gameObject, iTween.Hash ("y", -10f, "easeType", "easeInOutElastic", "time", 1f, "delay", 0.52f));
	}

	void enableBtns (bool toSet) {
		resetBtn.SetActive(toSet);
		twitterBtn.SetActive(toSet);
		fbBtn.SetActive(toSet);
		
	}
	
	void enableStuff () {
		Master.master.restart();
		Master.master.topScore.gameObject.SetActive (true);
	}
}
