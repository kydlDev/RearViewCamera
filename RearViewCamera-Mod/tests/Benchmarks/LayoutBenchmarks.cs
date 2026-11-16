using BenchmarkDotNet.Attributes;
using RearViewCamera.Logic;

namespace RearViewCamera.Benchmarks;

/// <summary>Layout maths for the rear-view picture: the small camera is placed again whenever the screen, the minimap or a setting changes; the key logic runs every frame.</summary>
[MemoryDiagnoser]
public class LayoutBenchmarks
{
    ScreenSize _screen;
    MinimapAnchor _map;
    RearViewKeyLogic _keys;
    RearViewKeys _held;
    HudRect _placed;

    [GlobalSetup]
    public void Setup()
    {
        _screen = new ScreenSize(2560, 1440);
        _map = new MinimapAnchor(30f, 30f, 8f, 10f);
        _keys = new RearViewKeyLogic(null);
        _held = default(RearViewKeys);
        _placed = SmallCameraLayout.Place(_screen, 270, _map, 0f, 0f);
    }

    [Benchmark]
    public bool PlaceSmall() => SmallCameraLayout.Place(_screen, 270, _map, 0f, 0f).SameAs(_placed);

    [Benchmark]
    public int ClampAndWidth() => SmallCameraLayout.WidthFor(SmallCameraLayout.ClampHeight(333));

    [Benchmark]
    public int FullScreen()
    {
        int w, h;
        FullScreenSize.For(2560, 1440, 1080, out w, out h);
        return w + h;
    }

    [Benchmark]
    public int ResolveKey()
    {
        int used;
        return (int)KeyConflict.Resolve(7, 9, k => k == 7, out used);
    }

    [Benchmark]
    public int KeyStepIdle() => (int)_keys.Step(_held, SmallCameraMode.Toggle);
}
