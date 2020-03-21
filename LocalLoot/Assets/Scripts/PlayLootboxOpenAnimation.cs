using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLootboxOpenAnimation : MonoBehaviour, ISceneUpdatable
{  
    public List<string> targetStateNames => _targetStateNames;
    [SerializeField] List<string> _targetStateNames;
    [SerializeField] string animationStateName = "PackageExplode";

    public void OnEnter()
    {
        GetComponent<Animator>().Play(animationStateName);
    }

    public void OnInit(string stateName)
    {

    }

    public void OnUpdate()
    {
    }
}
