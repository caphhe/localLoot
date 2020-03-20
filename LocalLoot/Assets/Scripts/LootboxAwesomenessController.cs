using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxAwesomenessController : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float amount
        )
    {
    }
}
