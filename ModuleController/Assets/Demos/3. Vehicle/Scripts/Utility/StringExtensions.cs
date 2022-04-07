using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Demos.Vehicle
{
    public static class StringExtensions
    {
        public static string FormatTime(this float time, float numberSpacing = 32f, float dividerSpacing = 16f)
        {
            var timespan = TimeSpan.FromSeconds(time);
            var tenth = Mathf.Floor(timespan.Milliseconds * 0.1f);

            return $"<mspace={numberSpacing}px>{timespan.Minutes:00}</mspace>" +
                   $"<mspace={dividerSpacing}px>:</mspace>" +
                   $"<mspace={numberSpacing}px>{timespan.Seconds:00}</mspace>" +
                   $"<mspace={dividerSpacing}px>,</mspace>" +
                   $"<mspace={numberSpacing}px>{tenth:00}</mspace>";
        }
        
        public static string BuildTime(this float time, ref StringBuilder builder, float numberSpacing = 32f, float dividerSpacing = 16f)
        {
            var timespan = TimeSpan.FromSeconds(time);
            var tenth = Mathf.Floor(timespan.Milliseconds * 0.1f);

            builder.Clear();
            builder.Append($"<mspace={numberSpacing}px>{timespan.Minutes:00}</mspace>");
            builder.Append($"<mspace={dividerSpacing}px>:</mspace>");
            builder.Append($"<mspace={numberSpacing}px>{timespan.Seconds:00}</mspace>");
            builder.Append($"<mspace={dividerSpacing}px>,</mspace>");
            builder.Append($"<mspace={numberSpacing}px>{tenth:00}</mspace>");

            return builder.ToString();
        }
    }
}
