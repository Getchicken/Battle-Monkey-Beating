using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject parentObject;
    public ParticleSystem _oParticleSystem;
    public ParticleSystem _tParticleSystem;
    public GameObject _player;
    public GameObject _xpBar;
    private PlayerStats playerStats;
    private LevelSystem levelSystem;
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerStats = _player.GetComponent<PlayerStats>();
        levelSystem = _xpBar.GetComponent<LevelSystem>();
        playerMovement = _player.GetComponent<PlayerMovement>();
    }

    public void OnTriggerEnter(Collider other)
    {
        // touching an item
        if (other.CompareTag("PlayerObj"))
        {
            XPGained();
            HPGained();

            if (gameObject.tag == "Speed")
                MSGained();

            if (_oParticleSystem != null)
            {
                _oParticleSystem = Instantiate(_oParticleSystem, transform.position, Quaternion.identity);
            }
            Destroy(parentObject);
        }

        if(other.CompareTag("Kunai"))
        {
            XPGained();
            HPGained();

            if(_tParticleSystem != null)
                _tParticleSystem = Instantiate(_tParticleSystem, transform.position, Quaternion.identity);

            Destroy(parentObject);
            Destroy(other.gameObject);
        }
    }

    
    private void XPGained()
    {
        levelSystem.GainExperienceFlateRate(10);
    }

    private void HPGained()
    {
        playerStats.Healing(10);
    }

    private void MSGained()
    {
        playerMovement.BuffSpeed(2f);
    }
}
