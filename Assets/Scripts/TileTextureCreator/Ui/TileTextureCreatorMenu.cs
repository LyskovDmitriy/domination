using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TileTextureCreatorMenu : MonoBehaviour
{
    [SerializeField] private TileTextureScreen textureViewScreen = default;
    [SerializeField] private GameObject menuRoot = default;
    [SerializeField] private Button showTextureViewScreenButton = default;
    [SerializeField] private Button showGenerationScreenButton = default;
    [SerializeField] private GenerateMapScreen mapGenerationScreen = default;


    private void Awake()
    {
        showTextureViewScreenButton.onClick.AddListener(() =>
        {
            menuRoot.SetActive(false);
            textureViewScreen.Show((_) => menuRoot.SetActive(true));
        });

        showGenerationScreenButton.onClick.AddListener(() =>
        {
            menuRoot.SetActive(false);
            mapGenerationScreen.Show((_) => menuRoot.SetActive(true));
        });
    }
}
