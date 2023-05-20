using UnityEngine;

public class CallOther : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // touching an item
        if (other.CompareTag("PlayerObj"))
        {
            GetComponentInParent<Item>().PlayerEntered();
        }
    }
}
