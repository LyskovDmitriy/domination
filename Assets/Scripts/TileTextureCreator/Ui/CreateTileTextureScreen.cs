using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.Ui;


namespace Generator.Ui
{
    public class CreateTileTextureScreen : UiUnit
    {
        [SerializeField] private TMP_InputField nameInputField = default;
        [SerializeField] private TMP_InputField xInputField = default;
        [SerializeField] private TMP_InputField yInputField = default;
        [SerializeField] private Button createButton = default;
        [SerializeField] private Button cancelButton = default;

        private Action onHidden;


        private void Awake()
        {
            cancelButton.onClick.AddListener(Hide);

            createButton.onClick.AddListener(() =>
            {
                if (int.TryParse(xInputField.text, out int x) && int.TryParse(yInputField.text, out int y))
                {
                    TileTexturesHolder.CreateTexture(nameInputField.text, new Vector2Int(x, y));
                }
                Hide();
            });
        }


        public void Show(Action onHidden)
        {
            base.Show();

            this.onHidden = onHidden;
        }


        public override void Hide()
        {
            base.Hide();

            onHidden?.Invoke();
            onHidden = null;
        }
    }
}
