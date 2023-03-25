using UnityEngine;

public class PreGameMenu : MonoBehaviour
{
	public GameObject LevelButtonPrefab;
	public Transform LevelContainer;
	
	private void Awake()
	{
		DisplayAllLevels();
	}

	private void DisplayAllLevels()
	{
		for (int i = 0; i < GameManager.Instance.Levels.Count; i++)
		{
			var button = Instantiate(LevelButtonPrefab, LevelContainer);
			button.GetComponent<LevelButton>().LevelLabel.text = GameManager.Instance.Levels[i].LevelNumber.ToString();
		}
	}
}