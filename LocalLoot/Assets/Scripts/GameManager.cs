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
        ProcessInstantiate(
            obj,
            null,
            obj.transform.position, 
            Quaternion.identity);
        return GameObject.Instantiate(obj);
    }

    public static GameObject Instantiate(GameObject obj, Transform parent)
    {
        ProcessInstantiate(
            obj,
            parent,
            obj.transform.position, 
            Quaternion.identity);
        return GameObject.Instantiate(obj, parent);
    }
    public static GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {
        ProcessInstantiate(
            obj,
            null,
            position,
            rotation);
        return GameObject.Instantiate(obj, position, rotation);
    }
    #endregion

    private static void ProcessInstantiate(GameObject obj, Transform parent, Vector3 position, Quaternion rotation)
    {
        SceneManager sm = FindObjectOfType<SceneManager>();
        foreach (MonoBehaviour mb in obj.GetComponents<MonoBehaviour>())
        {
            if (mb is ISceneUpdatable &&
                sm != null)
            {
                sm.ProcessSceneUpdatable(mb);
            }

        }
    }
}
