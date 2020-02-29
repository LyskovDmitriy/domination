using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateTileTextureScreen : UiUnit<object>
{
    [SerializeField] private TMP_InputField nameInputField = default;    
    [SerializeField] private TMP_InputField xInputField = default;    
    [SerializeField] private TMP_InputField yInputField = default;
    [SerializeField] private Button createButton = default;
    [SerializeField] private Button cancelButton = default;


    private void Awake()
    {
        cancelButton.onClick.AddListener(() => Hide(null));

        createButton.onClick.AddListener(() =>
        {
            if (int.TryParse(xInputField.text, out int x) && int.TryParse(yInputField.text, out int y))
            {
                TileTexturesHolder.CreateTexture(nameInputField.text, new Vector2Int(x, y));
            }
            Hide(null);
        });
    }
}
