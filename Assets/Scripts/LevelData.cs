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
	[HideInInspector]
	public GUID Id;
	
	public int LevelNumber;

	[FormerlySerializedAs("RowColumnSquareGridCount")] [Range(5,9)]
	public int RowColumnGridCount = 5;
	public int TilesToPopTarget;
	public int StartingMovesQuantity;

	public List<Tile> TileColours = new List<Tile>();
	
	private void OnEnable()
	{
		Id = GUID.Generate();
	}
}