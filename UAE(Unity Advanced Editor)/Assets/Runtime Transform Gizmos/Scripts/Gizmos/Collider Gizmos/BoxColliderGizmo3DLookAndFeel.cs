using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTG
{
    [Serializable]
    public class BoxColliderGizmo3DLookAndFeel
    {
        [SerializeField]
        private Color _wireColor = DefaultWireColor;
        [SerializeField]
        private GizmoCap2DLookAndFeel[] _tickLookAndFeel = new GizmoCap2DLookAndFeel[6];
        [SerializeField]
        private GizmoCap3DLookAndFeel _midCapLookAndFeel = new GizmoCap3DLookAndFeel();
        [SerializeField]
        private bool _isMidCapVisible = DefaultMidCapVisible;
        [SerializeField]
        private float _tickCullAlphaScale = DefaultTickCullAlphaScale;

        public static Color DefaultWireColor { get { return ColorEx.FromByteValues(153, 232, 144, 255); } }
        public static Color DefaultTickColor { get { return ColorEx.FromByteValues(153, 232, 144, 255); } }
        public static float DefaultTickQuadWidth { get { return 6.0f; } }
        public static float DefaultTickQuadHeight { get { return 6.0f; } }
        public static float DefaultTickCircleRadius { get { return 3.0f; } }
        public static float DefaultTickCullAlphaScale { get { return 0.3f; } }
        public static GizmoCap2DType DefaultTickCapType { get { return GizmoCap2DType.Quad; } }
        public static Color DefaultTickHoveredColor { get { return RTSystemValues.HoveredAxisColor; } }
        public static Color DefaultTickBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultTickHoveredBorderColor { get { return ColorEx.KeepAllButAlpha(Color.black, 0.0f); } }
        public static Color DefaultMidCapColor { get { return ColorEx.FromByteValues(153, 232, 144, 255); } }
        public static Color DefaultMidCapHoveredColor { get { return RTSystemValues.HoveredAxisColor; } }
        public static GizmoFillMode3D DefaultMidCapFillMode { get { return GizmoFillMode3D.Filled; } }
        public static GizmoCap3DType DefaultMidCapType { get { return GizmoCap3DType.Box; } }
        public static bool DefaultMidCapVisible { get { return true; } }
        public static float DefaultMidCapBoxWidth { get { return 0.7f; } }
        public static float DefaultMidCapBoxHeight { get { return 0.7f; } }
        public static float DefaultMidCapBoxDepth { get { return 0.7f; } }

        public Color WireColor { get { return _wireColor; } }
        public Color XTickColor { get { return GetTickLookAndFeel(0, AxisSign.Positive).Color; } }
        public Color YTickColor { get { return GetTickLookAndFeel(1, AxisSign.Positive).Color; } }
        public Color ZTickColor { get { return GetTickLookAndFeel(2, AxisSign.Positive).Color; } }
        public Color TickBorderColor { get { return GetTickLookAndFeel(0, AxisSign.Positive).BorderColor; } }
        public Color TickHoveredColor { get { return GetTickLookAndFeel(0, AxisSign.Positive).HoveredColor; } }
        public Color TickHoveredBorderColor { get { return GetTickLookAndFeel(0, AxisSign.Positive).HoveredBorderColor; } }
        public GizmoCap2DType TickType { get { return GetTickLookAndFeel(0, AxisSign.Positive).CapType; } }
        public float TickQuadWidth { get { return GetTickLookAndFeel(0, AxisSign.Positive).QuadWidth; } }
        public float TickQuadHeight { get { return GetTickLookAndFeel(0, AxisSign.Positive).QuadHeight; } }
        public float TickCircleRadius { get { return GetTickLookAndFeel(0, AxisSign.Positive).CircleRadius; } }
        public float TickCullAlphaScale { get { return _tickCullAlphaScale; } }
        public Color MidCapColor { get { return _midCapLookAndFeel.Color; } }
        public Color MidCapHoveredColor { get { return _midCapLookAndFeel.HoveredColor; } }
        public GizmoFillMode3D MidCapFillMode { get { return _midCapLookAndFeel.FillMode; } }
        public GizmoCap3DType MidCapType { get { return _midCapLookAndFeel.CapType; } }
        public bool IsMidCapVisible { get { return _isMidCapVisible; } }
        public float MidCapBoxWidth { get { return _midCapLookAndFeel.BoxWidth; } }
        public float MidCapBoxHeight { get { return _midCapLookAndFeel.BoxHeight; } }
        public float MidCapBoxDepth { get { return _midCapLookAndFeel.BoxDepth; } }

        public BoxColliderGizmo3DLookAndFeel()
        {
            for (int tickIndex = 0; tickIndex < _tickLookAndFeel.Length; ++tickIndex)
            {
                _tickLookAndFeel[tickIndex] = new GizmoCap2DLookAndFeel();
            }

            SetTickColor(0, DefaultTickColor);
            SetTickColor(1, DefaultTickColor);
            SetTickColor(2, DefaultTickColor);

            SetTickHoveredColor(DefaultTickHoveredColor);
            SetTickBorderColor(DefaultTickBorderColor);
            SetTickHoveredBorderColor(DefaultTickHoveredBorderColor);

            SetTickQuadWidth(DefaultTickQuadWidth);
            SetTickQuadHeight(DefaultTickQuadHeight);
            SetTickCircleRadius(DefaultTickCircleRadius);
            SetTickType(DefaultTickCapType);

            SetMidCapColor(DefaultMidCapColor);
            SetMidCapHoveredColor(DefaultMidCapHoveredColor);
            SetMidCapFillMode(DefaultMidCapFillMode);
            SetMidCapType(DefaultMidCapType);
            SetMidCapVisible(DefaultMidCapVisible);

            SetMidCapBoxWidth(DefaultMidCapBoxWidth);
            SetMidCapBoxHeight(DefaultMidCapBoxHeight);
            SetMidCapBoxDepth(DefaultMidCapBoxDepth);
        }

        public void SetMidCapBoxWidth(float width)
        {
            _midCapLookAndFeel.BoxWidth = width;
        }

        public void SetMidCapBoxHeight(float height)
        {
            _midCapLookAndFeel.BoxHeight = height;
        }

        public void SetMidCapBoxDepth(float depth)
        {
            _midCapLookAndFeel.BoxDepth = depth;
        }

        public void SetMidCapColor(Color color)
        {
            _midCapLookAndFeel.Color = color;
        }

        public void SetMidCapHoveredColor(Color color)
        {
            _midCapLookAndFeel.HoveredColor = color;
        }

        public void SetMidCapFillMode(GizmoFillMode3D fillMode)
        {
            _midCapLookAndFeel.FillMode = fillMode;
        }

        public void SetMidCapType(GizmoCap3DType capType)
        {
            if (IsMidCapTypeAllowed(capType))
                _midCapLookAndFeel.CapType = capType;
        }

        public void SetMidCapVisible(bool visible)
        {
            _isMidCapVisible = visible;
        }

        public List<Enum> GetAllowedMidCapTypes()
        {
            return new List<Enum>() { GizmoCap3DType.Box, GizmoCap3DType.Sphere };
        }

        public bool IsMidCapTypeAllowed(GizmoCap3DType capType)
        {
            return capType == GizmoCap3DType.Box || capType == GizmoCap3DType.Sphere;
        }

        public List<Enum> GetAllowedTickTypes()
        {
            return new List<Enum>() { GizmoCap2DType.Circle, GizmoCap2DType.Quad };
        }

        public bool IsTickTypeAllowed(GizmoCap2DType tickType)
        {
            return tickType == GizmoCap2DType.Circle || tickType == GizmoCap2DType.Quad;
        }

        public void SetBoxWireColor(Color color)
        {
            _wireColor = color;
        }

        public void SetTickCullAlphaScale(float alphaScale)
        {
            _tickCullAlphaScale = Mathf.Clamp(alphaScale, 0.0f, 1.0f);
        }

        public void SetTickColor(int axisIndex, Color color)
        {
            GetTickLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
            GetTickLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
        }

        public void SetAllTicksColor(Color color)
        {
            GetTickLookAndFeel(0, AxisSign.Positive).Color = color;
            GetTickLookAndFeel(0, AxisSign.Negative).Color = color;

            GetTickLookAndFeel(1, AxisSign.Positive).Color = color;
            GetTickLookAndFeel(1, AxisSign.Negative).Color = color;

            GetTickLookAndFeel(2, AxisSign.Positive).Color = color;
            GetTickLookAndFeel(2, AxisSign.Negative).Color = color;
        }

        public void SetTickBorderColor(Color color)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.BorderColor = color;
        }

        public void SetTickHoveredColor(Color color)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.HoveredColor = color;
        }

        public void SetTickHoveredBorderColor(Color color)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.HoveredBorderColor = color;
        }

        public void SetTickType(GizmoCap2DType tickType)
        {
            if (IsTickTypeAllowed(tickType))
            {
                foreach (var lookAndFeel in _tickLookAndFeel)
                    lookAndFeel.CapType = tickType;
            }
        }

        public void SetTickQuadWidth(float width)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.QuadWidth = width;
        }

        public void SetTickQuadHeight(float height)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.QuadHeight = height;
        }

        public void SetTickCircleRadius(float radius)
        {
            foreach (var lookAndFeel in _tickLookAndFeel)
                lookAndFeel.CircleRadius = radius;
        }

        public void ConnectTickLookAndFeel(GizmoCap2D tick, int axisIndex, AxisSign axisSign)
        {
            tick.SharedLookAndFeel = GetTickLookAndFeel(axisIndex, axisSign);
        }

        public void ConnectMidCapLookAndFeel(GizmoCap3D midCap)
        {
            midCap.SharedLookAndFeel = _midCapLookAndFeel;
        }

        private GizmoCap2DLookAndFeel GetTickLookAndFeel(int axisIndex, AxisSign axisSign)
        {
            if (axisSign == AxisSign.Positive) return _tickLookAndFeel[axisIndex];
            else return _tickLookAndFeel[axisIndex + 3];
        }
    }
}