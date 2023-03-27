using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public List<LevelData> Levels;

	[HideInInspector] public LevelData CurrentLevel;
	
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

	private void Start()
	{
		SceneManager.LoadScene(sceneBuildIndex: 1, LoadSceneMode.Additive);
	}
	
	public void SetLevel(Guid referencedLevelId)
	{
		for (int i = 0; i < Instance.Levels.Count; i++)
		{
			if (Instance.Levels[i].Id == referencedLevelId)
			{
				CurrentLevel = Instance.Levels[i];
				break;
			}
		}
	}
}