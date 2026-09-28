using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField, Min(0f)] private float fallSpeed = 2.5f;
    [SerializeField] private float destroyY = -6f;

    private void Update()
    {
        Vector3 position = transform.position;
        position.y -= fallSpeed * Time.deltaTime;
        transform.position = position;

        if (position.y < destroyY)
            Destroy(gameObject);
    }
}
