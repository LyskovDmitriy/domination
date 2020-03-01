using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Level level = default;


    private void Awake()
    {
        level.Create();
    }
}
