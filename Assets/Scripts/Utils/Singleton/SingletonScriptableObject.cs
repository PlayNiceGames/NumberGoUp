using System.Linq;
using UnityEngine;

/// <summary>
/// Abstract class for making singletons out of ScriptableObjects
/// Returns the asset inside of Resources folder or null if there is none
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Resources.LoadAll<T>("").FirstOrDefault();
			}
			return _instance;
		}
	}
}