using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public GameObject UiCanvas;
    UiManager um;

    void Start()
    {
        um = UiCanvas.GetComponent<UiManager>();
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerObj")      
        {
            um.DeathUI();
        }
        else
        {
            //Debug.Log("GameObject doesnt have PlayerObj tag or PlayerStats script");
        }
    }
}
