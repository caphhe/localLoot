using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraEffectsController : MonoBehaviour
{
    static CameraEffectsController instance;
    Animator animator;
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }


    public static void Shake()
    {
        instance.animator.Play("Shake");
    }

}
