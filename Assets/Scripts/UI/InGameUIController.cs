using System;
using System.Collections;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _noTargetForPossessionText;
    [SerializeField] private GameObject _alreadyInPossessionText;
    [SerializeField] private GameObject _whistleFailedText;
    [SerializeField] private SpiritPossession _spiritPossession;
    [SerializeField] private WhistleMechanic _whistleMechanic;

    void OnEnable()
    {
        _spiritPossession.possessionFailed += PossessionFailedEventHandler;
        _whistleMechanic.OnWhistleFailed += WhistleFailedEventHandler;
    }

    private void OnDisable()
    {
        _spiritPossession.possessionFailed -= PossessionFailedEventHandler;
        _whistleMechanic.OnWhistleFailed -= WhistleFailedEventHandler;
    }

    private void WhistleFailedEventHandler()
    {
        StartCoroutine(DeactivateAfterAnimationComplete(_whistleFailedText, _whistleFailedText.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length));
    }

    private void PossessionFailedEventHandler()
    {
        if (!_spiritPossession.alreadyInPossession)
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
