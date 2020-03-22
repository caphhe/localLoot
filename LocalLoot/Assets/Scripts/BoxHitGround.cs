using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxHitGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<SceneManager>().gameObject.GetComponent<Animator>().SetBool("LootBoxOnGround", true);
    }
}
