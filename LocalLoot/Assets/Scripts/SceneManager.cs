using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class SceneManager : MonoBehaviour
{
    [SerializeField] List<StateRegistryEntry> stateRegistry;
    StateMachineBehaviour[] stateMachineBehaviours;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Init();
    }

    void ResetAnimatorState()
    {
        animator.SetBool("StoreSelected", false);
        animator.SetBool("CouponSelected", false);
        animator.SetBool("LootboxReadyForOpen", false);
        animator.SetBool("LootboxOnGround", false);
        animator.SetBool("DonationAmountConfirmed", false);
        animator.SetBool("LootboxOpened", false);
    }

    private void Init()
    {
        stateMachineBehaviours = animator.GetBehaviours<SceneManagerUpdater>();

        foreach (MonoBehaviour mb in FindObjectsOfType<MonoBehaviour>())
        {
            if (!(mb is ISceneUpdatable))
                continue;
            ProcessSceneUpdatable(mb);
        }
    }

    public void ProcessSceneUpdatable(MonoBehaviour mb)
    {
        ISceneUpdatable sceneUpdatable = (ISceneUpdatable)mb;
        foreach (string stateName in sceneUpdatable.targetStateNames)
        {
            int fullPathHash = Animator.StringToHash("Base Layer." + stateName);
            int stateHash = Animator.StringToHash(stateName);

            if (animator.HasState(0, stateHash))
            {
                foreach (StateMachineBehaviour stateMachineBehaviour in animator.GetBehaviours(fullPathHash, 0))
                {
                    if (!(stateMachineBehaviour is SceneManagerUpdater))
                        continue;

                    ((SceneManagerUpdater)stateMachineBehaviour).AddReference(sceneUpdatable);
                    sceneUpdatable.OnInit(stateName);
                }
            }
        }
    }
}
