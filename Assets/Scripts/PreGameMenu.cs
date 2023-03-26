using UnityEngine;

public class PreGameMenu : MonoBehaviour
{
	public GameObject LevelButtonPrefab;
	public Transform LevelContainer;
	
	private void Start()
	{
		DisplayAllLevels();
	}

	private void DisplayAllLevels()
	{
		for (int i = 0; i < GameManager.Instance.Levels.Count; i++)
		{
			var button = Instantiate(LevelButtonPrefab, LevelContainer);
			
			var component = button.GetComponent<LevelSelectButton>();
			component.LevelLabel.text = GameManager.Instance.Levels[i].LevelNumber.ToString();
			component.ReferencedLevelId = GameManager.Instance.Levels[i].Id;
		}
	}
}