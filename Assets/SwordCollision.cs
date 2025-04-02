using UnityEngine;

public class SwordCollision : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 20f; // Lower damage
        knockbackForce = 5f; // Lighter knockback
    }
}
