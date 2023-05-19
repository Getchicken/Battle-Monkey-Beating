using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffect/HealthIncrease")]
public class HealthIncrease : PowerUpEffect
{
    [SerializeField] private float amount;
    PlayerStats _playerStats;

    public override void Apply(GameObject target)
    {
        _playerStats = target.GetComponentInParent<PlayerStats>();
        _playerStats.Healing(amount);
        _playerStats.BuffMaxHealth(amount);
    }
}
