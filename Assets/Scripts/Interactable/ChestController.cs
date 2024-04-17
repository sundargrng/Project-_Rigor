using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator animator;

    public bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenChest()
    {
        if(!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest Opened");
            animator.SetBool("IsOpen", isOpen);
        }
    }
}
