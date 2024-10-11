using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class HeroAnimations : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed;
    public float attackWeight;

    public AnimationClip idleClip;
    public AnimationClip runClip;
    private AnimationMixerPlayable moveMixer;

    private PlayableGraph graph;

    // public enum HState
    // {
    //     None = 0,
    //     Idle = 1,
    //     Run = 2,
    //     Attack = 3,
    //     Die = 4,
    //     Gethit = 5,
    // }
    // public HState state = HState.Idle;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        graph = PlayableGraph.Create("HeroAnimations");

        var idleAnim = AnimationClipPlayable.Create(graph, idleClip);
        var runAnim = AnimationClipPlayable.Create(graph, runClip);
        var moveMixer = AnimationMixerPlayable.Create(graph);
        moveMixer.AddInput(idleAnim, 0);
        moveMixer.AddInput(runAnim, 0);

        var output = AnimationPlayableOutput.Create(graph, "Anim", animator);
        output.SetSourcePlayable(moveMixer);
    }

    private void Update()
    {
        if (moveSpeed == 0f)
        {
            moveMixer.SetInputWeight(0, 1f);
            moveMixer.SetInputWeight(1, 0f);
        }
        else
        {
            moveMixer.SetInputWeight(0, 0f);
            moveMixer.SetInputWeight(1, 1f);
        }
    }

    private void OnDestroy()
    {
        graph.Destroy();
    }


    // public void PlayRun()
    // {
    //     animator.SetBool("Run", true);
    //     state = HState.Run;
    // }
    //
    // public void PlayAttack()
    // {
    //     animator.SetTrigger("Attack");
    //     state = HState.Attack;
    // }
    //
    // public void PlayDie()
    // {
    //     animator.SetTrigger("Die");
    //     state = HState.Die;
    // }
    //
    // public void PlayGetHit()
    // {
    //     animator.SetTrigger("GetHit");
    //     state = HState.Gethit;
    // }
}
