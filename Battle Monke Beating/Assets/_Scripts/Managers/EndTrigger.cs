using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    private int nextSceneIndex;

    public GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = Object.FindObjectOfType<GameManager>();
            //Debug.Log("GameManager object found: " + (gameManager != null));
        }
        else return;
    }
    public void OnTriggerEnter(Collider other)
    {
        gameManager = Object.FindObjectOfType<GameManager>();
        //Debug.Log("GameManager object found: " + (gameManager != null));

        // Check if the collision is with the _player
        if (other.gameObject.tag == "PlayerObj")
        {
            if(DoesNextSceneExist())
            {
                // Load the next scene if there is one
                gameManager.CallOther();
            }
            else
            {
                Debug.Log("The next Scene does not exist, please add it to the build settings");
            }
        }
        else
        {
            Debug.Log("GameObject doesnt have Player tag or PlayerStats script");
        }
    }

    public bool DoesNextSceneExist()
    {
        int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        return nextBuildIndex < SceneManager.sceneCountInBuildSettings;
    }
}
