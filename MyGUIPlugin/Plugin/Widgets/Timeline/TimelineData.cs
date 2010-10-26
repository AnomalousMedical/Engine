using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public abstract class TimelineData
    {
        public float StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                if (_CurrentButton != null)
                {
                    _CurrentButton.updatePosition();
                }
            }
        }

        public float Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
                if (_CurrentButton != null)
                {
                    _CurrentButton.updateDurationWidth();
                }
            }
        }

        public abstract String Track { get; }

        public abstract float _StartTime { get; set; }

        public abstract float _Duration { get; set; }

        internal TimelineViewButton _CurrentButton { get; set; }
    }
}
