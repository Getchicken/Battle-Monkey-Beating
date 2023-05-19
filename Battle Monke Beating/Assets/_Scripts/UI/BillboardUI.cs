using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void FixedUpdate()
    {
        transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward, playerCamera.transform.rotation * Vector3.up);
    }
}
