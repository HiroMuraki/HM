#nullable enable
using UnityEngine;

namespace HM.UnityEngine
{
    public class ReactiveArea
    {
        public Margin Margin { get; set; }
        public int PixelsPerUnit { get; set; } = 100;
        public float ScreenWidthInUnit => (float)Screen.width / PixelsPerUnit;
        public float ScreenHeightInUnit => (float)Screen.height / PixelsPerUnit;
        public float Width => ScreenWidthInUnit - (Margin.Left + Margin.Right);
        public float Height => ScreenHeightInUnit - (Margin.Top + Margin.Bottom);
        public Vector2 Center => new Vector2(Margin.Left - Margin.Right, Margin.Bottom - Margin.Top) / 2;
        public Vector2 TopLeft => new Vector2(-ScreenWidthInUnit / 2 + Margin.Left, ScreenHeightInUnit / 2 - Margin.Top);
        public Vector2 TopRight => new Vector2(ScreenWidthInUnit / 2 - Margin.Right, ScreenHeightInUnit / 2 - Margin.Top);
        public Vector2 BottomLeft => new Vector2(-ScreenWidthInUnit / 2 + Margin.Left, -ScreenHeightInUnit / 2 + Margin.Bottom);
        public Vector2 BottomRight => new Vector2(ScreenWidthInUnit / 2 - Margin.Right, -ScreenHeightInUnit / 2 + Margin.Bottom);

        public ReactiveArea(Margin margin)
        {
            Margin = margin;
        }
    }
}
