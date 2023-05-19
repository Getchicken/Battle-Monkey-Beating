using UnityEngine;
using TMPro;


public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 randomiseOffset;
    [SerializeField] private Color damageColour;

    private TextMeshPro damageText;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();

        Vector3 randomiseOffset = new Vector3(1.0f, 1.0f, 1.0f);

        transform.localPosition += offset;
        transform.localPosition += offset + new Vector3(
            Random.Range(-randomiseOffset.x, randomiseOffset.x),
            Random.Range(-randomiseOffset.y, randomiseOffset.y),
            Random.Range(-randomiseOffset.z, randomiseOffset.z));       
    }

    public void Initialise(float damageAmount)
    {
        damageText.text = damageAmount.ToString();
    }
}

