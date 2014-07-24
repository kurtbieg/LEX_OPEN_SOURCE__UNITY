using UnityEngine;
using System.Collections;

public class WordBox : MonoBehaviour {

	public static WordBox wordBox;
	public TextMesh wordText;
	public GameObject rPart, lPart;
	public Color currColor;
	public Vector2 tempV3, tempV2;
	public bool mayReset, isClicked;


	void Start () {
		wordBox = this;
		isClicked = false;
		currColor = wordText.color;
		move ();
		turnOff ();
	}
	
	public void OnMouseDown () {
		if (!Master.master.menuOpen && !Master.master.didCheck) {
			iTween.StopByName ("sally");
			tempV3 = Input.mousePosition;
			mayReset = true;
			ScalePunch ();
			isClicked = true;
		}
	}

	void Update () {
		
		tempV2 = Input.mousePosition;
		if (Input.GetMouseButtonUp(0) && mayReset){
			iTween.StopByName ("sally");
			if(Vector2.Distance(Input.mousePosition, tempV3) > 100) {
				wordReset ();
				resetPos ();
				reset ();
			}
			else {
				backSpace ();
			}
			mayReset = false;
			isClicked = false;
		}
		
		if (Input.GetMouseButton(0) && mayReset) {
			if (Vector2.Distance(Input.mousePosition, tempV3) > 100) {
				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				transform.position = Vector2.Lerp(transform.position, new Vector2 (mousePosition.x, transform.position.y), Time.deltaTime * 5f);
				
				wordText.transform.position = Vector3.Lerp (wordText.transform.position, new Vector3 (mousePosition.x, wordText.transform.position.y, wordText.transform.position.z), Time.deltaTime * 5f);
				move ();
			}
			
		}
	}

	void ScalePunch () {
		if (wordText.renderer.bounds.size.x > 0)
			iTween.PunchScale(gameObject, iTween.Hash ("name", "sally", "x", 0.15f, "easeType", "easeOutElastic", "onupdate", "move", "time", 0.25f));
	}

	void resetPos (string oncomplete = null) {
		if (oncomplete == null)
			iTween.MoveTo(gameObject, iTween.Hash ("x", 0f, "onupdate", "move", "time", 0.25f));
		else
			iTween.MoveTo(gameObject, iTween.Hash ("x", 0f, "onupdate", "move", "time", 0.25f, "oncomplete", oncomplete));
	}

	void wordReset () {
		iTween.MoveTo(wordText.gameObject, iTween.Hash ("x", 0f, "time", 0.25f));
	}
	
	public void grow () {
		turnOn ();
		iTween.ScaleTo(gameObject, iTween.Hash ("x", wordText.renderer.bounds.size.x * 0.25f, "easeType", "easeOutElastic", "onupdate", "move", "time", 0.25f, "oncomplete", "checkEnds"));
		//Debug.Log (wordText.renderer.bounds.size.x.ToString ());
	}
	
	public void shrink () {	
		iTween.ScaleTo(gameObject, iTween.Hash ("x", wordText.renderer.bounds.size.x * 0.25f, "easeType", "easeOutElastic", "onupdate", "move", "time", 0.25f, "oncomplete", "checkEnds"));
	}
	
	public void reset () {
		if (wordText.renderer.bounds.size.x != 0f) {
			Master.master.resetWord ();
			iTween.ScaleTo(gameObject, iTween.Hash ("x", 0f, "onupdate", "move", "time", 0.025f, "oncomplete", "shrinkEnds"));
			wordText.color = currColor;
		}
	}

	public void Word (bool isWord, int lengthCount) {
		if (isWord && lengthCount > 2) {
			wordText.color = Color.green;
			iTween.PunchPosition(wordText.gameObject, iTween.Hash ("y", 1f, "easeType", "easeInOutElastic", "time", 0.25f, "oncompletetarget", gameObject, "oncomplete", "reset"));
		}
		else {
			wordText.color = Color.red;
			iTween.PunchPosition(wordText.gameObject, iTween.Hash ("y", -1f, "easeType", "easeInOutElastic", "time", 0.25f, "oncompletetarget", gameObject, "oncomplete", "reset"));
		}
	}

	void move () {
		rPart.transform.position = new Vector3 (gameObject.transform.renderer.bounds.max.x - 0.05f, 2.575241f, 0);
		lPart.transform.position = new Vector3 (gameObject.transform.renderer.bounds.min.x + 0.05f, 2.575241f, 0);
	}

	void shrinkEnds () {
		iTween.ValueTo (gameObject, iTween.Hash ("name", "tom", "from", 1f, "to", 0f, "time", 0.25f, "onupdate", "endScale", "easetype", "easeOutExpo", "oncomplete", "turnOff"));
	}

	void checkEnds () {
		if (lPart.transform.localScale != new Vector3 (1,1,1) || rPart.transform.localScale != new Vector3 (1,1,1)) {
			lPart.transform.localScale = new Vector3 (-1,1,1);
			rPart.transform.localScale = new Vector3 (1,1,1);
		}
	}
	
	void endScale (float scaleValue) {
		lPart.transform.localScale = new Vector3 (-1f * scaleValue, scaleValue, 1f);
		rPart.transform.localScale = new Vector3 (scaleValue, scaleValue, 1f);
	}

	public void backSpace () {
		if (Master.master.boxes.Count > 1) {
			Master.master.undo ();
			wordReset ();
			resetPos ();
			shrink();
		}
		else if (Master.master.boxes.Count <= 1) {
			Master.master.resetWord ();
			wordReset ();
			resetPos ();
			reset();
		}
	}

	public void turnOff () {
		lPart.SetActive (false);
		rPart.SetActive (false);
		iTween.StopByName ("tom");

	}

	void turnOn () {
		iTween.StopByName ("tom");
		rPart.transform.localScale = new Vector3 (1,1,1);
		lPart.transform.localScale = new Vector3 (-1,1,1);
		lPart.SetActive (true);
		rPart.SetActive (true);
	}
}
