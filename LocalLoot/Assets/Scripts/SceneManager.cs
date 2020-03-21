using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceneManager : MonoBehaviour
{
    [SerializeField] List<StateRegistryEntry> stateRegistry;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Init();
    }

    private void Init()
    {
        foreach(StateRegistryEntry entry in stateRegistry)
        {
            foreach(MonoBehaviour mb in entry.targetScripts)
            {
                
            }
        }
    }
}
