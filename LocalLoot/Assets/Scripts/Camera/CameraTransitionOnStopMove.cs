using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTransitionOnStopMove : MonoBehaviour, ISceneUpdatable
{
    [SerializeField]
    CinemachineVirtualCamera cm1, cm2;
    [SerializeField]
    float velocityForListen, velocityForTransition;
    Rigidbody rb;
    private bool initialVelocityMet;
    bool used;

    public List<string> targetStateNames => _targetStateNames;
    [SerializeField] List<string> _targetStateNames;

    public void OnEnter()
    {
        rb = gameObject.AddComponent<Rigidbody>();
    }

    public void OnInit(string stateName)
    {
        initialVelocityMet = false;
        used = false;
    }

    public void OnUpdate()
    {
        if (used) return;
        if (!initialVelocityMet &&
            rb.velocity.magnitude > velocityForListen)
            initialVelocityMet = true;

        if (rb.velocity.magnitude < velocityForTransition && initialVelocityMet)
        {
            cm2.gameObject.SetActive(true);

            cm1.Priority = -1;
            used = true;
        }
    }
}
