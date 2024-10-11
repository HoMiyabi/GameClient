using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class RuntimeControllerSample : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;
    public RuntimeAnimatorController controller;

    [Range(0f, 1f)] public float weight;
    private AnimationMixerPlayable mixer;

    private PlayableGraph graph;

    private void Awake()
    {
        graph = PlayableGraph.Create();

        mixer = AnimationMixerPlayable.Create(graph);
        var anim = AnimationClipPlayable.Create(graph, clip);
        var ctrl = AnimatorControllerPlayable.Create(graph, controller);
        mixer.AddInput(anim, 0, 1 - weight);
        mixer.AddInput(ctrl, 0, weight);

        var output = AnimationPlayableOutput.Create(graph, "Animation", animator);
        output.SetSourcePlayable(mixer);

        graph.Play();
    }

    private void Update()
    {
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
    }

    private void OnDestroy()
    {
        graph.Destroy();
    }
}
