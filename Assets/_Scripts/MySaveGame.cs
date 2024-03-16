using System.Collections.Generic;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using UnityEngine;

public class MySaveGame : MonoBehaviour
{
	public static MySaveGame instance;
    
	private enum SaveFormat
	{
		XML,
	
		Json,
	
		Binary
	}


	[Header("Settings")] 
	[Space]


	[SerializeField] private string path = "";

	[SerializeField] private bool encode = false;

	[SerializeField] private SaveFormat format = SaveFormat.Json;

	private ISaveGameSerializer _serializer;

	[SerializeField] private SaveGamePath savePath = SaveGamePath.PersistentDataPath;

	[SerializeField] private bool dontDestroyOnLoad;


	[Header ( "Save Events" )]
	[Space]


	public bool saveOnApplicationQuit = true;

	public bool saveOnApplicationPause;

	protected virtual void Awake ()
	{
		if (instance == null) instance = this;
		if (dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
	
		switch ( format )
		{
			case SaveFormat.Binary:
				_serializer = new SaveGameBinarySerializer ();
				break;
			case SaveFormat.Json:
				_serializer = new SaveGameJsonSerializer ();
				break;
			case SaveFormat.XML:
				_serializer = new SaveGameXmlSerializer ();
				break;
		}
	}

	protected virtual void OnApplicationQuit ()
	{
		if ( saveOnApplicationQuit )
		{
			LevelManager.instance.SaveUserLevel();
		}
	}

	protected virtual void OnApplicationPause ()
	{
		if ( saveOnApplicationPause )
		{
			LevelManager.instance.SaveUserLevel();
		}
	}

	public virtual void LevelSave(List<Level> levels)
	{
		if (levels.Count == 0)
		{
			return;
		}
		SaveGame.Save<List<Level>> (
			path + "data", 
			levels, 
			encode,
			_serializer,
			savePath);
	}

	public List<Level> LevelLoad()
	{
		var levels = SaveGame.Load<List<Level>> ( 
			path + "data", 
			new List<Level>(), 
			encode,
			_serializer,
			savePath);

		return levels;
	}
}
