using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class HpStatCurves
    {
        public StatCurve GetStatCurve(long attainingLevel, int rank)
        {
            switch (attainingLevel)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    return L2to11[rank];
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                    return L12to21[rank];
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                    return L22to31[rank];
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                    return L32to41[rank];
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                    return L42to51[rank];
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                    return L52to61[rank];
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                    return L62to81[rank];
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                    return L82to99[rank];
                default:
                    throw new InvalidOperationException($"Invalid level {attainingLevel}");
            }
        }

        public StatCurveLevelRange L2to11 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(19,    200),
            new StatCurve(22,    200),
            new StatCurve(19,    200),
            new StatCurve(17,    160),
            new StatCurve(21,    200),
            new StatCurve(18,    200),
            new StatCurve(24,    200),
            new StatCurve(18,    160),
            new StatCurve(23,    200)
        });

        public StatCurveLevelRange L12to21 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(42,    -40),
            new StatCurve(45,      0),
            new StatCurve(38,      0),
            new StatCurve(36,      0),
            new StatCurve(45,    -40),
            new StatCurve(37,      0),
            new StatCurve(51,    -80),
            new StatCurve(41,    -80),
            new StatCurve(44,    -40)
        });

        public StatCurveLevelRange L22to31 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(72,   -640),
            new StatCurve(82,   -760),
            new StatCurve(64,   -520),
            new StatCurve(65,   -560),
            new StatCurve(75,   -640),
            new StatCurve(64,   -560),
            new StatCurve(80,   -640),
            new StatCurve(67,   -600),
            new StatCurve(73,   -640)
        });

        public StatCurveLevelRange L32to41 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(100,  -1440),
            new StatCurve(118,  -1840),
            new StatCurve( 96,  -1520),
            new StatCurve( 93,  -1400),
            new StatCurve(105,  -1520),
            new StatCurve( 89,  -1320),
            new StatCurve(111,  -1560),
            new StatCurve( 86,  -1160),
            new StatCurve(107,  -1640)
        });

        public StatCurveLevelRange L42to51 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(121,  -2280),
            new StatCurve(143,  -2840),
            new StatCurve(121,  -2520),
            new StatCurve(114,  -2240),
            new StatCurve(126,  -2360),
            new StatCurve(111,  -2160),
            new StatCurve(141,  -2760),
            new StatCurve(110,  -2120),
            new StatCurve(125,  -2360)
        });

        public StatCurveLevelRange L52to61 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(137,  -3080),
            new StatCurve(143,  -2840),
            new StatCurve(131,  -3000),
            new StatCurve(126,  -2880),
            new StatCurve(134,  -2760),
            new StatCurve(127,  -2960),
            new StatCurve(138,  -2600),
            new StatCurve(123,  -2800),
            new StatCurve(129,  -2560)
        });

        public StatCurveLevelRange L62to81 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(120,  -2040),
            new StatCurve(115,  -1160),
            new StatCurve(117,  -2160),
            new StatCurve(113,  -2080),
            new StatCurve(119,  -1840),
            new StatCurve(120,  -2560),
            new StatCurve( 99,   -240),
            new StatCurve(120,  -2640),
            new StatCurve(115,  -1720)
        });

        public StatCurveLevelRange L82to99 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(98,   -200),
            new StatCurve(95,    600),
            new StatCurve(92,    -80),
            new StatCurve(93,   -400),
            new StatCurve(97,    -80),
            new StatCurve(96,   -520),
            new StatCurve(72,   2000),
            new StatCurve(92,   -400),
            new StatCurve(93,      0)
        });
    }
}