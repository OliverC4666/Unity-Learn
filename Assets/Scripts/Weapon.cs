using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f; // Default damage
    public float knockbackForce = 10f; // Default knockback

    // Universal attack logic
    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);

        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
        if (health != null)
        {
            Vector3 hitDirection = (collision.transform.position - transform.position).normalized;

            // Apply damage and knockback
            health.TakeDamage(damage, hitDirection * knockbackForce, collision.transform);
            Debug.Log(collision.gameObject.name + " took " + damage + " damage from " + gameObject.name);
        }
    }
}
