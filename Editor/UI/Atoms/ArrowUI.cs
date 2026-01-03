using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Atoms
{
    public class ArrowUI : VisualElement
    {
        private readonly VisualElement _from;
        private readonly VisualElement _to;
        private readonly bool _isActive;

        public ArrowUI(VisualElement from, VisualElement to, bool isActive = true)
        {
            AddToClassList("arrow");
            _from = from;
            _to = to;
            _isActive = isActive;
            generateVisualContent += OnGenerateVisualContent;
            
            if (_isActive)
            {
                AddToClassList("arrow--active");
            }
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var painter2D = mgc.painter2D;

            painter2D.lineWidth = 3.0f;
            painter2D.strokeColor = mgc.visualElement.resolvedStyle.color;
            painter2D.dashPattern = _isActive ? new float[] { } : new float[] { 5, 5 };
            painter2D.lineJoin = LineJoin.Round;
            painter2D.lineCap = LineCap.Round;

            var fromLocalCenter = new Vector2(_from.layout.width / 2, _from.layout.height / 2);
            var toLocalCenter = new Vector2(_to.layout.width / 2, _to.layout.height / 2);
            var fromCenter = this.WorldToLocal(_from.LocalToWorld(fromLocalCenter));
            var toCenter = this.WorldToLocal(_to.LocalToWorld(toLocalCenter));
            var fromBottom = new Vector2(fromCenter.x, fromCenter.y + _from.layout.height / 2);
            var toTop = new Vector2(toCenter.x, toCenter.y - _to.layout.height / 2);

            painter2D.BeginPath();
            painter2D.MoveTo(fromBottom);
            painter2D.LineTo(toTop);
            painter2D.Stroke();

            painter2D.dashPattern = new float[] { };
            var direction = (toTop - fromBottom).normalized;
            var perpendicular = new Vector2(-direction.y, direction.x);

            var arrowHeadLength = 10.0f;
            var arrowHeadWidth = 10.0f;

            var arrowBase = toTop - direction * arrowHeadLength;
            var arrowLeft = arrowBase + perpendicular * arrowHeadWidth;
            var arrowRight = arrowBase - perpendicular * arrowHeadWidth;

            painter2D.BeginPath();
            painter2D.MoveTo(arrowLeft);
            painter2D.LineTo(toTop);
            painter2D.LineTo(arrowRight);
            painter2D.Stroke();
        }
    }
}