using UnityEngine;

public class DestroyOn : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 4f;
    public void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        Invoke("Destroy", _destroyTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
