using Hellmade.Sound;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterFootfallAudio : MonoBehaviour
{
    [SerializeField] private float _timeBetweenSteps = .2f;
    [SerializeField] private float _volume = .3f;
    [SerializeField] private List<AudioClip> _clips = new();
    private Character _character;
    private float _timeOfLastStep;
    private bool _isRunAnimationPlaying = false;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        SetIsRunAnimationPlaying(_character.IsRunAnimationPlaying);
        if (!_isRunAnimationPlaying)
            return;

        float timeSinceLastStep = Time.time - _timeOfLastStep;
        bool shouldStep = timeSinceLastStep >= _timeBetweenSteps;
        if (shouldStep)
        {
            PlayStepSound();
        }
    }

    private void SetIsRunAnimationPlaying(bool isRunAnimationPlaying)
    {
        if (isRunAnimationPlaying == _isRunAnimationPlaying)
            return;

        _isRunAnimationPlaying = isRunAnimationPlaying;
        if (isRunAnimationPlaying)
        {
            PlayStepSound();
        }
    }

    private void PlayStepSound()
    {
        int index = Random.Range(0, _clips.Count);
        AudioClip clip = _clips[index];
        EazySoundManager.PlaySound(clip, _volume);
        _timeOfLastStep = Time.time;
    }
}