using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class EnableVcamOnStateEnter : MonoBehaviour, ISceneUpdatable
{
    [SerializeField] private List<string> _targetStateNames;
    [SerializeField] CinemachineVirtualCamera targetVCam;

    public List<string> targetStateNames => _targetStateNames;

    public void OnEnter()
    {
        foreach(CinemachineVirtualCamera vcam in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            vcam.Priority = 0;
        }
        targetVCam.Priority = 1;
    }

    public void OnInit(string stateName)
    {
        targetVCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void OnUpdate()
    {
    }
}
