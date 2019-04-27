using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBurn : MonoBehaviour
{
    [SerializeField]
    private string animation;
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.Play(animation);
    }
}
