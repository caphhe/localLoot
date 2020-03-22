using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxOpenController : MonoBehaviour
{
    private void OnMouseDown()
    {
        FindObjectOfType<SceneManager>().GetComponent<Animator>().SetBool("DonationAmountConfirmed", true);
    }
}
