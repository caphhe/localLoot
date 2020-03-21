using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerUpdater : StateMachineBehaviour
{
    List<ISceneUpdatable> updatables;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
        foreach (ISceneUpdatable updatable in updatables)
            updatable.OnUpdate();
        
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        foreach (ISceneUpdatable updatable in updatables)
            updatable.OnEnter();

    }

    internal void AddReference(ISceneUpdatable mb)
    {
        if (updatables == null)
            updatables = new List<ISceneUpdatable>();

        updatables.Add(mb);
    }
}
