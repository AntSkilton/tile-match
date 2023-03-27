using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObject/Level", order = 1)]
public class LevelData : ScriptableObject
{
	public Guid Id;
	
	public int LevelNumber;

	[Range(3,9)]
	public int RowColumnGridCount = 5;
	public int TilesToPopTarget;
	public int StartingMovesQuantity;

	public List<GameObject> TileItemPrefabs = new List<GameObject>();
	
	private void OnEnable()
	{
		Id = Guid.NewGuid();
	}
}