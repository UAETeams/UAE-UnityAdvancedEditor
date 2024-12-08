using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class TerrainGizmoLookAndFeel
    {
        [SerializeField]
        private Color _radiusCircleColor = Color.white;
        [SerializeField]
        GizmoLineSlider3DLookAndFeel _axisSliderLookAndFeel = new GizmoLineSlider3DLookAndFeel();
        [SerializeField]
        GizmoCap3DLookAndFeel _midCapLookAndFeel = new GizmoCap3DLookAndFeel();
        [SerializeField]
        GizmoCap2DLookAndFeel _radiusTickLookAndFeel = new GizmoCap2DLookAndFeel();

        public Color AxisSliderColor { get { return _axisSliderLookAndFeel.Color; } set { _axisSliderLookAndFeel.Color = value; } }
        public Color AxisSliderHoveredColor { get { return _axisSliderLookAndFeel.HoveredColor; } set { _axisSliderLookAndFeel.HoveredColor = value; } }
        public Color AxisSliderCapColor { get { return _axisSliderLookAndFeel.CapLookAndFeel.Color; } set { _axisSliderLookAndFeel.CapLookAndFeel.Color = value; } }
        public Color AxisSliderCapHoveredColor { get { return _axisSliderLookAndFeel.CapLookAndFeel.HoveredColor; } set { _axisSliderLookAndFeel.CapLookAndFeel.HoveredColor = value; } }
        public GizmoCap3DType AxisSliderCapType { get { return _axisSliderLookAndFeel.CapLookAndFeel.CapType; } set { _axisSliderLookAndFeel.CapLookAndFeel.CapType = value; } }
        public GizmoLine3DType AxisSliderLineType { get { return _axisSliderLookAndFeel.LineType; } set { _axisSliderLookAndFeel.LineType = value; } }
        public float AxisSliderLength { get { return _axisSliderLookAndFeel.Length; } set { _axisSliderLookAndFeel.Length = value; } }
        public GizmoCap3DType MidCapType { get { return _midCapLookAndFeel.CapType; } set { if (value == GizmoCap3DType.Box || value == GizmoCap3DType.Sphere) _midCapLookAndFeel.CapType = value; } }
        public float MidCapBoxWidth { get { return _midCapLookAndFeel.BoxWidth; } set { _midCapLookAndFeel.BoxWidth = value; } }
        public float MidCapBoxHeight { get { return _midCapLookAndFeel.BoxHeight; } set { _midCapLookAndFeel.BoxHeight = value; } }
        public float MidCapBoxDepth { get { return _midCapLookAndFeel.BoxDepth; } set { _midCapLookAndFeel.BoxDepth = value; } }
        public float MidSphereRadius { get { return _midCapLookAndFeel.SphereRadius; } set { _midCapLookAndFeel.SphereRadius = value; } }
        public Color MidCapColor { get { return _midCapLookAndFeel.Color; } set { _midCapLookAndFeel.Color = value; } }
        public Color MidCapHoveredColor { get { return _midCapLookAndFeel.HoveredColor; } set { _midCapLookAndFeel.HoveredColor = value; } }
        public Color RadiusCircleColor { get { return _radiusCircleColor; } set { _radiusCircleColor = value; } }
        public GizmoCap2DType RadiusTickType { get { return _radiusTickLookAndFeel.CapType; } set { if (value == GizmoCap2DType.Circle || value == GizmoCap2DType.Quad) _radiusTickLookAndFeel.CapType = value; } }
        public Color RadiusTickColor { get { return _radiusTickLookAndFeel.Color; } set { _radiusTickLookAndFeel.Color = value; } }
        public Color RadiusTickHoveredColor { get { return _radiusTickLookAndFeel.HoveredColor; } set { _radiusTickLookAndFeel.HoveredColor = value; } }
        public float RadiusTickQuadWidth { get { return _radiusTickLookAndFeel.QuadWidth; } set { _radiusTickLookAndFeel.QuadWidth = value; } }
        public float RadiusTickQuadHeight { get { return _radiusTickLookAndFeel.QuadHeight; } set { _radiusTickLookAndFeel.QuadHeight = value; } }
        public float RadiusTickCircleRadius { get { return _radiusTickLookAndFeel.CircleRadius; }set { _radiusTickLookAndFeel.CircleRadius = value; } }

        public TerrainGizmoLookAndFeel()
        {
            AxisSliderColor = Color.red;
            AxisSliderCapColor = Color.red;
            AxisSliderCapType = GizmoCap3DType.Cone;
            AxisSliderLineType = GizmoLine3DType.Thin;
            AxisSliderLength = 5.0f;
            MidCapType = GizmoCap3DType.Box;
            MidCapBoxWidth = 0.7f;
            MidCapBoxHeight = 0.7f;
            MidCapBoxDepth = 0.7f;
            MidSphereRadius = 0.35f;
            MidCapColor = Color.green;
            RadiusTickType = GizmoCap2DType.Quad;
            RadiusTickColor = Color.green;
            RadiusTickQuadWidth = 8.0f;
            RadiusTickQuadHeight = 8.0f;
            RadiusTickCircleRadius = 4.0f;

            _radiusTickLookAndFeel.BorderColor = Color.white.KeepAllButAlpha(0.0f);
        }

        public void ConnectAxisSliderLookAndFeel(GizmoLineSlider3D axisSlider)
        {
            axisSlider.SharedLookAndFeel = _axisSliderLookAndFeel;
        }

        public void ConnectMidCapLookAndFeel(GizmoCap3D pickPointCap)
        {
            pickPointCap.SharedLookAndFeel = _midCapLookAndFeel;
        }

        public void ConnectRadiusTickLookAndFeel(GizmoCap2D radiusTick)
        {
            radiusTick.SharedLookAndFeel = _radiusTickLookAndFeel;
        }
    }
}