using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ParticleSystem _purpleBuffParticle;
    PlayerStats _playerStats;
    LevelSystem _levelSystem;
    Shape _shape;

    void Awake()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _levelSystem = FindObjectOfType<LevelSystem>();
        _shape = GetComponent<Shape>();
    }

    public void OnTriggerEnter(Collider other)
    {
        // touching an item
        if (other.CompareTag("PlayerObj"))
        {
            PlayerEntered();
        }
    }

    public void PlayerEntered()
    {
        XPGained();
        HPGained();

        if (_purpleBuffParticle != null)
        {
            _purpleBuffParticle = Instantiate(_purpleBuffParticle, transform.position, Quaternion.identity);
        }
        //Remove the object from the array before destroying it
        gameObject.SetActive(false);
    }

    
    private void XPGained()
    {
        _levelSystem.GainExperienceFlateRate(25);
    }

    private void HPGained()
    {
        _playerStats.Healing(10f);
    }
}
