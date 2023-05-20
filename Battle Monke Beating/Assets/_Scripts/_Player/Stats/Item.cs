using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ParticleSystem _oParticleSystem;
    [SerializeField] ParticleSystem _tParticleSystem;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _xpBar;
    PlayerStats _playerStats;
    LevelSystem _levelSystem;
    Shape _shape;

    void Awake()
    {
        _playerStats = _player.GetComponent<PlayerStats>();
        _levelSystem = _xpBar.GetComponent<LevelSystem>();
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

        if (_oParticleSystem != null)
        {
            _oParticleSystem = Instantiate(_oParticleSystem, transform.position, Quaternion.identity);
        }
        //Remove the object from the array before destroying it
        gameObject.SetActive(false);
    }

    
    private void XPGained()
    {
        _levelSystem.GainExperienceFlateRate(10);
    }

    private void HPGained()
    {
        _playerStats.Healing(10f);
    }
}
