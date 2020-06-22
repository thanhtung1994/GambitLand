using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class PlayerController : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag.Equals("Gate"))
            {
                GameManager.Instance.NextMap();
                SceneManager.LoadScene("GambitLand");
            }
            else if (other.gameObject.tag.Equals("GateTutorial"))
            {
                SceneManager.LoadScene("GambitLand");
            }
            else if (other.gameObject.tag.Equals("Chest"))
            {
            }
            else if (other.gameObject.tag.Equals("Health"))
            {
                GameManager.Instance.currentdataplayer.AddHealth(3);
            }
        }
    }
}