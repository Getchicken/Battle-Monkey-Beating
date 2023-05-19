using System.Collections;
using UnityEngine;

public class RapidDecay : MonoBehaviour
{
    public float _decayMultiplier;
    public float _decayBase;
    public bool _isDecaying = true;
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        StartCoroutine(DecayOverTime());
    }

    IEnumerator DecayOverTime()
    {
        while (_isDecaying)
        {
            playerStats.TakeDamage(calculateDecay());
            yield return new WaitForSeconds(0.4f); // adjust the wait time to your desired rate
        }
    }

    private float calculateDecay()
    {
        return Time.deltaTime * _decayBase * _decayMultiplier; 
    }
}
