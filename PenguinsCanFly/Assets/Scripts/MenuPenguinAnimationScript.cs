using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPenguinAnimationScript : MonoBehaviour
{
    private Animator mAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void onHoverEnter()
    {
        mAnimator.SetBool("isHover", true);
    }

    public void onHoverExit()
    {
        mAnimator.SetBool("isHover", false);
    }
}
