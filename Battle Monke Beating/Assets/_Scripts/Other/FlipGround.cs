using System.Collections;
using UnityEngine;

public class FlipGround : MonoBehaviour
{
    [SerializeField] private float _flipAngle = 90f;
    [SerializeField] private float _flipDuration = 2f;
    [SerializeField] private float _flipBackDelay;

    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private bool _isFlipping = false;

    private void Start()
    {
        _startRotation = Quaternion.Euler(0f, 0f, 0f); ;
        _endRotation = Quaternion.Euler(0f, 0f, _flipAngle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !_isFlipping)
        {
            StartCoroutine(Flip());
        }
    }

    private IEnumerator Flip()
    {
        _isFlipping = true;
        float t = 0f;
        Quaternion startRotation = transform.parent.rotation;
        Quaternion endRotation = _endRotation;

        while (t < 1f)
        {
            t += Time.deltaTime / _flipDuration;
            transform.parent.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        StartCoroutine(FlipBack());
    }

    private IEnumerator FlipBack()
    {
        yield return new WaitForSeconds(_flipBackDelay);
        float t = 0f;
        Quaternion startRotation = transform.parent.rotation;
        Quaternion endRotation = _startRotation;

        while (t < 1f)
        {
            t += Time.deltaTime / _flipDuration;
            transform.parent.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        _isFlipping = false;
    }
}




