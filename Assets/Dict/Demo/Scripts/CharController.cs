/*  Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
    This is free software. Please refer to LICENSE for more information. */

using UnityEngine;

public class CharController : MonoBehaviour
{
    public GameObject arm;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseEnter()
    {
        animator.SetTrigger("open mouth");
    }

    void OnMouseExit()
    {
        animator.SetTrigger("close mouth");
    }
}
