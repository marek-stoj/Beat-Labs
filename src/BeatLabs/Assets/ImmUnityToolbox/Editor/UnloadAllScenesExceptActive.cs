using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class UnloadAllScenesExceptActive
{
  static UnloadAllScenesExceptActive()
  {
    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
  }

  private static void OnPlayModeStateChanged(PlayModeStateChange state)
  {
    switch (state)
    {
      case PlayModeStateChange.ExitingEditMode:
      {
        Debug.Log("Unloading all scenes except the active one.");

        void CloseSceneWithoutRemoving(Scene scene) =>
          EditorSceneManager.CloseScene(scene, removeScene: false);

        ForEachInactiveSceneDo(CloseSceneWithoutRemoving);

        break;
      }

      case PlayModeStateChange.EnteredEditMode:
      {
        Debug.Log("Loading all scenes back.");

        void OpenSceneAdditive(Scene scene) =>
          EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);

        Debug.Log("STOPPING");

        ForEachInactiveSceneDo(OpenSceneAdditive);

        break;
      }
    }
  }

  private static void ForEachInactiveSceneDo(Action<Scene> action)
  {
    Scene activeScene = SceneManager.GetActiveScene();

    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
      Scene scene = SceneManager.GetSceneAt(i);

      if (scene.path != activeScene.path)
      {
        action(scene);
      }
    }
  }
}
