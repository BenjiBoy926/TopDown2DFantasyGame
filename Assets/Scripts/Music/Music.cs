using Hellmade.Sound;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip _loop;
    [SerializeField] private float _volume = 1;
    private bool _isPlaying = false;
    private Audio _introAudio;
    private Audio _loopAudio;

    public void Play()
    {
        int id = EazySoundManager.PlayMusic(_intro, _volume);
        _introAudio = EazySoundManager.GetAudio(id);
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        if (ShouldTransitionToLoop())
        {
            int id = EazySoundManager.PlayMusic(_loop, _volume, true, true, 0, 0);
            _loopAudio = EazySoundManager.GetAudio(id);
        }
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
