using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClip : StateMachineBehaviour
{
    public float t = 0.0f;  // This now represents the last play time to avoid confusion
    public float modulus = 0.33f;  // Frequency of playing the sound
    public AudioClip clip;

    private float nextPlayTime = 0.0f;  // Tracks when to next play the sound

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= nextPlayTime)
        {
            AudioSource.PlayClipAtPoint(clip, animator.transform.position);
            nextPlayTime += modulus;
        }

        if (stateInfo.normalizedTime < t)
        {
            nextPlayTime = stateInfo.normalizedTime + modulus;
        }

        t = stateInfo.normalizedTime;
    }
}
