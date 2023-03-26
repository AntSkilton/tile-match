using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }
	
	public TextMeshProUGUI LevelNumberLabel;
	public TextMeshProUGUI TilesToPopRemainingLabel;
	public TextMeshProUGUI MovesRemainingLabel;
	
	public GameObject TileGridContainer;
	public GameObject TileSlot;
	private Dictionary<Vector2, TileSlot> m_tileGrid = new Dictionary<Vector2, TileSlot>();

	private void Awake() 
	{ 
		if (Instance != null && Instance != this) 
		{ 
			Destroy(this); 
		} 
		else 
		{ 
			Instance = this; 
		} 
	}

	private void OnEnable()
	{
		LoadLevelData();
		// populate grid randomly
	}

	private void LoadLevelData()
	{
		LevelNumberLabel.text = $"Level: {GameManager.Instance.CurrentLevel.LevelNumber}";
		TilesToPopRemainingLabel.text = $"Tiles to pop: {GameManager.Instance.CurrentLevel.TilesToPopTarget}";
		MovesRemainingLabel.text = $"Moves remaining: {GameManager.Instance.CurrentLevel.StartingMovesQuantity}";
		
		LoadTileSlots();
	}

	private void LoadTileSlots()
	{
		TileGridContainer.GetComponent<GridLayoutGroup>().constraintCount = GameManager.Instance.CurrentLevel.RowColumnGridCount;

		for (int i = 0; i < GameManager.Instance.CurrentLevel.RowColumnGridCount; i++)
		{
			for (int j = 0; j < GameManager.Instance.CurrentLevel.RowColumnGridCount; j++)
			{
				var slotObj = Instantiate(TileSlot, TileGridContainer.transform);
				var slot = slotObj.GetComponent<TileSlot>();
				slot.Coordinates = new Vector2(i, j);
				m_tileGrid.Add(slot.Coordinates, slot);
			}
		}
	}
}