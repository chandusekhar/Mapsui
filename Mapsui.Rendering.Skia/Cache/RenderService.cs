﻿using Mapsui.Styles;

namespace Mapsui.Rendering.Skia.Cache;

public sealed class RenderService : IRenderService
{
    public RenderService(int vectorCacheCapacity = 10000)
    {
        SymbolCache = new SymbolCache();
        TileCache = new TileCache();
        LabelCache = new LabelCache();
        ImageSourceCache = ImageSourceCache.Instance;
        VectorCache = new VectorCache(this, vectorCacheCapacity);
        SpriteCache = new SpriteCache();
    }

    public SymbolCache SymbolCache { get; }
    public IVectorCache VectorCache { get; }
    public ITileCache TileCache { get; }
    public LabelCache LabelCache { get; }
    public ImageSourceCache ImageSourceCache { get; }
    public ISpriteCache SpriteCache { get; }

    public void Dispose()
    {
        LabelCache.Dispose();
        SymbolCache.Dispose();
        VectorCache.Dispose();
        TileCache.Dispose();
        ImageSourceCache.Dispose();
        SpriteCache.Dispose();
    }
}
