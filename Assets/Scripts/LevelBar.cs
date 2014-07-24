using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBar : MonoBehaviour {

	public static LevelBar lb;

	public List<Color> bgColors;
	public List<Texture> fgColors;
	public int colorID, words, wordsToLvl;
	public GUITexture fgColor;
	public TextMesh lvlText;

	public void Start () {
		lb = this;
		iTween.Stop (fgColor.gameObject);
		fgColor.transform.localScale = new Vector3 (0,1,1);
		lvlText.transform.position = new Vector3 (-10, 4.7f, 0);
		lvlText.text = "";
		colorID = 0;
		words = 0;
		gameObject.camera.backgroundColor = bgColors[colorID];
		colorID++;
		fgColor.texture = fgColors[colorID];
		colorID++;
	}

	public void addExp () {
		if (Master.master.currLevel < 10)
			words++;
		if (words == wordsToLvl) {
			Master.master.currLevel++;
			Master.master.gameObject.BroadcastMessage ("augmentAnimSpeed");
			popTextUp ();
		}
		iTween.ScaleAdd (fgColor.gameObject, iTween.Hash ("x", (1f / (wordsToLvl * 1f)) * 2f, "time", 0.75f,  "easeType", "easeOutElastic", "oncomplete", "checkLvUp", "oncompletetarget", gameObject));
	}
	
	void checkLvUp () {
		if (words == wordsToLvl) {
			words = 0;
			int holder = fgColors.IndexOf (fgColor.texture);
			gameObject.camera.backgroundColor = bgColors[holder];
			fgColor.transform.localScale = new Vector3 (0,1,1);
			fgColor.texture = fgColors[colorID];
			colorID++;
		}
	}

	void popTextUp () {
		iTween.MoveBy (lvlText.gameObject, iTween.Hash ("y", 5f, "time", 0.25f, "delay", 0.5f, "easetype", "easeOutElastic", "oncomplete", "popTextDown", "oncompletetarget", gameObject));
	}

	void popTextDown () {
		lvlText.text = Master.master.currLevel + "x";
		iTween.MoveBy (lvlText.gameObject, iTween.Hash ("y", -5f, "time", 0.25f, "easetype", "easeOutElastic"));
	}

	public void endAnimFirst () {
		iTween.ScaleTo (fgColor.gameObject, iTween.Hash ("x", 0, "time", (0.35f - (0.35f * (words * 1f / 3f))), "easeType", "easeInExpo", "oncomplete", "endAnimSecond", "oncompletetarget", gameObject, "delay", 0.05f));
	}
	
	void endAnimSecond () {
		if (colorID > 1) {
			int holder = bgColors.IndexOf (gameObject.camera.backgroundColor);
			fgColor.texture = fgColors[holder];
			fgColor.transform.localScale = new Vector3 (2,1,1);
			gameObject.camera.backgroundColor = bgColors[0];
			words = 3;
			endAnimFirst ();
		}
		else {
			gameObject.camera.backgroundColor = bgColors[0];
			fgColor.texture = fgColors[1];
			colorID = 2;
			words = 0;
		}
	}
}
