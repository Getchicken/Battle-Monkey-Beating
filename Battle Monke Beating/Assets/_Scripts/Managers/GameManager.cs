using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public UiManager um;
    private float delayTime = 1.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // set the ui manager state to game 
        um.GameUI();
        // Set the _player's position to the spawn point position
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Vector3.zero;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        um = GameObject.FindObjectOfType<UiManager>();
        um.EnablePlayerScripts();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void CallOther()
    {
        if (um == null)
        {
            print("no canvasUI assigned");
            return;
        }

        um.WinUI();
        Invoke("CompleteLevel", delayTime);
    }

    public void SetPositionToSpawn()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Vector3.zero;
    }
}
