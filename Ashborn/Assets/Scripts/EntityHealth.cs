using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    private EntityVFX entityVfx;
    private Entity entity;

    [SerializeField] float maxHp = 100;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")] [SerializeField]
    private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);

    [SerializeField] private float knockbackDuration = .2f;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<EntityVFX>();
        entity = GetComponent<Entity>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        Vector2 knockback = CalculateKnockback(damageDealer);

        entity?.ReceiveKnockback(knockback, knockbackDuration);
        entityVfx?.PlayOnDamageVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;
        if (maxHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Dead");
    }

    private Vector2 CalculateKnockback(Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }
}