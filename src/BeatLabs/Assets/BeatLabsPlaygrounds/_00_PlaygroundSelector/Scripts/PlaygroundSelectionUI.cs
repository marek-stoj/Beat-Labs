using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaygroundSelectionUI : MonoBehaviour
{
  public void OnClick_Button_10_DataBinding()
  {
    Debug.Log("Click 10_DataBinding!");

    SceneManager.LoadScene("_10_DataBindingScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_09_UI()
  {
    Debug.Log("Click 09_UI!");

    SceneManager.LoadScene("_09_UIScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_08_SeatBaber()
  {
    Debug.Log("Click 08_SeatBaber!");

    SceneManager.LoadScene("_08_SeatBaberScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_06_Sabers()
  {
    Debug.Log("Click _06_Sabers!");

    SceneManager.LoadScene("_06_SabersScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_05_BlockSpawning()
  {
    Debug.Log("Click _05_BlockSpawning!");

    SceneManager.LoadScene("_05_BlockSpawningScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_04_BeatSaberMaps()
  {
    Debug.Log("Click _04_BeatSaberMaps!");

    SceneManager.LoadScene("_04_BeatSaberMapsScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_03_KoreoMidi()
  {
    Debug.Log("Click _03_KoreoMidi!");

    SceneManager.LoadScene("_03_KoreoMidiScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_02_KoreographerPlayground()
  {
    Debug.Log("Click _02_KoreographerPlayground!");

    SceneManager.LoadScene("_02_KoreographerPlaygroundScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_01_XRRig()
  {
    Debug.Log("Click _01_XRRig!");

    SceneManager.LoadScene("_01_XRRigScene", LoadSceneMode.Single);
  }

  public void OnClick_Button_XX_BeaVeR()
  {
    Debug.Log("Click XX_BeaVeR!");

    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
  }
}
