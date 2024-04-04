using UnityEngine;
using UnityEngine.InputSystem;

public static class BindPrefs
{
	public static void SaveBind(InputAction action)
	{
		var bind = action.SaveBindingOverridesAsJson();
		PlayerPrefs.SetString(action.name, bind);
		PlayerPrefs.Save();
	}
	
	public static void LoadBind(InputAction action)
	{
		var bind = PlayerPrefs.GetString(action.name);
		action.LoadBindingOverridesFromJson(bind);
	}

	public static void DeleteAll() => PlayerPrefs.DeleteAll();
}
