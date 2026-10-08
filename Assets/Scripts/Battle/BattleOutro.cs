using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleOutro : MonoBehaviour
{
    [SerializeField] private float _initialDelay = 1;
    [SerializeField] private Music _music;
    [SerializeField] private Overlay _fadeOutOverlay;
    [SerializeField] private SceneAsset _nextScene;

    public void Play()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(_initialDelay);
        _music.Play();
        DontDestroyOnLoad(_music);
        yield return _fadeOutOverlay.FadeIn();
        SceneManager.LoadScene(_nextScene.name);
    }
}