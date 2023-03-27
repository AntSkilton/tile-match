using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }
	
	public TextMeshProUGUI LevelNumberLabel;
	public TextMeshProUGUI TilesToPopRemainingLabel;
	public TextMeshProUGUI MovesRemainingLabel;
	
	public GameObject TileGridContainer;
	public GameObject TileSlotPrefab;
	
	private Dictionary<Vector2, TileSlot> m_tileGrid = new Dictionary<Vector2, TileSlot>();
	private int m_movesRemaining;
	private int m_tilesToPopRemaining;

	public GameObject WinLosePopup;
	public TextMeshProUGUI WinLoseTextLabel;

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
		TileItem.RegisterTileToPop += OnClickTilePop;
		
		LoadLevelData();
		PopulateEmptySlots();
	}

	private void OnDisable()
	{
		TileItem.RegisterTileToPop -= OnClickTilePop;
	}

	private void LoadLevelData()
	{
		m_movesRemaining = GameManager.Instance.CurrentLevel.StartingMovesQuantity;
		m_tilesToPopRemaining = GameManager.Instance.CurrentLevel.TilesToPopTarget;
		
		LevelNumberLabel.text = $"Level: {GameManager.Instance.CurrentLevel.LevelNumber}";
		TilesToPopRemainingLabel.text = $"Tiles to pop: {m_tilesToPopRemaining}";
		MovesRemainingLabel.text = $"Moves remaining: {m_movesRemaining}";
		
		LoadTileSlots();
	}

	private void LoadTileSlots()
	{
		TileGridContainer.GetComponent<GridLayoutGroup>().constraintCount = GameManager.Instance.CurrentLevel.RowColumnGridCount;

		for (int i = 0; i < GameManager.Instance.CurrentLevel.RowColumnGridCount; i++)
		{
			for (int j = 0; j < GameManager.Instance.CurrentLevel.RowColumnGridCount; j++)
			{
				var slotObj = Instantiate(TileSlotPrefab, TileGridContainer.transform);
				var slot = slotObj.GetComponent<TileSlot>();
				slot.Coordinates = new Vector2(j, i); // j=x axis, i=y axis
				m_tileGrid.Add(slot.Coordinates, slot);
			}
		}
	}

	private void PopulateEmptySlots()
	{
		foreach (var slot in m_tileGrid)
		{
			if (slot.Value.TileItem == null)
			{
				var itemPrefab =
					GameManager.Instance.CurrentLevel.TileItemPrefabs[
						Random.Range(0, GameManager.Instance.CurrentLevel.TileItemPrefabs.Count)];

				var obj = Instantiate(itemPrefab, slot.Value.gameObject.transform);
				var tileItem = obj.GetComponent<TileItem>();
				
				tileItem.SetSlot(slot.Value);
				slot.Value.TileItem = tileItem;
			}
		}
	}
	
	private void OnClickTilePop(Vector2 selectedTileSlot)
	{
		List<TileItem> tilesToPop = new List<TileItem>();
		tilesToPop.Add(m_tileGrid[selectedTileSlot].TileItem);
		List<TileItem> prevTilesDiscovered = new List<TileItem>();

		// Discover all neighbours and iterate until all eligible tiles to pop have been gathered.
		while (prevTilesDiscovered.Count != tilesToPop.Count)
		{
			prevTilesDiscovered.Clear();
			foreach (var tile in tilesToPop)
			{
				prevTilesDiscovered.Add(tile);
			}
			
			tilesToPop.Clear();
			tilesToPop.Add(m_tileGrid[selectedTileSlot].TileItem); // Re-add the originally selected slot

			foreach (var tile in prevTilesDiscovered)
			{
				var slot = tile.ParentSlot;
			
				// Check Up Tile
				var upSlot = new Vector2(slot.Coordinates.x, slot.Coordinates.y + 1);
				if (CanConsiderTile(upSlot, selectedTileSlot, tilesToPop))
				{
					tilesToPop.Add(m_tileGrid[upSlot].TileItem);
				}
				
				// Check Down Tile
				var downSlot = new Vector2(slot.Coordinates.x, slot.Coordinates.y - 1);
				if (CanConsiderTile(downSlot, selectedTileSlot, tilesToPop))
				{
					tilesToPop.Add(m_tileGrid[downSlot].TileItem);
				}
				
				// Check Right
				var rightSlot = new Vector2(slot.Coordinates.x + 1, slot.Coordinates.y);
				if (CanConsiderTile(rightSlot, selectedTileSlot, tilesToPop))
				{
					tilesToPop.Add(m_tileGrid[rightSlot].TileItem);
				}
				
				// Check Left
				var leftSlot = new Vector2(slot.Coordinates.x - 1, slot.Coordinates.y);
				if (CanConsiderTile(leftSlot, selectedTileSlot, tilesToPop))
				{
					tilesToPop.Add(m_tileGrid[leftSlot].TileItem);
				}
			}
		}
		
		// When no new neighbours found, pop all tiles
		foreach (var tileItem in tilesToPop)
		{
			m_tilesToPopRemaining--;
			//tileItem.PopTile();
			Destroy(m_tileGrid[tileItem.ParentSlot.Coordinates].TileItem.gameObject);
			m_tileGrid[tileItem.ParentSlot.Coordinates].TileItem = null;
		}
		
		m_movesRemaining--;
		CalculateWinLoseCondition();
		UpdateHeaderView();
		PopulateEmptyTiles();
	}

	private void CalculateWinLoseCondition()
	{
		if (m_tilesToPopRemaining < 1)
		{
			WinLosePopup.gameObject.SetActive(true);
			WinLoseTextLabel.text = "You Win!";
			return;
		}

		if (m_movesRemaining < 1)
		{
			WinLosePopup.SetActive(true);
			WinLoseTextLabel.text = "You Lose :(";
		}
	}

	private void PopulateEmptyTiles()
	{
		foreach (var slot in m_tileGrid)
		{
			if (slot.Value.TileItem != null) continue;
			
			// Inject a new prefab then attach it to the previously null slot
			var itemPrefab =
				GameManager.Instance.CurrentLevel.TileItemPrefabs[
					Random.Range(0, GameManager.Instance.CurrentLevel.TileItemPrefabs.Count)];
			
			var obj = Instantiate(itemPrefab,  m_tileGrid[slot.Key].gameObject.transform);
			var tileItem = obj.GetComponent<TileItem>();
				
			tileItem.SetSlot(slot.Value);
			slot.Value.TileItem = tileItem;
		}
	}

	private bool CanConsiderTile(Vector2 slotPosToCheck, Vector2 selectedTileSlot, List<TileItem> tilesToPop)
	{
		if (IsSlotWithinBounds(slotPosToCheck))
		{
			if (DoTilesMatch(m_tileGrid[slotPosToCheck], m_tileGrid[selectedTileSlot]))
			{
				if (!DoesItemExistInList(tilesToPop, m_tileGrid[slotPosToCheck].Coordinates))
				{
					return true;
				}
			}
		}

		return false;
	}

	private static bool DoTilesMatch(TileSlot tilePosSource, TileSlot tilePosToCheck)
	{
		return tilePosSource.TileItem.TileType == tilePosToCheck.TileItem.TileType;
	}

	private static bool DoesItemExistInList(List<TileItem> items, Vector2 coordinatesToCheck)
	{
		foreach (var item in items)
		{
			if (item.ParentSlot.Coordinates == coordinatesToCheck)
			{
				return true;
			}
		}

		return false;
	}

	private static bool IsSlotWithinBounds(Vector2 slotToCheck)
	{
		if (slotToCheck.x >= 0 && slotToCheck.x <= GameManager.Instance.CurrentLevel.RowColumnGridCount -1)
		{
			if (slotToCheck.y >= 0 && slotToCheck.y <= GameManager.Instance.CurrentLevel.RowColumnGridCount -1)
			{
				return true;
			}
		}

		return false;
	}

	private void UpdateHeaderView()
	{
		TilesToPopRemainingLabel.text = $"Tiles to pop: {m_tilesToPopRemaining}";
		MovesRemainingLabel.text = $"Moves remaining: {m_movesRemaining}";
	}

	public void OnClickReturnToMenu()
	{
		SceneManager.LoadScene(sceneBuildIndex: 1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(sceneBuildIndex: 2);
	}
}