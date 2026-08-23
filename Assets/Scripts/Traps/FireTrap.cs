using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hazardHitbox;

    [Header("Timing")]
    [SerializeField] private float offDuration = 1.5f;
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float onDuration = 1.2f;

    private Coroutine fireRoutine;

    private void OnEnable()
    {
        fireRoutine = StartCoroutine(FireCycleRoutine());
    }

    private void OnDisable()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
        }
    }

    private IEnumerator FireCycleRoutine()
    {
        while (true)
        {
            SetFireOff();
            yield return new WaitForSeconds(offDuration);

            SetFireWarning();
            yield return new WaitForSeconds(warningDuration);

            SetFireOn();
            yield return new WaitForSeconds(onDuration);
        }
    }

    private void SetFireOff()
    {
        hazardHitbox.SetActive(false);
        animator.Play("FireOff");
    }

    private void SetFireWarning()
    {
        hazardHitbox.SetActive(false);
        animator.Play("FireHit");
    }

    private void SetFireOn()
    {
        hazardHitbox.SetActive(true);
        animator.Play("FireOn");
    }
}