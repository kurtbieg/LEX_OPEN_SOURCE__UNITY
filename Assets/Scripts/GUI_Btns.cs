using UnityEngine;
using System.Collections;

public class GUI_Btns : MonoBehaviour {
	void OnMouseDown () {
		scaleUp ();
		Master.master.GUIstuff (gameObject.name);
	}

	void scaleUp () {
		iTween.PunchScale(gameObject, new Vector3(0.5f,0.5f,0), 0.5f);
	}
}
