using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject gameUi;
    public GameObject menuUi;
    public GameObject optionsUi;
    public GameObject completeLevelUI;
    public GameObject respawnButtonHolder;
    public GameObject player;
    public GameObject camObj;
    public GameManager gm;
    [SerializeField] private TextMeshProUGUI _BananaGunUI;
    [SerializeField] private TextMeshProUGUI _BananaBlasterUI;
    [SerializeField] private TextMeshProUGUI _CoconutLaucherUI;

    private PlayerCam playerCam;
    private Rigidbody rb;

    public MonoBehaviour[] scriptsToDisable;

    public KeyCode UiKey = KeyCode.Escape;
    public KeyCode UiKey2 = KeyCode.Tab;
    public UiState state;

    public enum UiState
    {
        GameUi,
        MenuUi,
        OptionsUi,
        WinUi,
        DeathUi,
    }

    private void Awake()
    {
        // Add all scripts on the _player object to the list
        scriptsToDisable = player.GetComponents<MonoBehaviour>();
        playerCam = camObj.GetComponent<PlayerCam>();
        rb = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        state = UiState.GameUi;
        EnablePlayerScripts();
    }

    void Update()
    {
        UiStateHandler();
        ChangeUiState();
    }

    public void DisablePlayerScripts()
    {
        // If the _player pauses/dies/goes to the menu - freeze the rigidbody and disable the _player scripts
        FreezeRb();
        if (state != UiState.GameUi) 
        {
            // disable all monobehavior scipts in an array "scriptsToDisable"
            foreach (MonoBehaviour m in scriptsToDisable)
            {
                m.enabled = false;
            }
        }
    }

    public void EnablePlayerScripts()
    {
        // Resume the game - re-enable the _player scripts and unfreeze the rigidbody
        UnFreezeRb();
        if (state == UiState.GameUi)
        // enable all monobehavior scipts in an array "scriptsToDisable"
        {
            foreach (MonoBehaviour m in scriptsToDisable)
            {
                m.enabled = true;
            }
        }
    }

    public void UiStateHandler()
    {
        if(state == UiState.DeathUi)
        {
            DisablePlayerScripts();
            respawnButtonHolder.SetActive(true);
            menuUi.SetActive(false);
            optionsUi.SetActive(false);
            gameUi.SetActive(false);
            completeLevelUI.SetActive(false);
            // Re-lock the cursor when the game UI is active again
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(state == UiState.GameUi)
        {
            EnablePlayerScripts();
            respawnButtonHolder.SetActive(false);
            menuUi.SetActive(false);
            optionsUi.SetActive(false);
            gameUi.SetActive(true);
            completeLevelUI.SetActive(false);
            // Re-lock the cursor when the game UI is active again
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if(state == UiState.MenuUi)
        {
            DisablePlayerScripts();
            menuUi.SetActive(true);
            optionsUi.SetActive(false);
            gameUi.SetActive(false);
            completeLevelUI.SetActive(false);
            // Release the cursor when the menu UI is active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(state == UiState.OptionsUi)
        {
            DisablePlayerScripts();
            menuUi.SetActive(false);
            optionsUi.SetActive(true);
            gameUi.SetActive(false);
            completeLevelUI.SetActive(false);
            // Release the cursor when the menu UI is active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(state == UiState.WinUi)
        {
            UnFreezeRb(); 
            menuUi.SetActive(false);
            optionsUi.SetActive(false);
            gameUi.SetActive(false);
            completeLevelUI.SetActive(true);
            // Release the cursor when the menu UI is active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ChangeUiState()
    {
        if(Input.GetKeyDown(UiKey) || Input.GetKeyDown(UiKey2))
        {
            // if GameUi is on, turn it off and turn MenuUi on 
            if (gameUi.activeSelf)
            {
                state = UiState.MenuUi;
            }
            // if MenuUi is on, turn it off and turn GameUi on 
            else if (menuUi.activeSelf)
            {
                state = UiState.GameUi;
            }
            else if (optionsUi.activeSelf)
            {
                state = UiState.GameUi;
            }
        }
    }

    public void DeathUI()
    {
        state = UiState.DeathUi;
    }

    public void GameUI()
    {
        state = UiState.GameUi;
    }

    public void MenuUI()
    {
        state = UiState.MenuUi;
    }

    public void OptionsUI()
    {
        state = UiState.OptionsUi;
    }

    public void WinUI()
    {
        state = UiState.WinUi;
    }

    public void LevelReset()
    {
        gm.SetPositionToSpawn();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void FreezeRb()
    {
        rb = player.GetComponent<Rigidbody>();
        playerCam = camObj.GetComponent<PlayerCam>();
        
        playerCam.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void UnFreezeRb()
    {
        rb = player.GetComponent<Rigidbody>();
        playerCam = camObj.GetComponent<PlayerCam>();
        
        playerCam.enabled = true;
        rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        rb.freezeRotation = true;
    }

    public void UpdateAmmo(float currentAmmo)
    {
        _BananaGunUI.text = "" + currentAmmo;
    }
    public void UpdateBlasterAmmo (float currentAmmo)
    {
        _BananaBlasterUI.text = "" + currentAmmo;
    }
    public void UpdateCocoAmmo(float currentAmmo)
    {
        _CoconutLaucherUI.text = "" + currentAmmo;
    }
}
