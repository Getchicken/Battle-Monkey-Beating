using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffect/DamageIncrease")]
public class DamageIncrease : PowerUpEffect
{
    [SerializeField] private float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponentInParent<PlayerStats>().IncreaseDamage(amount);
    }
}
