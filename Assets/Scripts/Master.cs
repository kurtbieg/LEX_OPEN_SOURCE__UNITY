using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Master : MonoBehaviour {

	public static Master master;
	
	public List<string> letters, vowels;
	public List<GameObject> boxes, tiles;
	public List<string> wordPartLetters, lettersInHand;
	public List<int> wordPartScore;
	public List<float> Xlocations;
	public int handCount, currLevel, vCount, cCount, letter;
	public int myScore, myBonus;
	public bool countDown, isOver, menuOpen;
	public string currentWord;
	public TextMesh Score, topScore, wordPrev, scorePrev, bonusPreview;
	public GameObject checkButton;
	public bool didCheck, oneDouble, twoDoubles;
	public string doubleLetterOne, doubleLetterTwo;
	public List<string> tutorialGraphics;
	public bool tutorialOn;
	public TextMesh tutorialText;
	public int tWords;


	void Start () {
		master = this;

		List<string> dic = new List<string>(){"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
		wordCheck.setLanguage(dic);
		SaveManager.Load ();
		if (PlayerInfo.bestScore == 0) 
			tutorialSetUp ();
		else 
			tutorialOn = false;

		gamePrep ();
		checkSpawn ();
	}

	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.ScreenToWorldPoint(Input.mousePosition));
			
			if (hit)
				hit.collider.collider2D.SendMessage ("OnMouseDown", SendMessageOptions.DontRequireReceiver);
		}

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return))
			checkWord ();

		if (Input.GetKeyDown(KeyCode.Escape)){
			if (isOver)
				restartAnim ();
			else
				StartCoroutine (backToMainMenu());
		}

		foreach (char c in Input.inputString) {
			if ((c >= 'a' && c <= 'z')||(c >= 'A' && c <= 'Z'))
				checkLetter (c.ToString ());

			else if (c == "\b"[0])
				if (letters.Count > 0 && !didCheck)
					WordBox.wordBox.backSpace ();
		}
	}

	void FixedUpdate () {

	}

	public void GUIstuff (string type) {
		switch (type) {
		case "check" : 
			checkWord ();
			break;
			
		case "Menu" :
			menuOpen = true;
			break;
			
		case "restartButton" :
			restartAnim();
			break;
			
		case "twitterButton" :
			
			break;
			
		case "facebookButton" :
			
			break;

		case "back" :
			Application.LoadLevel(0);
			break;
		}
	}

	void gamePrep () {
		isOver = false;
		didCheck = false;
		oneDouble = false;
		twoDoubles = false;
		currLevel = 1;
		topScore.text = PlayerInfo.bestScore.ToString ();
		PlayerInfo.currentLongestWord = "";
		PlayerInfo.currentLongestWordLength = 0;
		PlayerInfo.currentMostValuableWord = "";
		PlayerInfo.currentMostValuableWordValue = 0;
		vCount = 0;
		cCount = 0;
		myScore = 0;
		Score.text = myScore.ToString ();
		letters = new List<string>();

		for (int i  = 0; i < 19; i++){
			letters.Add ("e");
		}
		for (int i  = 0; i < 13; i++){
			letters.Add ("t");
		}
		for (int i  = 0; i < 12; i++){
			letters.Add ("a");
			letters.Add ("r");
		}
		for (int i  = 0; i < 11; i++){
			letters.Add ("i");
			letters.Add ("n");
			letters.Add ("o");
		}
		for (int i  = 0; i < 9; i++){
			letters.Add ("s");
		}
		for (int i  = 0; i < 6; i++){
			letters.Add ("d");
		}
		for (int i  = 0; i < 5; i++){
			letters.Add ("c");
			letters.Add ("h");
			letters.Add ("u");
		}
		for (int i  = 0; i < 4; i++){
			letters.Add ("f");
			letters.Add ("m");
			letters.Add ("p");
		}
		for (int i  = 0; i < 3; i++){
			letters.Add ("g");
			letters.Add ("y");
		}
		for (int i  = 0; i < 2; i++){
			letters.Add ("w");
		}
		letters.Add ("b");
		letters.Add ("j");
		letters.Add ("k");
		letters.Add ("q");
		letters.Add ("v");
		letters.Add ("x");
		letters.Add ("z");
	}

	void checkSpawn () {
		countDown = false;
		foreach (Transform i in gameObject.transform) {
			if (i.localPosition == new Vector3 (0,0,0)) {

				checkDoubles ();
				int letterType = letter;
				lettersInHand.Add (letters[letterType]);
				i.gameObject.GetComponent<LetterBox>().setLetter (letters[letterType]);
				tiles.Add (i.gameObject);
			}
		}

		for (int i = 0; i < tiles.Count; i++) {
			tiles[i].GetComponent<LetterBox>().moveTo (Xlocations[i], (0));
		}

		if (!tutorialOn || tWords > 2) {
			BroadcastMessage ("doomAnim");
			countDown = true;
		}
	}

	void getLetter (int temp, int dtype) {

		if (vowels.Contains (letters[temp].ToLower ())) {
			if (vCount < 4) {
				vCount++;
				letter = temp;

				if (dtype == 1) {
					oneDouble = true;
					doubleLetterOne = letters[temp];
				}
				else if (dtype == 2) {
					twoDoubles = true;
					doubleLetterTwo = letters[temp];
				}
			}
			else {
				checkDoubles();
			}
		}
		else {
			if (cCount < 5) {
				cCount++;
				letter = temp;

				if (dtype == 1) {
					oneDouble = true;
					doubleLetterOne = letters[temp];
				}
				else if (dtype == 2) {
					twoDoubles = true;
					doubleLetterTwo = letters[temp];
				}
			}
			else {
				checkDoubles();
			}
		}
	}

	void checkDoubles () {
		int temp = Random.Range (0, letters.Count);
		int dtype = 0;

		if (lettersInHand.Contains (letters[temp])) {
			if (!oneDouble && letters[temp] != doubleLetterTwo) {
				dtype = 1;
				getLetter (temp, dtype);
			}
			else if (!twoDoubles && letters[temp] != doubleLetterOne) {
				dtype = 2;
				getLetter (temp, dtype);
			}
			else {
				checkDoubles();
			}
		}
		else {
			getLetter (temp, dtype);
		}
	}
	
	void removeLetter (string toRemove) {
		lettersInHand.Remove (toRemove);
		if (toRemove == doubleLetterOne) {
			doubleLetterOne = null;
			oneDouble = false;
		}
		else if (toRemove == doubleLetterTwo) {
			doubleLetterTwo = null;
			twoDoubles = false;
		}
		if (vowels.Contains (toRemove)) {
			vCount--;
		}
		else {
			cCount--;
		}
	}

	void checkLetter (string letterToCheck) {
		GameObject i = tiles.Where (x => x.GetComponent<LetterBox>().myLetter == letterToCheck.ToLower () && x.GetComponent<LetterBox>().isOn).FirstOrDefault();

		if (i != null)
			i.SendMessage ("OnMouseDown",SendMessageOptions.DontRequireReceiver);

	}

	void checkWord () {
		if (!didCheck) {
			didCheck = true;
			currentWord = "";
			
			foreach (string i in wordPartLetters)
				currentWord += i;
			
			if (wordCheck.isWord(currentWord) && (wordPartLetters.Count > 2)) {
				if (!tutorialOn || tWords > 2) {
					foreach (int i in wordPartScore)
						myScore += (i * currLevel);
					myScore += myBonus;

					Score.text = myScore.ToString ();
					LevelBar.lb.addExp();
					setInfo (wordPartScore, wordPartLetters, myBonus);
				}

				foreach (string i in wordPartLetters) 
					removeLetter (i);
				
				foreach (GameObject i in boxes) {
					tiles.Remove (i);
					i.Remove ();
				}

				tutorialImage ();
				checkSpawn();



			}

			else {
				this.gameObject.BroadcastMessage ("turnOn");
			}
			
			WordBox.wordBox.Word (wordCheck.isWord(currentWord), wordPartLetters.Count);
		}
	}
	
	void setInfo (List<int> pts, List<string> ltrs, int bonus) {
		int tempScore = 0;
		string tempWord = "";
		foreach (int i in pts)
			tempScore += i;
		
		foreach (string i in ltrs)
			tempWord += i;
		
		if (myScore > PlayerInfo.bestScore) {
			PlayerInfo.bestScore = myScore;
		}
		
		if (ltrs.Count > PlayerInfo.longestWordLength) {
			PlayerInfo.longestWordLength = ltrs.Count;
			PlayerInfo.longestWord = tempWord;
		}
		
		if (ltrs.Count > PlayerInfo.currentLongestWordLength) {
			PlayerInfo.currentLongestWordLength = ltrs.Count;
			PlayerInfo.currentLongestWord = tempWord;
		}
		
		if (((tempScore * currLevel) + bonus) > PlayerInfo.mostValuableWordValue){
			PlayerInfo.mostValuableWordValue = ((tempScore * currLevel) + bonus);
			PlayerInfo.mostValuableWord = tempWord;
		}
		
		if (((tempScore * currLevel) + bonus) > PlayerInfo.currentMostValuableWordValue){
			PlayerInfo.currentMostValuableWordValue = ((tempScore * currLevel) + bonus);
			PlayerInfo.currentMostValuableWord = tempWord;
		}
	}

	void SeeWord () {
		string word = "";
		int pts = 0;
		foreach (string i in wordPartLetters)
			word += i;
		
		foreach (int i in wordPartScore)
			pts += i;
		
		wordPrev.text = word;
		
		if ((wordPartLetters.Count > 4) && (wordPartLetters.Count < 9)){
			myBonus = 3 * currLevel;
			bonusPreview.text = "+" + myBonus;
		}
		else if (wordPartLetters.Count == 9) {
			myBonus = 10 * currLevel;
			bonusPreview.text = "+" + myBonus;
		}
		else {
			myBonus = 0;
			bonusPreview.text = "";
		}
		
		if (pts > 0 && wordPartLetters.Count >2)
			scorePrev.text = "+" + (pts * currLevel);
		else
			scorePrev.text = "";
	}

	public void addNewLetter (string Letter, int Value, GameObject Box) {
		wordPartLetters.Add (Letter);
		wordPartScore.Add (Value);
		boxes.Add (Box);
		SeeWord ();
	}

	public void GameOver () {
		if (!isOver) {
			isOver = true;
			tutorialImage ();
			WordBox.wordBox.reset ();
			LevelBar.lb.lvlText.text = "";
			LevelBar.lb.endAnimFirst ();
			wordPartLetters = new List<string>();
			wordPartScore = new List<int>();
			WordBox.wordBox.reset();
			WordBox.wordBox.gameObject.transform.localScale = new Vector3 (0,0.4f,1);
			WordBox.wordBox.turnOff();
			wordPrev.color = WordBox.wordBox.currColor;
			SeeWord ();
			Score.gameObject.SetActive (false);
			checkButton.SetActive (false);
			topScore.text = PlayerInfo.bestScore.ToString ();
			SaveManager.Save (PlayerInfo.bestScore, PlayerInfo.longestWordLength, PlayerInfo.longestWord, PlayerInfo.mostValuableWordValue, PlayerInfo.mostValuableWord);
			EndGameAnimation.ega.startAnims ();
			StartCoroutine (waitandAnimate ());
		}
	}

	void restartProcess () {
		tutorialOn = false;
		BroadcastMessage ("die");

		tiles = new List<GameObject>();
		boxes = new List<GameObject>();
		lettersInHand = new List<string>();
		gamePrep();
		WordBox.wordBox.gameObject.transform.localScale = new Vector3 (0,0.4f,1);
		Score.gameObject.SetActive (true);
		checkButton.SetActive (true);
		checkSpawn ();
	}

	IEnumerator waitandAnimate () {
		yield return new WaitForSeconds(0.15f);
		BroadcastMessage ("endAnims");
	}
	
	IEnumerator waitandEndingAnimate () {
		yield return new WaitForSeconds(0.05f);
		LevelBar.lb.endAnimFirst();
	}

	public void undo () {
		wordPartLetters.RemoveAt (wordPartLetters.Count - 1);
		wordPartScore.RemoveAt (wordPartScore.Count - 1);
		boxes[boxes.Count - 1].GetComponent<LetterBox>().turnOn ();
		boxes.RemoveAt (boxes.Count - 1);
		SeeWord ();
	}
	
	public void resetWord () {
		wordPartLetters = new List<string>();
		wordPartScore = new List<int>();
		SeeWord ();
		boxes = new List<GameObject>();
		BroadcastMessage ("turnOn");
		didCheck = false;
	}
	
	void restartAnim () {
		BroadcastMessage ("restartAnims");
		EndGameAnimation.ega.resetAnim();
		StartCoroutine (waitandEndingAnimate());
	}
	
	public void restart () {
		LevelBar.lb.Start ();
		restartProcess();
	}
	
	void quit () {
		Application.Quit();
	}

	void tutorialSetUp () {
		tutorialOn = true;
		tWords = 0;
		tutorialText.text = tutorialGraphics[tWords];
	}
	public void tutorialImage () {
		tWords++;
		if (tutorialOn) {
			if (tWords >= 0 && tWords < 3) 
				tutorialText.text = tutorialGraphics[0];

			else if (tWords >= 3 && tWords < 6) 
				tutorialText.text = tutorialGraphics[1];

			else if (tWords >= 6 && !isOver) 
				tutorialText.text = tutorialGraphics[2];

			else if (isOver) 
				tutorialText.text = tutorialGraphics[3];

		}
		else {
			tutorialText.text = "";
		}
	}

	IEnumerator backToMainMenu () {
		iTween.Stop ();
		yield return new WaitForEndOfFrame();
		Application.LoadLevel(0);
	}
}
