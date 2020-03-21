using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "app_config.asset")]
public class AppConfig : ScriptableObject
{
    public float minMoney => _minMoney;
    [SerializeField] float _minMoney;

    public float maxMoney => _maxMoney;
    [SerializeField] float _maxMoney;

    public float moneySliderChangeTime => _moneySliderChangeTime;
    [SerializeField] float _moneySliderChangeTime;

    public List<MoneyPrefabTuple> lootBoxPrefabs;

    public static AppConfig current;

    public void Init()
    {
        lootBoxPrefabs = lootBoxPrefabs.OrderBy(p => p.money).ToList();
    }
}
