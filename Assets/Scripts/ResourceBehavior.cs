using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior<T> where T : Object
{
    private T instance;
    private string path;


    public T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate(Resources.Load<T>(path));
            }

            return instance;
        }
    }


    public ResourceBehavior(string path)
    {
        this.path = path;
    }
}
