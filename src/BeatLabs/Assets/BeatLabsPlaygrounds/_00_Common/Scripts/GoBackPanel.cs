using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackPanel : MonoBehaviour
{
  public void Start()
  {
    GetComponent<Canvas>().worldCamera = Camera.main;
  }

  public void OnClick_GoBackButton()
  {
    Debug.Log("Click GoBack!");

    SceneManager.LoadScene("_00_PlaygroundSelectorScene", LoadSceneMode.Single);
  }
}
