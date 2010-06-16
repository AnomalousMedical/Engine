using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public enum Align
    {
        HCenter = 0, /**< center horizontally */
        VCenter = 0, /**< center vertically */
        Center = HCenter | VCenter, /**< center in the dead center */

        Left = 1<<(1), /**< value from the left (and center vertically) */
        Right = 1<<(2), /**< value from the right (and center vertically) */
        HStretch = Left | Right, /**< stretch horizontally proportionate to parent window (and center vertically) */

        Top = 1<<(3), /**< value from the top (and center horizontally) */
        Bottom = 1<<(4), /**< value from the bottom (and center horizontally) */
        VStretch = Top | Bottom, /**< stretch vertically proportionate to parent window (and center horizontally) */

        Stretch = HStretch | VStretch, /**< stretch proportionate to parent window */
        Default = Left | Top, /**< default value (value from left and top) */

        HRelative = 1<<(5),
        VRelative = 1<<(6),
        Relative = HRelative | VRelative
    };
}
