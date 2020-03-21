using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class MoneyPrefabTuple
{
    public GameObject prefab => _prefab;
    [SerializeField] GameObject _prefab;

    public float money => _money;
    [SerializeField] float _money;
}

