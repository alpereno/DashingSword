using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // Animating
    Animator animator;
    int horizontal;
    int vertical;

    void Start()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalInput, float verticalInput)
    {
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalInput > 0 && horizontalInput < .55f)
        {
            snappedHorizontal = .5f;
        }
        else if (horizontalInput > .55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalInput < 0 && horizontalInput > -.55f)
        {
            snappedHorizontal = -.5f;
        }
        else if (horizontalInput < -.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region Snapped Vertical
        if (verticalInput > 0 && verticalInput < .55f)
        {
            snappedVertical = .5f;
        }
        else if (verticalInput > .55f)
        {
            snappedVertical = 1;
        }
        else if (verticalInput < 0 && verticalInput > -.55f)
        {
            snappedVertical = -.5f;
        }
        else if (verticalInput < -.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        animator.SetFloat(horizontal, snappedHorizontal, .1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, .1f, Time.deltaTime);
    }
}