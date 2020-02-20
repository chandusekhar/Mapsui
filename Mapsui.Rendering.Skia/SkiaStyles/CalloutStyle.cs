﻿using Mapsui.Geometries;
using Mapsui.Styles;
using Mapsui.Widgets;
using System.Runtime.CompilerServices;

namespace Mapsui.Rendering.Skia
{
    /// <summary>
    /// Type of CalloutStyle
    /// </summary>
    public enum CalloutType
    {
        /// <summary>
        /// Only one line is shown
        /// </summary>
        Single,
        /// <summary>
        /// Header and detail is shown
        /// </summary>
        Detail,
        /// <summary>
        /// Content is custom, the bitmap given in Content is shown
        /// </summary>
        Custom,
    }

    /// <summary>
    /// Determins, where the pointer is
    /// </summary>
    public enum ArrowAlignment
    {
        /// <summary>
        /// Callout arrow is at bottom side of bubble
        /// </summary>
        Bottom,
        /// <summary>
        /// Callout arrow is at left side of bubble
        /// </summary>
        Left,
        /// <summary>
        /// Callout arrow is at top side of bubble
        /// </summary>
        Top,
        /// <summary>
        /// Callout arrow is at right side of bubble
        /// </summary>
        Right,
    }

    public class CalloutStyle : SymbolStyle
    {
        private CalloutType _type = CalloutType.Single;
        private bool _invalidated; // todo: set to false after rendering.
        private ArrowAlignment _arrowAlignment = ArrowAlignment.Bottom;
        private float _arrowWidth = 8f;
        private float _arrowHeight = 8f;
        private float _arrowPosition = 0.5f;
        private float _rectRadius = 4f;
        private float _shadowWidth = 2f;
        private BoundingBox _padding = new BoundingBox(3f, 3f, 3f, 3f);
        private Color _color = Color.Black;
        private Color _backgroundColor = Color.White;
        private float _strokeWidth = 1f;
        private int _content = -1;
        private Point _offset = new Point(0, 0);
        private double _rotation = 0;
        private string _title;
        private string _subtitle;
        private Alignment _titleTextAlignment;
        private Alignment _subtitleTextAlignment;
        private double _spacing;
        private double _maxWidth;
        
        public new static double DefaultWidth { get; set; } = 100;
        public new static double DefaultHeight { get; set; } = 30;

        public CalloutStyle()
        {
        }

        /// <summary>
        /// Type of Callout
        /// </summary>
        /// <remarks>
        /// Could be single, detail or custom. The last is a bitmap id for an owner drawn image.
        /// </remarks>
        public CalloutType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Offset position in pixels of Callout
        /// </summary>
        public Point Offset
        {
            get => _offset;
            set
            {
                if (!_offset.Equals(value))
                {
                    _offset = value;
                    SymbolOffset = new Offset(_offset.X, _offset.Y);
                }
            }
        }

        /// <summary>
        /// BoundingBox relative to offset point
        /// </summary>
        public BoundingBox BoundingBox = new BoundingBox();

        /// <summary>
        /// Gets or sets the rotation of the Callout in degrees (clockwise is positive)
        /// </summary>
        public double Rotation
        { 
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    SymbolRotation = _rotation;
                }
            }
        }

        /// <summary>
        /// Anchor position of Callout
        /// </summary>
        public ArrowAlignment ArrowAlignment 
        { 
            get => _arrowAlignment; 
            set
            {
                if (value != _arrowAlignment)
                {
                    _arrowAlignment = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Width of opening of anchor of Callout
        /// </summary>
        public float ArrowWidth
        {
            get => _arrowWidth;
            set
            {
                if (value != _arrowWidth)
                {
                    _arrowWidth = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Height of anchor of Callout
        /// </summary>
        public float ArrowHeight
        {
            get => _arrowHeight;
            set
            {
                if (value != _arrowHeight)
                {
                    _arrowHeight = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Relative position of anchor of Callout on the side given by AnchorType
        /// </summary>
        public float ArrowPosition
        {
            get => _arrowPosition;
            set
            {
                if (value != _arrowPosition)
                {
                    _arrowPosition = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Color of stroke around Callout
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// BackgroundColor of Callout
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Stroke width of frame around Callout
        /// </summary>
        public float StrokeWidth
        {
            get => _strokeWidth;
            set
            {
                if (value != _strokeWidth)
                {
                    _strokeWidth = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Radius of rounded corners of Callout
        /// </summary>
        public float RectRadius
        {
            get => _rectRadius;
            set
            {
                if (value != _rectRadius)
                {
                    _rectRadius = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Padding around content of Callout
        /// </summary>
        public BoundingBox Padding
        {
            get => _padding;
            set
            {
                if (value != _padding)
                {
                    _padding = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Width of shadow around Callout
        /// </summary>
        public float ShadowWidth
        {
            get => _shadowWidth;
            set
            {
                if (value != _shadowWidth)
                {
                    _shadowWidth = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Content of Callout
        /// </summary>
        /// <remarks>
        /// Is a BitmapId of a save image
        /// </remarks>
        public int Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Content of Callout title label
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Font color to render title
        /// </summary>
        public Color TitleFontColor { get; set; }

        /// <summary>
        /// Text alignment of title
        /// </summary>
        public Alignment TitleTextAlignment
        {
            get => _titleTextAlignment;
            set
            {
                if (_titleTextAlignment != value)
                {
                    _titleTextAlignment = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Content of Callout subtitle label
        /// </summary>
        public string Subtitle
        {
            get => _subtitle;
            set
            {
                if (_subtitle != value)
                {
                    _subtitle = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Font color to render subtitle
        /// </summary>
        public Color SubtitleFontColor { get; set; }

        /// <summary>
        /// Text alignment of subtitle
        /// </summary>
        public Alignment SubtitleTextAlignment
        {
            get => _subtitleTextAlignment;
            set
            {
                if (_subtitleTextAlignment != value)
                {
                    _subtitleTextAlignment = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Space between Title and Subtitel of Callout
        /// </summary>
        public double Spacing
        {
            get => _spacing;
            set
            {
                if (_spacing != value)
                {
                    _spacing = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// MaxWidth for Title and Subtitel of Callout
        /// </summary>
        public double MaxWidth
        {
            get => _maxWidth;
            set
            {
                if (_maxWidth != value)
                {
                    _maxWidth = value;
                    _invalidated = true;
                    OnPropertyChanged();
                }
            }
        }

        public int InternalContent { get; set; } = -1;
        public Font TitleFont { get; set; } = new Font(); // todo set invalidate
        public Font SubtitleFont { get; set; } = new Font();

        /// <summary>
        /// Something changed, so create new image
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (_content < 0 && _type == CalloutType.Custom)
                return;

            // Create content of this Callout
            if (propertyName.Equals(nameof(Title))
                || propertyName.Equals(nameof(TitleFont))
                || propertyName.Equals(nameof(TitleFontColor))
                || propertyName.Equals(nameof(TitleTextAlignment))
                || propertyName.Equals(nameof(Subtitle))
                || propertyName.Equals(nameof(SubtitleFont))
                || propertyName.Equals(nameof(SubtitleFontColor))
                || propertyName.Equals(nameof(SubtitleTextAlignment))
                || propertyName.Equals(nameof(Spacing))
                || propertyName.Equals(nameof(MaxWidth)))
            {
                CalloutStyleRenderer.UpdateContent(this);
            }

            CalloutStyleRenderer.RenderCallout(this);
        }
    }
}
