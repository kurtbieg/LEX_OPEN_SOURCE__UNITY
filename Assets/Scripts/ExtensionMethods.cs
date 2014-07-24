using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


internal static class ExtensionMethods 
{

	public static void Swap<T>(this IList<T> list, int indexA, int indexB)
	{
		T tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
	}

	public static void Remove (this GameObject letterTile) 
	{
		letterTile.transform.localPosition = new Vector3(0,0,0);
		letterTile.GetComponent<LetterBox>().Start ();
		Master.master.tiles.Remove (letterTile);
	}
}
