using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayAnimationSample : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;

    private PlayableGraph graph;

    private void Start()
    {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        var anim = AnimationClipPlayable.Create(graph, clip);
        var output = AnimationPlayableOutput.Create(graph, "Anim", animator);
        output.SetSourcePlayable(anim);

        graph.Play();
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
