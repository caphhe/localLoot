using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenAnimationOnStateEntry : MonoBehaviour, ISceneUpdatable
{
    public List<string> targetStateNames => new List<string> { "OpeningLootbox" };
    Animator animator;
    public void OnEnter()
    {
        animator.Play("PackageExplode");
    }

    public void OnInit(string stateName)
    {

    }

    public void OnUpdate()
    {
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

}
