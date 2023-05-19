using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EG
{
    public class Respawn : MonoBehaviour
    {
        public void LevelReset()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
