using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(Rigidbody))]
public class CameraTransitionOnStopMove : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera cm1, cm2;
    [SerializeField]
    float velocityForListen, velocityForTransition;
    Rigidbody rb;
    private bool initialVelocityMet;
    bool used;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialVelocityMet = false;
        used = false;
    }

    // Update is called once per frame
    void Update()
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
