using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    private Animator _animator;

    public void Awake()
    {
        if (Instance is null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Death.playerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        Death.playerDied -= OnPlayerDied;
    }
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnPlayerDied()
    {
        _animator.SetTrigger("PlayerDied");
    }
}
