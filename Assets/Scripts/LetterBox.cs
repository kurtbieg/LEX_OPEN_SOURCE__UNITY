using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterBox : MonoBehaviour {
	public string myLetter;
	public int myPointValue;
	public GameObject doomBox, greyBox, whiteBox, exclmationMark;
	public TextMesh Letter, Value, gameOverText;
	public bool isOn;
	bool toShake, toFall, isDooming;

	public void Start () {
		doomBox.animation.Stop ();
		whiteBox.animation.Stop ();
		doomBox.transform.localScale = new Vector3 (1,1,1);
		whiteBox.transform.localPosition = new Vector3 (0,0,0);
		turnOn ();
		isOn = true;
		toFall = true;
		toShake = false;
	}

	public void setLetter (string value) {
		myLetter = value;
		Letter.text = myLetter;
		
		List<string> OnePt = new List<string>
		{ "e" , "a" , "i" , "o" , "n" , "r" , "t" , "l" , "s" , "u" };
		List<string> TwoPt = new List<string>
		{ "d" , "g" };
		List<string> ThreePt = new List<string>
		{ "b" , "c" , "m" , "p" };
		List<string> FourPt = new List<string>
		{ "f" , "h" , "v" , "w" , "y" };
		List<string> FivePt = new List<string>
		{ "k" };
		List<string> EightPt = new List<string>
		{ "j", "x" };
		List<string> TenPt = new List<string>
		{ "q", "z" };
		
		if (OnePt.Contains (myLetter))
			myPointValue = 1;
		else if (TwoPt.Contains (myLetter))
			myPointValue = 2;
		else if (ThreePt.Contains (myLetter))
			myPointValue = 3;
		else if (FourPt.Contains (myLetter))
			myPointValue = 4;
		else if (FivePt.Contains (myLetter))
			myPointValue = 5;
		else if (EightPt.Contains (myLetter))
			myPointValue = 8;
		else if (TenPt.Contains (myLetter))
			myPointValue = 10;
		
		Value.text = myPointValue.ToString ();
	}

	public void turnOn () {
		if (!isOn) {
			greyBox.SetActive(false);
			isOn = true;
		}
	}

	public void OnMouseDown () {
		if (isOn && !Master.master.isOver && !Master.master.didCheck) {
			Master.master.addNewLetter(myLetter, myPointValue, this.gameObject);
			greyBox.SetActive(true);
			WordBox.wordBox.grow ();
			isOn = false;
		}
	}

	public void warning () {
		startShake();
	}

	public void gameOver () {
		Master.master.GameOver();
	}

	public void die () {
		gameObject.Remove();
		toFall = true;
	}

	void resetBoxes () {
		whiteBox.transform.localPosition = new Vector3(0,0,0);
	}

	public void doomAnim () {
		if (!doomBox.animation.isPlaying) {
			doomBox.animation["rise"].speed = (1f / (myPointValue * (50f / Master.master.currLevel)));
			doomBox.animation.Play ();
			StartCoroutine(waitToAnim());
		}
	}

	IEnumerator waitToAnim () {
		yield return new WaitForEndOfFrame ();
		if (!doomBox.animation.isPlaying) 
			doomAnim ();
	}

	public void changeSpeed () {
		whiteBox.animation["shake"].speed = 10 * (1f - doomBox.transform.localScale.y);
	}

	public void augmentAnimSpeed () {
		doomBox.animation["rise"].speed = (1f / (myPointValue * (50f / Master.master.currLevel)));
	}

	public void moveTo (float posX, float wait) {
		iTween.MoveTo(gameObject, iTween.Hash ("x", (posX + 0.15f), "easeType", "easeInOutExpo", "time", 0.6f, "delay", wait, "oncomplete", "falconPaunch"));
	}


	public void endAnims () {
		if (toFall) {
			doomBox.animation.Stop ();
			whiteBox.animation.Stop ();
			turnOn ();
			whiteBox.transform.localEulerAngles = new Vector3(0,0,0);
			float waitTime = Random.Range (0,100) * 0.0025f;
			iTween.MoveAdd(whiteBox, iTween.Hash ("y", -4f, "easetype", "easeOutBounce", "time", 0.5f, "delay", waitTime));
			toFall = false;
		}
	}

	public void restartAnims () {
		float waitTime = Random.Range (0,100) * 0.0025f;
		iTween.MoveAdd(whiteBox, iTween.Hash ("name", "peter", "y", -5f, "easetype", "easeOutBounce", "time", 0.5f, "delay", waitTime));
	}


	void rotateOpposite () {
		iTween.RotateTo(whiteBox, iTween.Hash ("z", -3f, "easeType", "easeInQuad", "time", (0.05f * (doomBox.transform.localScale.y / 0.25f)) + 0.025f, "oncompletetarget", gameObject, "oncomplete", "startShake"));
	}

	public void startShake () {
		whiteBox.animation.Play ();
	}

	void falconPaunch () {
		iTween.MoveTo(gameObject, iTween.Hash ("x", (transform.position.x - 0.15f), "easeType", "easeInOutQuad", "time", 0.15f));
	}

	void checkX () {
		whiteBox.transform.localPosition = new Vector3(0,whiteBox.transform.localPosition.y,0);
	}
}
