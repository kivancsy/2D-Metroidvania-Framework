using System;
using System.Linq.Expressions;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public float damage = 10;

    [Header("Target Detection")] [SerializeField]
    private Transform targetCheck;

    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            EntityHealth targetHealth = target.GetComponent<EntityHealth>();

            if (targetHealth != null)
                targetHealth.TakeDamage(damage);
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}