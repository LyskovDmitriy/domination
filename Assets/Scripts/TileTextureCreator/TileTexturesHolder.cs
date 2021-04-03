using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu]
public class TileTexturesHolder : ScriptableObject
{
    private const string TexturesFolderPath = "Assets/Data/TileTextures";

    private static readonly ResourceAsset<TileTexturesHolder> asset = new ResourceAsset<TileTexturesHolder>("TileTexturesHolder");
    
    [SerializeField] private List<TileTextureData> tileTextures = default;


    public static List<TileTextureData> Textures => asset.Value.tileTextures;


    public static void CreateTexture(string name, Vector2Int resolution)
    {
        TileTextureData texture = ScriptableObject.CreateInstance<TileTextureData>();
        texture.SetResolution(resolution);
        AssetDatabase.CreateAsset(texture, TexturesFolderPath + "/" + name + ".asset");
        asset.Value.tileTextures.Add(texture);
        EditorUtility.SetDirty(asset.Value);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    public static void Delete(TileTextureData texture)
    {
        Textures.Remove(texture);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(texture));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
