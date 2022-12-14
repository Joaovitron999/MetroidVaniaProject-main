using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    //open door
    public void OpenDoor()
    {
        if (!isOpen)
        {
            animator.SetTrigger("open");
            isOpen = true;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }
}
