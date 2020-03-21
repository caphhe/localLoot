using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerUpdater : StateMachineBehaviour
{
    public List<MonoBehaviour> behavioursToUpdate;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
    }
}
