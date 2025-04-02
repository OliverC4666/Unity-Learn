using UnityEngine;

public class Knife : Weapon
{
    void Start()
    {
        damage = 20f; // Lower damage
        knockbackForce = 5f; // Lighter knockback
    }
}
