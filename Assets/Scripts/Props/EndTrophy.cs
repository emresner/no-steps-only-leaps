using UnityEngine;

public class EndTrophy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [SerializeField] private string pressTriggerName = "Press";

    private bool hasBeenPressed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenPressed)
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null)
            return;

        Press();
    }

    private void Press()
    {
        hasBeenPressed = true;

        if (animator != null)
            animator.SetTrigger(pressTriggerName);
    }
}