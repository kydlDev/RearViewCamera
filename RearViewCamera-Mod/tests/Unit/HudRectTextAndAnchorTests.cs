using RearViewCamera.Logic;

namespace RearViewCamera.Tests.Unit;

/// <summary>Exact log text of a placed rectangle, the minimap anchor really sets the right edge, and the clamp flag at its 0.01 px boundary (found by mutation testing).</summary>
[TestFixture]
public class HudRectTextAndAnchorTests
{
    [Test]
    public void ToString_UnderMinimap_NamesSizePlaceAndAnchor()
    {
        var r = new HudRect { X = 1100f, Y = 310f, Width = 800f, Height = 450f, UnderMinimap = true };

        Assert.That(r.ToString(), Is.EqualTo("800x450 px at (1100, 310) under the minimap"));
    }

    [Test]
    public void ToString_NoMinimapIconsAndClamped_ListsEveryReason()
    {
        var r = new HudRect { X = 0f, Y = 20f, Width = 640f, Height = 360f, UnderIcons = true, Clamped = true };

        Assert.That(r.ToString(), Is.EqualTo("640x360 px at (0, 20) top-right (no minimap), below the status icons, pushed inside the screen"));
    }

    [Test]
    public void Place_UnderMinimap_RightEdgeIsTheMinimapsNotTheScreens()
    {
        HudRect r = SmallCameraLayout.Place(new ScreenSize(1920, 1080), 450, new MinimapAnchor(1500f, 300f, 10f, 20f), 0f, 0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(r.UnderMinimap, Is.True);
            Assert.That(r.Right, Is.EqualTo(1500f));
        }
    }

    [Test]
    public void Place_ClampOfExactlyOneHundredthOfAPixel_IsNotReported_horizontal()
    {
        HudRect r = SmallCameraLayout.Place(new ScreenSize(1920, 1080), 450, new MinimapAnchor(800f, 300f, 10f, 20f), -0.01f, 0f);

        Assert.That(r.Clamped, Is.False);
    }

    [Test]
    public void Place_ClampOfExactlyOneHundredthOfAPixel_IsNotReported_vertical()
    {
        HudRect r = SmallCameraLayout.Place(new ScreenSize(1920, 1080), 450, new MinimapAnchor(1500f, -10f, 10f, 20f), 0f, -0.01f);

        Assert.That(r.Clamped, Is.False);
    }
}
