using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTG
{
    [Serializable]
    public class DirectionalLightGizmo3DLookAndFeel
    {
        [SerializeField]
        private GizmoCap2DLookAndFeel _dirSnapTickLookAndFeel = new GizmoCap2DLookAndFeel();
        [SerializeField]
        private Color _lightRaysColor = DefaultLightRaysColor;
        [SerializeField]
        private Color _sourceCircleBorderColor = DefaultSourceCircleBorderColor;
        [SerializeField]
        private float _sourceCircleRadius = DefaultSourceCircleRadius;
        [SerializeField]
        private int _numLightRays = DefaultNumLightRays;
        [SerializeField]
        private float _lightRayLength = DefaultLightRayLength;
        [SerializeField]
        private Color _dirSnapSegmentColor = DefaultDirSnapSegmentColor;

        public static Color DefaultLightRaysColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static Color DefaultSourceCircleBorderColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static float DefaultSourceCircleRadius { get { return 2.0f; } }
        public static int DefaultNumLightRays { get { return 8; } }
        public static float DefaultLightRayLength { get { return 10.0f; } }
        public static Color DefaultDirSnapSegmentColor { get { return Color.green; } }
        public static Color DefaultDirSnapTickColor { get { return ColorEx.FromByteValues(210, 210, 138, 255); } }
        public static float DefaultDirSnapTickQuadWidth { get { return 6.0f; } }
        public static float DefaultDirSnapTickQuadHeight { get { return 6.0f; } }
        public static float DefaultDirSnapTickCircleRadius { get { return 3.0f; } }
        public static GizmoCap2DType DefaultDirSnapTickCapType { get { return GizmoCap2DType.Quad; } }
        public static Color DefaultDirSnapTickHoveredColor { get { return RTSystemValues.HoveredAxisColor; } }
        public static Color DefaultDirSnapTickBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultDirSnapTickHoveredBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }

        public Color LightRaysColor { get { return _lightRaysColor; } }
        public Color SourceCircleBorderColor { get { return _sourceCircleBorderColor; } }
        public float SourceCircleRadius { get { return _sourceCircleRadius; } }
        public int NumLightRays { get { return _numLightRays; } }
        public float LightRayLength { get { return _lightRayLength; } }
        public Color DirSnapSegmentColor { get { return _dirSnapSegmentColor; } }
        public Color DirSnapTickBorderColor { get { return _dirSnapTickLookAndFeel.BorderColor; } }
        public Color DirSnapTickHoveredColor { get { return _dirSnapTickLookAndFeel.HoveredColor; } }
        public Color DirSnapTickHoveredBorderColor { get { return _dirSnapTickLookAndFeel.HoveredBorderColor; } }
        public GizmoCap2DType DirSnapTickType { get { return _dirSnapTickLookAndFeel.CapType; } }
        public float DirSnapTickQuadWidth { get { return _dirSnapTickLookAndFeel.QuadWidth; } }
        public float DirSnapTickQuadHeight { get { return _dirSnapTickLookAndFeel.QuadHeight; } }
        public float DirSnapTickCircleRadius { get { return _dirSnapTickLookAndFeel.CircleRadius; } }

        public DirectionalLightGizmo3DLookAndFeel()
        {
            SetDirSnapTickColor(DefaultDirSnapTickColor);

            SetDirSnapTickHoveredColor(DefaultDirSnapTickHoveredColor);
            SetDirSnapTickBorderColor(DefaultDirSnapTickBorderColor);
            SetDirSnapTickHoveredBorderColor(DefaultDirSnapTickHoveredBorderColor);

            SetDirSnapTickQuadWidth(DefaultDirSnapTickQuadWidth);
            SetDirSnapTickQuadHeight(DefaultDirSnapTickQuadHeight);
            SetDirSnapTickCircleRadius(DefaultDirSnapTickCircleRadius);
            SetDirSnapTickType(DefaultDirSnapTickCapType);
        }

        public List<Enum> GetAllowedTickTypes()
        {
            return new List<Enum>() { GizmoCap2DType.Circle, GizmoCap2DType.Quad };
        }

        public bool IsTickTypeAllowed(GizmoCap2DType tickType)
        {
            return tickType == GizmoCap2DType.Circle || tickType == GizmoCap2DType.Quad;
        }

        public void SetNumLightRays(int numLightRays)
        {
            _numLightRays = Mathf.Max(3, numLightRays);
        }

        public void SetDirSnapSegmentColor(Color color)
        {
            _dirSnapSegmentColor = color;
        }

        public void SetLightRayLength(float length)
        {
            _lightRayLength = Mathf.Max(0.0f, length);
        }

        public void SetSourceCircleRadius(float radius)
        {
            _sourceCircleRadius = Mathf.Max(1e-4f, radius);
        }

        public void SetLightRaysColor(Color color)
        {
            _lightRaysColor = color;
        }

        public void SetSourceCircleBorderColor(Color color)
        {
            _sourceCircleBorderColor = color;
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
    }
}