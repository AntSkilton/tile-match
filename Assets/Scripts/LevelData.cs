using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum TileColours
{
	Red,
	Blue,
	Green,
	Yellow,
	Orange
}

[CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObject/Level", order = 1)]
public class LevelData : ScriptableObject
{
	public Guid Id;
	
	public int LevelNumber;

	[Range(5,9)]
	public int RowColumnGridCount = 5;
	public int TilesToPopTarget;
	public int StartingMovesQuantity;

	public List<GameObject> TileItemPrefabs = new List<GameObject>();
	
	private void OnEnable()
	{
		Id = Guid.NewGuid();
	}
}