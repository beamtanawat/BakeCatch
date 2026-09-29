using UnityEngine;

public class CatchZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) => TryCatch(other);
    // Stay also handles an ingredient overlapping when a paused session resumes.
    private void OnTriggerStay2D(Collider2D other) => TryCatch(other);

    private void TryCatch(Collider2D other)
    {
        FallingObject ingredient = other.GetComponentInParent<FallingObject>();
        if (ingredient != null) ingredient.TryCatch();
    }
}
