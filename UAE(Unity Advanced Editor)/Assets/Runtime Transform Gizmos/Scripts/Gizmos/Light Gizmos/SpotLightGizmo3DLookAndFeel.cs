using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTG
{
    [Serializable]
    public class SpotLightGizmo3DLookAndFeel
    {
        [SerializeField]
        private Color _wireColor = DefaultWireColor;
        [SerializeField]
        private GizmoCap2DLookAndFeel _tickLookAndFeel = new GizmoCap2DLookAndFeel();
        [SerializeField]
        private GizmoCap2DLookAndFeel _dirSnapTickLookAndFeel = new GizmoCap2DLookAndFeel();
        [SerializeField]
        private Color _dirSnapSegmentColor = DefaultDirSnapSegmentColor;

        public static Color DefaultWireColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static Color DefaultTickColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static float DefaultTickQuadWidth { get { return 6.0f; } }
        public static float DefaultTickQuadHeight { get { return 6.0f; } }
        public static float DefaultTickCircleRadius { get { return 3.0f; } }
        public static GizmoCap2DType DefaultTickCapType { get { return GizmoCap2DType.Quad; } }
        public static Color DefaultTickHoveredColor { get { return RTSystemValues.HoveredAxisColor; } }
        public static Color DefaultTickBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultTickHoveredBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultDirSnapSegmentColor { get { return Color.green; } }
        public static Color DefaultDirSnapTickColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static float DefaultDirSnapTickQuadWidth { get { return 6.0f; } }
        public static float DefaultDirSnapTickQuadHeight { get { return 6.0f; } }
        public static float DefaultDirSnapTickCircleRadius { get { return 3.0f; } }
        public static GizmoCap2DType DefaultDirSnapTickCapType { get { return GizmoCap2DType.Quad; } }
        public static Color DefaultDirSnapTickHoveredColor { get { return RTSystemValues.HoveredAxisColor; } }
        public static Color DefaultDirSnapTickBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultDirSnapTickHoveredBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }

        public Color WireColor { get { return _wireColor; } }
        public Color DirSnapSegmentColor { get { return _dirSnapSegmentColor; } }
        public Color DirSnapTickBorderColor { get { return _dirSnapTickLookAndFeel.BorderColor; } }
        public Color DirSnapTickHoveredColor { get { return _dirSnapTickLookAndFeel.HoveredColor; } }
        public Color DirSnapTickHoveredBorderColor { get { return _dirSnapTickLookAndFeel.HoveredBorderColor; } }
        public GizmoCap2DType DirSnapTickType { get { return _dirSnapTickLookAndFeel.CapType; } }
        public float DirSnapTickQuadWidth { get { return _dirSnapTickLookAndFeel.QuadWidth; } }
        public float DirSnapTickQuadHeight { get { return _dirSnapTickLookAndFeel.QuadHeight; } }
        public float DirSnapTickCircleRadius { get { return _dirSnapTickLookAndFeel.CircleRadius; } }
        public Color TickBorderColor { get { return _tickLookAndFeel.BorderColor; } }
        public Color TickHoveredColor { get { return _tickLookAndFeel.HoveredColor; } }
        public Color TickHoveredBorderColor { get { return _tickLookAndFeel.HoveredBorderColor; } }
        public GizmoCap2DType TickType { get { return _tickLookAndFeel.CapType; } }
        public float TickQuadWidth { get { return _tickLookAndFeel.QuadWidth; } }
        public float TickQuadHeight { get { return _tickLookAndFeel.QuadHeight; } }
        public float TickCircleRadius { get { return _tickLookAndFeel.CircleRadius; } }

        public SpotLightGizmo3DLookAndFeel()
        {
            SetDirSnapTickColor(DefaultDirSnapTickColor);

            SetDirSnapTickHoveredColor(DefaultDirSnapTickHoveredColor);
            SetDirSnapTickBorderColor(DefaultDirSnapTickBorderColor);
            SetDirSnapTickHoveredBorderColor(DefaultDirSnapTickHoveredBorderColor);

            SetDirSnapTickQuadWidth(DefaultDirSnapTickQuadWidth);
            SetDirSnapTickQuadHeight(DefaultDirSnapTickQuadHeight);
            SetDirSnapTickCircleRadius(DefaultDirSnapTickCircleRadius);
            SetDirSnapTickType(DefaultDirSnapTickCapType);

            SetTickColor(DefaultTickColor);

            SetTickHoveredColor(DefaultTickHoveredColor);
            SetTickBorderColor(DefaultTickBorderColor);
            SetTickHoveredBorderColor(DefaultTickHoveredBorderColor);

            SetTickQuadWidth(DefaultTickQuadWidth);
            SetTickQuadHeight(DefaultTickQuadHeight);
            SetTickCircleRadius(DefaultTickCircleRadius);
            SetTickType(DefaultTickCapType);
        }

        public void SetWireColor(Color color)
        {
            _wireColor = color;
        }

        public void SetTickColor(Color color)
        {
            _tickLookAndFeel.Color = color;
        }

        public void SetTickBorderColor(Color color)
        {
            _tickLookAndFeel.BorderColor = color;
        }

        public void SetTickHoveredColor(Color color)
        {
            _tickLookAndFeel.HoveredColor = color;
        }

        public void SetTickHoveredBorderColor(Color color)
        {
            _tickLookAndFeel.HoveredBorderColor = color;
        }

        public void SetTickType(GizmoCap2DType tickType)
        {
            if (IsTickTypeAllowed(tickType))
            {
                _tickLookAndFeel.CapType = tickType;
            }
        }

        public void SetTickQuadWidth(float width)
        {
            _tickLookAndFeel.QuadWidth = width;
        }

        public void SetTickQuadHeight(float height)
        {
            _tickLookAndFeel.QuadHeight = height;
        }

        public void SetTickCircleRadius(float radius)
        {
            _tickLookAndFeel.CircleRadius = radius;
        }

        public List<Enum> GetAllowedTickTypes()
        {
            return new List<Enum>() { GizmoCap2DType.Circle, GizmoCap2DType.Quad };
        }

        public bool IsTickTypeAllowed(GizmoCap2DType tickType)
        {
            return tickType == GizmoCap2DType.Circle || tickType == GizmoCap2DType.Quad;
        }

        public void SetDirSnapSegmentColor(Color color)
        {
            _dirSnapSegmentColor = color;
        }

        public void SetDirSnapTickColor(Color color)
        {
            _dirSnapTickLookAndFeel.Color = color;
        }

        public void SetDirSnapTickBorderColor(Color color)
        {
            _dirSnapTickLookAndFeel.BorderColor = color;
        }

        public void SetDirSnapTickHoveredColor(Color color)
        {
            _dirSnapTickLookAndFeel.HoveredColor = color;
        }

        public void SetDirSnapTickHoveredBorderColor(Color color)
        {
            _dirSnapTickLookAndFeel.HoveredBorderColor = color;
        }

        public void SetDirSnapTickType(GizmoCap2DType tickType)
        {
            _dirSnapTickLookAndFeel.CapType = tickType;
        }

        public void SetDirSnapTickQuadWidth(float width)
        {
            _dirSnapTickLookAndFeel.QuadWidth = width;
        }

        public void SetDirSnapTickQuadHeight(float height)
        {
            _dirSnapTickLookAndFeel.QuadHeight = height;
        }

        public void SetDirSnapTickCircleRadius(float radius)
        {
            _dirSnapTickLookAndFeel.CircleRadius = radius;
        }

        public void ConnectDirSnapTickLookAndFeel(GizmoCap2D tick)
        {
            tick.SharedLookAndFeel = _dirSnapTickLookAndFeel;
        }

        public void ConnectTickLookAndFeel(GizmoCap2D tick)
        {
            tick.SharedLookAndFeel = _tickLookAndFeel;
        }
    }
}