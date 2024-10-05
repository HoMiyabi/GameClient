using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    private Animator animator;

    public enum HState
    {
        None = 0,
        Idle = 1,
        Run = 2,
        Attack = 3,
        Die = 4,
        Gethit = 5,
    }

    public HState state = HState.Idle;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayRun()
    {
        animator.SetBool("Run", true);
        state = HState.Run;
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
        state = HState.Attack;
    }

    public void PlayDie()
    {
        animator.SetTrigger("Die");
        state = HState.Die;
    }

    public void PlayGetHit()
    {
        animator.SetTrigger("GetHit");
        state = HState.Gethit;
    }
}
