﻿using Mapsui.Widgets.BoxWidgets;
using System;

namespace Mapsui.Widgets.ButtonWidget;

/// <summary>
/// Widget that shows a button with an icon
/// </summary>
/// <remarks>
/// With this, the user could add buttons with SVG icons to the map.
/// 
/// Usage
/// To show a IconButtonWidget, add a instance of the IconButtonWidget to Map.Widgets by
/// 
///   map.Widgets.Add(new IconButtonWidget(map, picture));
///   
/// Customize
/// Picture: SVG image to display for button
/// Rotation: Value for rotation in degrees
/// Opacity: Opacity of button
/// </remarks>
public class IconButtonWidget : BoxWidget, ITouchableWidget
{
    /// <summary>
    /// Event handler which is called, when the button is touched
    /// </summary>
    public event EventHandler<WidgetTouchedEventArgs>? Touched;

    private double _paddingX = 0;

    /// <summary>
    /// Padding left and right for icon inside the Widget
    /// </summary>
    public double PaddingX
    {
        get => _paddingX;
        set
        {
            if (_paddingX == value)
                return;
            _paddingX = value;
            OnPropertyChanged();
        }
    }

    private double _paddingY = 0;

    /// <summary>
    /// Padding left and right for icon inside the Widget
    /// </summary>
    public double PaddingY
    {
        get => _paddingY;
        set
        {
            if (_paddingY == value)
                return;
            _paddingY = value;
            OnPropertyChanged();
        }
    }

    private string? _svgImage;

    /// <summary>
    /// SVG image to show for button
    /// </summary>
    public string? SvgImage
    {
        get => _svgImage;
        set
        {
            if (_svgImage == value)
                return;

            _svgImage = value;
            Picture = null;
            OnPropertyChanged();
        }
    }

    private object? _picture;

    /// <summary>
    /// Object for prerendered image. For internal use only.
    /// </summary>
    public object? Picture
    {
        get => _picture;
        set
        {
            if (Equals(value, _picture)) return;
            _picture = value;
            OnPropertyChanged();
        }
    }

    private double _rotation;

    /// <summary>
    /// Rotation of the SVG image
    /// </summary>
    public double Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation == value)
                return;
            _rotation = value;
            OnPropertyChanged();
        }
    }

    private double _opacity = 0.8f;

    /// <summary>
    /// Opacity of background, frame and signs
    /// </summary>
    public double Opacity
    {
        get => _opacity;
        set
        {
            if (_opacity == value)
                return;
            _opacity = value;
            OnPropertyChanged();
        }
    }

    public TouchableAreaType TouchableArea => TouchableAreaType.Widget;

    public bool HandleWidgetTouched(Navigator navigator, MPoint position, WidgetTouchedEventArgs args)
    {
        Touched?.Invoke(this, args);

        return args.Handled;
    }

    public bool HandleWidgetTouching(Navigator navigator, MPoint position, WidgetTouchedEventArgs args)
    {
        return false;
    }

    public bool HandleWidgetMoving(Navigator navigator, MPoint position, WidgetTouchedEventArgs args)
    {
        return false;
    }
}
