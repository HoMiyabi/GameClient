using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimQueue : PlayableBehaviour
{
    private AnimationMixerPlayable mixer;

    private int currentIndex = 0;
    private float currentLength;

    public void Init(AnimationClip[] clips, Playable playable, PlayableGraph graph)
    {
        mixer = AnimationMixerPlayable.Create(graph);
        foreach (var clip in clips)
        {
            mixer.AddInput(AnimationClipPlayable.Create(graph, clip), 0);
        }
        mixer.SetInputWeight(0, 1.0f);
        playable.AddInput(mixer, 0, 1f);

        currentLength = clips[0].length;
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);

        if (mixer.GetInput(currentIndex).GetTime() >= currentLength)
        {
            if (currentIndex < mixer.GetInputCount() - 1)
            {
                mixer.SetInputWeight(currentIndex++, 0);
                mixer.SetInputWeight(currentIndex, 1);
                var current = (AnimationClipPlayable)mixer.GetInput(currentIndex);
                current.SetTime(0);
                current.SetTime(0);
                currentLength = current.GetAnimationClip().length;
            }
        }
    }
}

public class AnimQueueSample : MonoBehaviour
{
    public Animator animator;
    public AnimationClip[] clips;

    private PlayableGraph graph;

    private void Start()
    {
        graph = PlayableGraph.Create();

        var animQueuePlayable = ScriptPlayable<AnimQueue>.Create(graph);
        animQueuePlayable.GetBehaviour().Init(clips, animQueuePlayable, graph);

        var output = AnimationPlayableOutput.Create(graph, "Animation", animator);
        output.SetSourcePlayable(animQueuePlayable);

        graph.Play();
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
