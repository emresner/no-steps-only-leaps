using UnityEngine;

public class StartFlagAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [SerializeField] private string startMovingStateName = "Start_Moving";

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        PauseAtFirstFrame();
    }

    public void PlayStartAnimation()
    {
        if (animator == null)
            return;

        animator.speed = 1f;
        animator.Play(startMovingStateName, 0, 0f);
        animator.Update(0f);
    }

    private void PauseAtFirstFrame()
    {
        if (animator == null)
            return;

        animator.speed = 0f;
        animator.Play(startMovingStateName, 0, 0f);
        animator.Update(0f);
    }
}