using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxAwesomenessController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Transform lootBoxImmediateParent;
    [SerializeField] GameObject lootBoxObject;
    MoneyPrefabTuple currentSelectedTuple;
    MoneyPrefabTuple changeCandidate;
    float changeTime;
    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        changeTime = 0.0f;
    }

    private void OnSliderValueChanged(float amount)
    {
        float moneyAmount = Mathf.Lerp(AppConfig.current.minMoney, AppConfig.current.maxMoney, amount);
        foreach(MoneyPrefabTuple tuple in AppConfig.current.lootBoxPrefabs)
        {
            if (tuple.money > moneyAmount)
                break;
            changeCandidate = tuple;
            
        }
        changeTime = 0.0f;
    }

    private void Update()
    {
        if (changeTime > AppConfig.current.moneySliderChangeTime)
        {
            ChangeLootboxPrefab();
        }
        changeTime += Time.deltaTime;
    }

    private void ChangeLootboxPrefab()
    {
        if (changeCandidate == currentSelectedTuple)
            return;

        currentSelectedTuple = changeCandidate;
        if (lootBoxObject != null)
            Destroy(lootBoxObject);
        GameObject inst = Instantiate(currentSelectedTuple.prefab, lootBoxImmediateParent);
        inst.transform.localPosition = Vector3.zero;
        inst.transform.rotation = Quaternion.identity;
        lootBoxObject = inst;
    }
}
