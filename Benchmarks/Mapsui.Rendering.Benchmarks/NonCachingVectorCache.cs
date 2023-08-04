﻿using Mapsui.Styles;

namespace Mapsui.Rendering.Benchmarks;

public class NonCachingVectorCache : IVectorCache
{
    private readonly ISymbolCache _symbolCache;

    public NonCachingVectorCache(ISymbolCache symbolCache)
    {
        _symbolCache = symbolCache;
    }

    public T GetOrCreatePaint<T>(Pen? pen, float opacity, Func<Pen?, float, T> toPaint) where T : class
    {
        return toPaint(pen, opacity);
    }

    public T GetOrCreatePaint<T>(Brush? brush, float opacity, double rotation, Func<Brush?, float, double, ISymbolCache, T> toPaint) where T : class
    {
        return toPaint(brush, opacity, rotation, _symbolCache);
    }

    public T GetOrCreateRect<T>(Viewport viewport, Func<Viewport, T> toSkRect)
    {
        return toSkRect(viewport);
    }

    public TPath GetOrCreatePath<TPath, TGeometry>(Viewport viewport, TGeometry geometry, float lineWidth, Func<TGeometry, Viewport, float, TPath> toPath) where TPath : class where TGeometry : class
    {
        return toPath(geometry, viewport, lineWidth);
    }
}
