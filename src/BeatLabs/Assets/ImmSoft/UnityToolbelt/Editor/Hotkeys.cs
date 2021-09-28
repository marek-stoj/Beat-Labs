using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ImmSoft.UnityToolbelt.Editor
{
  public class Hotkeys : ScriptableObject
  {
    private static MethodInfo _clearConsoleMethod;

    [MenuItem(Consts.MenuPath + "Save All #%&s")]
    public static void SaveAll()
    {
      AssetDatabase.SaveAssets();
      EditorSceneManager.SaveOpenScenes();
      EditorApplication.ExecuteMenuItem("File/Save Project");

      Debug.Log("Saved All");
    }

    [MenuItem(Consts.MenuPath + "Clear Console #%&c")]
    public static void ClearConsole()
    {
      ClearConsoleMethod.Invoke(new object(), null);
    }

    private static MethodInfo ClearConsoleMethod
    {
      get
      {
        if (_clearConsoleMethod == null)
        {
          Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
          Type logEntries = assembly.GetType("UnityEditor.LogEntries");

          _clearConsoleMethod = logEntries.GetMethod("Clear");
        }

        return _clearConsoleMethod;
      }
    }
  }
}
