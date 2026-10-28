using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleOutro : MonoBehaviour
{
    [SerializeField] private float _initialDelay = 1;
    [SerializeField] private Music _music;
    [SerializeField] private Overlay _fadeOutOverlay;
    [SerializeField] private Object _nextScene;

    public void Play()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(_initialDelay);
        _music.Play();
        yield return _fadeOutOverlay.FadeIn();
        SceneManager.LoadScene(_nextScene.name);
    }
}