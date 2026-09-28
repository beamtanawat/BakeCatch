using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider inputProvider;
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;

    private void Awake()
    {
        if (inputProvider == null || minX > maxX)
        {
            Debug.LogError("PlayerController needs an Input Provider and Min X <= Max X.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.x += inputProvider.HorizontalInput * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);

        // Preserve the existing Y and Z positions.
        transform.position = position;
    }
}
