using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelUi : UiUnit<object>
{
    public static readonly ResourceBehavior<LevelUi> Prefab = new ResourceBehavior<LevelUi>("Ui/LevelUi");

    public static event Action OnEndTurnButtonClick;

    [SerializeField] private Button endTurnButton = default;
    [SerializeField] private TextMeshProUGUI playerMoneyLabel = default;


    private void Awake()
    {
        endTurnButton.onClick.AddListener(() => OnEndTurnButtonClick?.Invoke());
        Player.OnCoinsCountChange += OnPlayerCoinsCountChange;
    }


    private void OnDestroy()
    {
        Player.OnCoinsCountChange -= OnPlayerCoinsCountChange;
    }


    public void Show(Level level, Action<object> onHidden = null)
    {
        Show(onHidden);

        OnPlayerCoinsCountChange(level.Player.Coins);
    }


    private void OnPlayerCoinsCountChange(int moneyCount)
    {
        playerMoneyLabel.text = moneyCount.ToString();
    }
}
