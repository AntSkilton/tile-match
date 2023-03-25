using System.Collections.Generic;
using UnityEngine;

public enum TileColours
{
	Red,
	Blue,
	Green,
	Yellow,
	Orange
}

[CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObject/Level", order = 1)]
public class Level : ScriptableObject
{
	public int LevelNumber = 0;

	[Range(5,9)]
	public int Rows = 5;

	[Range(5,9)]
	public int Columns = 5;

	public List<Tile> TileColours = new List<Tile>();
}