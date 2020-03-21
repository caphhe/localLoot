using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{

    public static AppManager instance
    {
        get
        {
            return _instance;
        }
    }
    static AppManager _instance;

    [SerializeField] AppConfig config;

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            AppConfig.current = config;
            _instance = this;
        }
        else
            Destroy(this.gameObject);
    }
}
