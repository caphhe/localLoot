using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    #region Instantiate Methods
    public static GameObject Instantiate(GameObject obj)
    {
        
        GameObject inst = GameObject.Instantiate(obj);
        ProcessInstantiate(
            inst,
            null,
            inst.transform.position,
            Quaternion.identity);
        return inst;
    }

    public static GameObject Instantiate(GameObject obj, Transform parent)
    {
        
        GameObject inst = GameObject.Instantiate(obj, parent);
        ProcessInstantiate(
            inst,
            parent,
            inst.transform.position,
            Quaternion.identity);
        return inst;
    }
    public static GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {
        
        GameObject inst = GameObject.Instantiate(obj, position, rotation);
        ProcessInstantiate(
            inst,
            null,
            position,
            rotation);
        return inst;
    }
    #endregion

    private static void ProcessInstantiate(GameObject inst, Transform parent, Vector3 position, Quaternion rotation)
    {
        SceneManager sm = FindObjectOfType<SceneManager>();
        foreach (MonoBehaviour mb in inst.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (mb is ISceneUpdatable &&
                sm != null)
            {
                sm.ProcessSceneUpdatable(mb);
            }

        }
    }
}
