﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;

namespace Mapsui.UI
{
    /// <summary>
    /// This Viewport limiter will allways keep the map within the zoom and pan limits.
    /// An exception is rotation. 
    /// </summary>
   public class ViewportLimiterKeepWithin : IViewportLimiter
    {
        /// <summary>
        /// Set this property in combination KeepCenterWithinExtents or KeepViewportWithinExtents.
        /// If PanLimits is not set, Map.Extent will be used as restricted extent.
        /// </summary>
        public BoundingBox PanLimits { get; set; }

        /// <summary>
        /// Pair of the limits for the resolutions (smallest and biggest). If ZoomMode is set 
        /// to anything else than None, resolution is kept between these values.
        /// </summary>
        public MinMax ZoomLimits { get; set; }

        private MinMax GetExtremes(IReadOnlyList<double> resolutions)
        {
            if (resolutions == null || resolutions.Count == 0) return null;
            resolutions = resolutions.OrderByDescending(r => r).ToList();
            var mostZoomedOut = resolutions[0];
            var mostZoomedIn = resolutions[resolutions.Count - 1] * 0.5; // divide by two to allow one extra level to zoom-in
            return new MinMax(mostZoomedOut, mostZoomedIn);
        }

        public void Limit(IViewport viewport, IReadOnlyList<double> mapResolutions, BoundingBox mapEnvelope)
        {
            viewport.SetResolution(LimitResolution(viewport.Resolution, viewport.Width, viewport.Height, mapResolutions, mapEnvelope));
            LimitExtent(viewport,  mapEnvelope);
        }

        public double LimitResolution(double resolution, double screenWidth, double screenHeight,  
            IReadOnlyList<double> mapResolutions, BoundingBox mapEnvelope)
        {
            var resolutionExtremes = ZoomLimits ?? GetExtremes(mapResolutions);
            if (resolutionExtremes == null) return resolution;

            if (resolutionExtremes.Min > resolution) return resolutionExtremes.Min;
            
            // This is the ...AndAlwaysFillViewport part
            var viewportFillingResolution = CalculateResolutionAtWhichMapFillsViewport(screenWidth, screenHeight, mapEnvelope);
            if (viewportFillingResolution < resolutionExtremes.Min) return resolution; // Mission impossible. Can't adhere to both restrictions
            var limit = Math.Min(resolutionExtremes.Max, viewportFillingResolution);
            if (limit < resolution) return limit;
        
            return resolution;
        }

        private static double CalculateResolutionAtWhichMapFillsViewport(double screenWidth, double screenHeight, BoundingBox mapEnvelope)
        {
            return Math.Min(mapEnvelope.Width / screenWidth, mapEnvelope.Height / screenHeight);
        }

        public void LimitExtent(IViewport viewport, BoundingBox mapEnvelope)
        {
            var maxExtent = PanLimits ?? mapEnvelope;
            if (maxExtent == null)
            {
                // Can be null because both panLimits and Map.Extent can be null. 
                // The Map.Extent can be null if the extent of all layers is null
                return; 
            }

            var x = viewport.Center.X;

            if (MapWidthSpansViewport(maxExtent.Width, viewport.Width, viewport.Resolution)) // if it doesn't fit don't restrict
            {
                if (viewport.Extent.Left < maxExtent.Left)
                    x  += maxExtent.Left - viewport.Extent.Left;
                if (viewport.Extent.Right > maxExtent.Right)
                    x += maxExtent.Right - viewport.Extent.Right;
            }

            var y = viewport.Center.Y;
            if (MapHeightSpansViewport(maxExtent.Height, viewport.Height, viewport.Resolution)) // if it doesn't fit don't restrict
            {
                if (viewport.Extent.Top > maxExtent.Top)
                    y += maxExtent.Top - viewport.Extent.Top;
                if (viewport.Extent.Bottom < maxExtent.Bottom)
                    y += maxExtent.Bottom - viewport.Extent.Bottom;
            }
            viewport.SetCenter(x, y);
        }

        private static bool MapWidthSpansViewport(double extentWidth, double viewportWidth, double resolution)
        {
            var mapWidth = extentWidth / resolution; // in screen units
            return viewportWidth <= mapWidth;
        }

        private static bool MapHeightSpansViewport(double extentHeight, double viewportHeight, double resolution)
        {
            var mapHeight = extentHeight / resolution; // in screen units
            return viewportHeight <= mapHeight;
        }
    }
}
