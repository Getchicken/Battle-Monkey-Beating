using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public Transform camHolder;
    public float yRotation;
    public float xRotation;
    public float sensX;
    public float sensY;


    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        yRotation += mouseX;

        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        xRotation += mouseY;

        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, transform.rotation.z);
    }
}
