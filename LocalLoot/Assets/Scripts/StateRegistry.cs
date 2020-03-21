using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class StateRegistryEntry
{
    public string stateName => _stateName;
    [SerializeField] string _stateName;

    public List<MonoBehaviour> targetScripts;
}

