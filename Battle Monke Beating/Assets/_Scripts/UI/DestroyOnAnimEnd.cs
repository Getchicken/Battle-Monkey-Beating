using UnityEngine;

public class DestroyOnAnimEnd : MonoBehaviour
{
    public void OnAnimationEvent()
    {
        Destroy(gameObject);
    }
}
