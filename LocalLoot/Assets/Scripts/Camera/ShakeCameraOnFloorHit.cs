using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraOnFloorHit : MonoBehaviour
{
    bool hasBeenShook = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenShook)
            return;
        hasBeenShook = true;
        CameraEffectsController.Shake();
    }
}
