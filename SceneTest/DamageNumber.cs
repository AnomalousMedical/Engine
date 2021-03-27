using Engine;
using SharpGui;

namespace SceneTest
{
    class DamageNumber
    {
        public DamageNumber(string number, long timeRemaining, Vector2 position, IScaleHelper scaleHelper)
        {
            this.StartPosition = position;
            this.EndPosition = position + new Vector2(0, scaleHelper.Scaled(-15));
            TimeRemaining = timeRemaining;
            HalfDuration = timeRemaining / 2;
            this.Text = new SharpText(number.ToString()) { Rect = new IntRect(0, 0, 10000, 10000) };
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            Vector2 position;
            if (TimeRemaining > HalfDuration)
            {
                position = StartPosition.lerp(EndPosition, 1f - (float)TimeRemaining / HalfDuration);
            }
            else
            {
                position = EndPosition.lerp(StartPosition, 1f - (float)(TimeRemaining - HalfDuration) / HalfDuration);
            }
            this.Text.Rect.Left = (int)position.x;
            this.Text.Rect.Top = (int)position.y;
        }

        public long HalfDuration { get; }

        public long TimeRemaining { get; set; }

        public Vector2 EndPosition { get; }

        public Vector2 StartPosition { get; }

        public SharpText Text { get; }
    }
}
