using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public bool isRagdollActive = false;

    private Rigidbody[] _ragdollRigidbodies;

    void Start()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Update()
    {
        ApplyRagdollState();
    }

    private void ApplyRagdollState()
    {
        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = !isRagdollActive;
        }
    }
}