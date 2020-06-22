using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
  public void OnStartGame()
  {
    SceneManager.LoadScene("GambitLand");
  }
}
