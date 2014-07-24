using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The MIT License (MIT)
//	
//Copyright (c) 2014, Simple Machine LLC
//		
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
//
//English dictionary from http://dreamsteep.com/projects/the-english-open-word-list.html
//The “English Open Word List” (EOWL) was developed by Ken Loge, but is almost entirely 
//derived from the “UK Advanced Cryptics Dictionary” (UKACD) Version 1.6, by J Ross Beresford.
//
//UK Advanced Cryptics Dictionary Licensing Information:
//
//Copyright © J Ross Beresford 1993-1999. All Rights Reserved. 
//The following restriction is placed on the use of this publication: 
//if the UK Advanced Cryptics Dictionary is used in a software package 
//or redistributed in any form, the copyright notice must be prominently 
//displayed and the text of this document must be included verbatim.

public class wordCheck : MonoBehaviour {

	public string defaultLanguage="en";
	private static Dictionary<string, bool> wordList;
	
	void Start () {
		setLanguage(defaultLanguage);
	}
	
	public static bool isWord(string word){
		return wordList.ContainsKey(word.ToLower());
	}
	
	public static void setLanguage(string res){
		char[] archDelims = new char[] { '\n', '\r' };

		string[] words = (Resources.Load("Dictionary/" + res) as TextAsset).text.Split(archDelims, System.StringSplitOptions.RemoveEmptyEntries);
		wordList = new Dictionary<string,bool>();
		foreach(string word in words)
			wordList.Add(word.Trim(),true);
	}

	public static void setLanguage(List<string> res){
		char[] archDelims = new char[] { '\n', '\r' };
		wordList = new Dictionary<string,bool>();

		foreach (string i in res) {
			string[] words = (Resources.Load("Dictionary/" + i.ToLower ()) as TextAsset).text.Split(archDelims, System.StringSplitOptions.RemoveEmptyEntries);
			foreach(string word in words)
				if (wordList.ContainsKey (word))
				    Debug.Log (i + ", " + word);
				else
					wordList.Add(word.Trim(),true);
			}
	}
}