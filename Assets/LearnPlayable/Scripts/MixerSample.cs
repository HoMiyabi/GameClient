using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MixerSample : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip1;
    public AnimationClip clip2;

    [Range(0f, 1f)] public float weight;

    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;

    private bool isPaused = false;

    public float time;

    private void Awake()
    {
        graph = PlayableGraph.Create();

        var anim1 = AnimationClipPlayable.Create(graph, clip1);
        var anim2 = AnimationClipPlayable.Create(graph, clip2);
        mixer = AnimationMixerPlayable.Create(graph, 2);
        graph.Connect(anim1, 0, mixer, 0);
        graph.Connect(anim2, 0, mixer, 1);
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
        mixer.SetPropagateSetTime(true);


        var output = AnimationPlayableOutput.Create(graph, "Anim", animator);
        output.SetSourcePlayable(mixer);

        graph.Play();
    }

    private void Update()
    {
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
        mixer.SetTime(time);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                mixer.SetSpeed(0);
            }
            else
            {
                mixer.SetSpeed(1);
            }
        }
    }

    private void OnDestroy()
    {
        graph.Destroy();
    }
}
