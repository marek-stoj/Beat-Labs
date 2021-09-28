using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeaVeR.UI
{
  public class NewGoBackPanel : MonoBehaviour
  {
    public void OnClick_GoBackButton()
    {
      Debug.Log("Click GoBack!");

      SceneManager.LoadScene("_00_PlaygroundSelectorScene", LoadSceneMode.Single);
    }
  }
}
