using UnityEngine;

public class XpIncrease : PowerUpEffect
{
    [SerializeField] private float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponentInParent<PlayerStats>().IncreaseDamage(amount);
    }
}
