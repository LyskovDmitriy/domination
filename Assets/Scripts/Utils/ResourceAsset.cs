using UnityEngine;


public class ResourceAsset<T> where T : Object
{
    private T value;
    private string path;


    public T Value
    {
        get
        {
            if (value == null)
            {
                value = Resources.Load<T>(path);
            }

            return value;
        }
    }


    public ResourceAsset(string path)
    {
        this.path = path;
    }
}
