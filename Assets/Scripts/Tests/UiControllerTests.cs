using System;
using NUnit.Framework;
using Domination.Ui;
using Utils.Ui;


[TestOf(typeof(UiController))]
public class UiControllerTests
{
    [Test]
    public void TryLoadScreens()
    {
        var values = Enum.GetValues(typeof(ScreenType));
        foreach (var value in values)
        {
            var type = (ScreenType)value;
            if (type != ScreenType.None)
            {
                var screen = UiController.LoadScreen(type);
                Assert.IsNotNull(screen, $"Screen of type {type} can't be loaded");
            }
        }
    }
}
