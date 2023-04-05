using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _noTargetForPossessionText;
    [SerializeField] private GameObject _alreadyInPossessionText;
    [SerializeField] private SpiritPossession _spiritPossession;

    void Awake()
    {
        _spiritPossession.possessionFailed += PossessionFailed;
    }

    private void PossessionFailed()
    {
        if (!_spiritPossession._alreadyInPossession)
            StartCoroutine(DeactivateAfterAnimationComplete(_noTargetForPossessionText, _noTargetForPossessionText.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length));
        else
            StartCoroutine(DeactivateAfterAnimationComplete(_alreadyInPossessionText, _alreadyInPossessionText.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length));
    }

    IEnumerator DeactivateAfterAnimationComplete(GameObject obj, float duration)
    {
        if (obj != null)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(duration);
            obj.SetActive(false);
        }
    }

    IEnumerator DeactivateAfterAnimationCompleteWithCallback(GameObject obj, float duration, Action callback)
    {
        if (obj != null)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(duration);
            obj.SetActive(false);
            callback();
        }
    }
}
