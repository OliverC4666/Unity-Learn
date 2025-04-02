using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthBar; // Assign in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage, Vector3 hitForce, Transform hitPoint)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            ApplyHitForce(hitForce, hitPoint);
        }
        UpdateHealthBar();
    }

    private void ApplyHitForce(Vector3 force, Transform hitPoint)
    {
        Rigidbody rb = hitPoint.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth; // Normalize between 0 and 1
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is dead!");
        GetComponent<RagdollController>().ActivateRagdoll(); // Activate ragdoll on death
    }
}
