using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : MonoBehaviour
{
    // Movement consumes only this value, independent of the input source.
    public float HorizontalInput
    {
        get
        {
            Keyboard keyboard = Keyboard.current;
            if (!isActiveAndEnabled || keyboard == null)
                return 0f;

            bool left = keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed;
            bool right = keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed;

            // Opposite directions cancel, including mixed letter/arrow keys.
            return (right ? 1f : 0f) - (left ? 1f : 0f);
        }
    }
}
