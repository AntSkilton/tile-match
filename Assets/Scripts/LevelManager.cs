using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }
	
	public TextMeshProUGUI LevelNumberLabel;
	public TextMeshProUGUI TilesToPopRemainingLabel;
	public TextMeshProUGUI MovesRemainingLabel;
	
	public GameObject TileGridContainer;
	public GameObject TileSlotPrefab;
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
		PopulateEmptySlots();
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
				var slotObj = Instantiate(TileSlotPrefab, TileGridContainer.transform);
				var slot = slotObj.GetComponent<TileSlot>();
				slot.Coordinates = new Vector2(i, j);
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

				Instantiate(itemPrefab, slot.Value.gameObject.transform);
				slot.Value.TileItem = itemPrefab.GetComponent<TileItem>();
			}
		}
		
	}

	public void OnClickReturnToMenu()
	{
		SceneManager.LoadScene(sceneBuildIndex: 1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(sceneBuildIndex: 2);
	}
}