using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Animator animator;
    private bool isRagdollActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRagdollActive(false);
        PreventSelfCollision();
    }

    void SetRagdollActive(bool state)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !state;
            rb.maxAngularVelocity = 5f;
            rb.angularDamping = 5f;  // ✅ Updated from angularDrag to angularDamping
            //rb.linearVelocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            rb.maxDepenetrationVelocity = 2f;
        }

    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;
        SetRagdollActive(true);
        StartCoroutine(WaitAndApplyForce());
    }

    private IEnumerator WaitAndApplyForce()
    {
        yield return new WaitForFixedUpdate();
    }

    public void ApplyForce(Vector3 force, Transform hitPoint)
    {
        Rigidbody rb = hitPoint.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void PreventSelfCollision()
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            Collider rbCollider = rb.GetComponent<Collider>();
            foreach (Rigidbody otherRb in ragdollRigidbodies)
            {
                Collider otherCollider = otherRb.GetComponent<Collider>();
                if (rb != otherRb)
                    Physics.IgnoreCollision(rbCollider, otherCollider);
            }
        }
    }

    void Update()
    {
        if (isRagdollActive)
        {
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                rb.AddForce(Vector3.down * rb.mass * 2, ForceMode.Acceleration);
            }
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            ActivateRagdoll();
            ApplyForce(new Vector3(0, 2, -5), transform);
        }
    }
}
