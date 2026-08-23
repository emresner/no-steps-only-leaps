using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float horizontalKnockbackForce = 6f;
    [SerializeField] private float verticalKnockbackForce = 5f;

    [Header("Hit Settings")]
    [SerializeField] private float hitCooldown = 0.5f;

    private float lastHitTime = -Mathf.Infinity;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHitPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryHitPlayer(other);
    }

    private void TryHitPlayer(Collider2D other)
    {
        if (Time.time < lastHitTime + hitCooldown)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        Vector2 hitVelocity = CalculateHitVelocity(player);

        player.TakeTrapHit(hitVelocity);

        lastHitTime = Time.time;
    }

    private Vector2 CalculateHitVelocity(Player player)
    {
        float directionX = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(directionX) < 0.1f)
        {
            directionX = -player.FacingDirection;
        }

        directionX = Mathf.Sign(directionX);

        return new Vector2(
            directionX * horizontalKnockbackForce,
            verticalKnockbackForce
        );
    }
}