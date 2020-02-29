using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceAsset<T> where T : Object
{
    private T instance;
    private string path;


    public T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>(path);
            }

            return instance;
        }
    }


    public ResourceAsset(string path)
    {
        this.path = path;
    }
}
