using Hellmade.Sound;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip _loop;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _fadeIn = 1;
    [SerializeField] private float _fadeOut = 1;
    private bool _isPlaying = false;
    private Audio _introAudio;
    private Audio _loopAudio;

    public void Play()
    {
        if (_intro)
        {
            _introAudio = FadeIn(_intro, false);
        }
        else
        {
            _loopAudio = FadeIn(_loop, true);
        }
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        if (ShouldTransitionToLoop())
        {
            _loopAudio = Play(_loop, true);
        }
    }

    private Audio FadeIn(AudioClip clip, bool loop)
    {
        return Play(clip, loop, _fadeIn);
    }

    private Audio Play(AudioClip clip, bool loop)
    {
        return Play(clip, loop, 0);
    }

    private Audio Play(AudioClip clip, bool loop, float fadeIn)
    {
        int id = EazySoundManager.PlayMusic(clip, _volume, loop, true, fadeIn, _fadeOut);
        return EazySoundManager.GetAudio(id);
    }

    private bool ShouldTransitionToLoop()
    {
        return _introAudio != null && !_introAudio.IsPlaying && _loopAudio == null;
    }

    public void Stop()
    {
        Stop(ref _introAudio);
        Stop(ref _loopAudio);
        _isPlaying = false;
    }

    private void Stop(ref Audio audio)
    {
        if (audio != null && audio.IsPlaying)
        {
            audio.Stop();
            audio = null;
        }
    }
}
