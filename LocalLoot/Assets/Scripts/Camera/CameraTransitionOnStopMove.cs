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
    float requiredVelocity;
    Rigidbody rb;
    private bool initialVelocityMet;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialVelocityMet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialVelocityMet &&
            rb.velocity.magnitude > requiredVelocity)
            initialVelocityMet = true;

        if (rb.velocity.magnitude < .05f && initialVelocityMet)
        {
            cm1.gameObject.SetActive(false);
            cm2.gameObject.SetActive(true);
        }
    }

}
