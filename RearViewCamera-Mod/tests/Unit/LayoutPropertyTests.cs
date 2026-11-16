using FsCheck;
using RearViewCamera.Logic;

namespace RearViewCamera.Tests.Unit;

/// <summary>Property tests (FsCheck): sizes stay in range and the picture keeps its 16:9 shape for any screen and setting.</summary>
[TestFixture]
public class LayoutPropertyTests
{
    static readonly Gen<int> AnyInt = Gen.Choose(-5000, 10000);
    static readonly Gen<int> Screen = Gen.Choose(1, 8000);

    [Test]
    public void ClampHeight_IsInRange_AndIdempotent()
    {
        Props.Check(Prop.ForAll(AnyInt.ToArbitrary(), h =>
        {
            int c = SmallCameraLayout.ClampHeight(h);
            return c >= SmallCameraLayout.MinHeight && c <= SmallCameraLayout.MaxHeight && SmallCameraLayout.ClampHeight(c) == c;
        }));
    }

    [Test]
    public void WidthFor_IsSixteenNinthsOfTheClampedHeight_AndNeverShrinksWithHeight()
    {
        Props.Check(Prop.ForAll(AnyInt.ToArbitrary(), AnyInt.ToArbitrary(), (a, b) =>
        {
            int lo = System.Math.Min(a, b), hi = System.Math.Max(a, b);
            int h = SmallCameraLayout.ClampHeight(a);
            return System.Math.Abs(SmallCameraLayout.WidthFor(a) - h * 16.0 / 9.0) <= 0.5 && SmallCameraLayout.WidthFor(lo) <= SmallCameraLayout.WidthFor(hi);
        }));
    }

    [Test]
    public void FullScreenSize_NeverExceedsTheScreenOrTheCap_AndKeepsTheAspect()
    {
        Props.Check(Prop.ForAll(Screen.ToArbitrary(), Screen.ToArbitrary(), Gen.Choose(-100, 4000).ToArbitrary(), (sw, sh, cap) =>
        {
            int w, h;
            FullScreenSize.For(sw, sh, cap, out w, out h);
            int swc = System.Math.Max(16, sw), shc = System.Math.Max(16, sh);
            bool aspect = System.Math.Abs(w - swc * (double)h / shc) <= 0.5;
            return h >= 16 && h <= shc && (h <= cap || h == 16) && w >= 0 && aspect;
        }));
    }

    [Test]
    public void KeyConflict_NeverUsesATakenKey_AndUnboundStaysUnbound()
    {
        Props.Check(Prop.ForAll(Gen.Choose(0, 20).ToArbitrary(), Gen.Choose(1, 20).ToArbitrary(), Gen.Choose(0, 20).ToArbitrary(), (cfg, bak, taken) =>
        {
            int used;
            KeyState s = KeyConflict.Resolve(cfg, bak, k => k == taken, out used);
            if (cfg == 0) return s == KeyState.Unbound && used == 0;
            return used == 0 ? s == KeyState.Off : used != taken && (s == KeyState.Free ? used == cfg : used == bak);
        }));
    }
}
