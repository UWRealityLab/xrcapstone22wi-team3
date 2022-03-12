using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupPenguinAnimationScript : MonoBehaviour
{
    private List<Animator> animatorList;
    public GameObject friendPang1LOD0;
    public GameObject friendPang1LOD1;
    public GameObject friendPang2LOD0;
    public GameObject friendPang2LOD1;

    // Start is called before the first frame update
    void Start()
    {
        animatorList = new List<Animator>();
        animatorList.Add(GetComponent<Animator>());
        animatorList.Add(friendPang1LOD0.GetComponent<Animator>());
        animatorList.Add(friendPang1LOD1.GetComponent<Animator>());
        animatorList.Add(friendPang2LOD0.GetComponent<Animator>());
        animatorList.Add(friendPang2LOD1.GetComponent<Animator>());
    }

    public void onHoverEnter()
    {
        foreach (Animator animator in animatorList)
        {
            animator.SetBool("isHover", true);
        }
    }

    public void onHoverExit()
    {
        foreach (Animator animator in animatorList)
        {
            animator.SetBool("isHover", false);
        }
    }
}
