using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _noTargetForPossessionText;
    [SerializeField] private GameObject _alreadyInPossessionText;
    [SerializeField] private SpiritPossession _spiritPossession;

    void Awake()
    {
        _spiritPossession.possessionFailed += () =>
        {
            if (!_spiritPossession._alreadyInPossession)
                StartCoroutine(DeactivateAfterAnimationComplete(_noTargetForPossessionText, _noTargetForPossessionText.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length));
            else
                StartCoroutine(DeactivateAfterAnimationComplete(_alreadyInPossessionText, _alreadyInPossessionText.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length));
        };
    }

    IEnumerator DeactivateAfterAnimationComplete(GameObject obj, float duration)
    {
        if(obj != null)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(duration);
            obj.SetActive(false);
        }
    }
}
