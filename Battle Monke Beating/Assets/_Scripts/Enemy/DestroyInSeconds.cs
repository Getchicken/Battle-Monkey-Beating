using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    [SerializeField] private float _destroyInSeconds = 3f;
    private void Awake()
    {
        Destroy(gameObject, _destroyInSeconds);
    }
}
