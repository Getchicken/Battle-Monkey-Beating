using UnityEngine;

public class RotateSword : MonoBehaviour
{
    public GameObject WeaponHolder;
    [Header("N2 Transform")]
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    public float positionX;
    public float positionY;
    public float positionZ;

    [Header("N3      Transform")]
    public float rotationX2;
    public float rotationY2;
    public float rotationZ2;

    public float positionX2;
    public float positionY2;
    public float positionZ2;

    private Quaternion startRotation;
    private Vector3 startPosition;

    void Start()
    {
        Quaternion startRotation = WeaponHolder.transform.rotation;
        Vector3 startPosition = WeaponHolder.transform.position;
    }

    public void ChangeSwordRotation()
    {
        Quaternion newRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        WeaponHolder.transform.rotation = newRotation;
    }

    public void OriginalSwordRotation()
    {
        WeaponHolder.transform.rotation = startRotation;
    }

    public void ChangeSwordPosition()
    {
        Vector3 newPosition = new Vector3(positionX, positionY, positionZ);
        WeaponHolder.transform.position = newPosition;
    }

    public void OriginalSwordPosition()
    {
        WeaponHolder.transform.position = startPosition;
    }
}
