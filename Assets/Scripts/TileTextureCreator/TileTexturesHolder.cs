using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu]
public class TileTexturesHolder : ScriptableObject
{
    private const string TexturesFolderPath = "Assets/Data/TileTextures";

    private static readonly ResourceAsset<TileTexturesHolder> asset = new ResourceAsset<TileTexturesHolder>("TileTexturesHolder");
    
    [SerializeField] private List<TileTextureData> tileTextures = default;


    public static List<TileTextureData> Textures => asset.Instance.tileTextures;


    public static void CreateTexture(string name, Vector2Int resolution)
    {
        TileTextureData texture = ScriptableObject.CreateInstance<TileTextureData>();
        texture.SetResolution(resolution);
        AssetDatabase.CreateAsset(texture, TexturesFolderPath + "/" + name + ".asset");
        asset.Instance.tileTextures.Add(texture);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    public static void Delete(TileTextureData texture)
    {
        Textures.Remove(texture);
        DestroyImmediate(texture, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
