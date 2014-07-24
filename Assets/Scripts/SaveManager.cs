using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	public static void Save (int BestScore, int LongestWordLength, string LongestWord, int MostValuableWordValue, string MostValuableWord) {

		PlayerPrefs.SetInt("BestScore", BestScore);
		PlayerPrefs.SetInt("LongestWordLength", LongestWordLength);
		PlayerPrefs.SetString("LongestWord", LongestWord);
		PlayerPrefs.SetInt("MostValuableWordValue", MostValuableWordValue);
		PlayerPrefs.SetString("MostValuableWord", MostValuableWord);
	}

	public static void Load () {

		PlayerInfo.bestScore = PlayerPrefs.GetInt("BestScore");

		PlayerInfo.longestWordLength = PlayerPrefs.GetInt("LongestWordLength");
		PlayerInfo.longestWord = PlayerPrefs.GetString("LongestWord");

		PlayerInfo.mostValuableWordValue = PlayerPrefs.GetInt("MostValuableWordValue");
		PlayerInfo.mostValuableWord = PlayerPrefs.GetString("MostValuableWord");
	}

}