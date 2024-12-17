using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerMovement playermovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetBool("IsMoving", playermovement.isMoving);
        
    }
    public void Dropp()
    {
        animator.SetTrigger("Drope");
    }
}
