using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
	public TextMeshProUGUI LevelLabel;
	[HideInInspector] public GUID ReferencedLevelId;
	
	public void OnClickLoadLevel()
	{
		GameManager.Instance.SetLevel(ReferencedLevelId); // Set level data to load
		SceneManager.LoadScene(sceneBuildIndex: 2, LoadSceneMode.Additive); // Load Level scene
		SceneManager.UnloadSceneAsync(sceneBuildIndex: 1); // Unload PreGameMenu
	}
}