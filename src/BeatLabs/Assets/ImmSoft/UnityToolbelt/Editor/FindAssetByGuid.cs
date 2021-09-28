using UnityEditor;
using UnityEngine;

namespace ImmSoft.UnityToolbelt.Editor
{
  internal class UnityToolbeltMenu : EditorWindow
  {
    private static UnityToolbeltMenu _window;

    [MenuItem(Consts.MenuPath + "Find Asset by GUID")]
    public static void Init()
    {
      FindAssetByGuid();

      // if (_window == null)
      // {
      //   _window = CreateInstance<UnityToolbeltMenu>();
      //   _window.titleContent = new GUIContent("Unity Toolbelt");
      // }
      //
      // _window.ShowUtility();
    }

    public static void FindAssetByGuid()
    {
      //string path = AssetDatabase.GUIDToAssetPath(new GUID("83d9acc7968244a8886f3af591305bcb"));
      string path = AssetDatabase.GUIDToAssetPath(new GUID("c9f956787b1d945e7b36e0516201fc76"));

      Debug.Log($"Asset path: {path}.");
    }
  }
}
