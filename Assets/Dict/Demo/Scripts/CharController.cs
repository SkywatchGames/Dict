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
