using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoryAnimatorManager : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void UpdateAnimatorMovementValues(float horizontal, float vertical)
    {
        float snappedVertical;
        float snappedHorizontal;

        #region SnappedHorizontal
        if (horizontal > 0 && horizontal < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontal > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontal < 0 && horizontal > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontal < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region SnappedVertical
        if (vertical > 0 && vertical < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (vertical > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (vertical < 0 && vertical > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (vertical < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        //No matter in which direction the player is moving we always want the same walking or running animation
        float sumOfMovementInputs = Mathf.Abs(snappedHorizontal) + Mathf.Abs(snappedVertical);

        _animator.SetFloat("PlayerInput", Mathf.Clamp01(Mathf.Abs(sumOfMovementInputs)), 0.1f, Time.deltaTime);
    }
}
