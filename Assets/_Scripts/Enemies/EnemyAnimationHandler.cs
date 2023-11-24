using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
        }
    }

    public void PlayHitAnimation()
    {
        animator.CrossFade("Enemy_Hit", 0.1f);
    }

    public void OnHitAnimationEnd()
    {
        animator.CrossFade("Enemy_Idle", 0.1f);
    }
}
