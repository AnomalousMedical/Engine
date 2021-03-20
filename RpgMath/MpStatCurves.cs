using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class MpStatCurves
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
            new StatCurve(64,     12),
            new StatCurve(57,     10),
            new StatCurve(60,     10),
            new StatCurve(70,     16),
            new StatCurve(58,     12),
            new StatCurve(58,     10),
            new StatCurve(60,     12),
            new StatCurve(63,     12),
            new StatCurve(54,     10)
        });

        public StatCurveLevelRange L12to21 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(78,      0),
            new StatCurve(67,      0),
            new StatCurve(70,      0),
            new StatCurve(84,      0),
            new StatCurve(75,     -6),
            new StatCurve(72,     -2),
            new StatCurve(75,     -2),
            new StatCurve(80,     -6),
            new StatCurve(75,    -12)
        });

        public StatCurveLevelRange L22to31 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(90,    -26),
            new StatCurve(77,    -20),
            new StatCurve(84,    -28),
            new StatCurve(99,    -30),
            new StatCurve(86,    -28),
            new StatCurve(80,    -20),
            new StatCurve(83,    -20),
            new StatCurve(90,    -26),
            new StatCurve(83,    -26)
        });

        public StatCurveLevelRange L32to41 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(101,    -58),
            new StatCurve( 90,    -60),
            new StatCurve( 94,    -58),
            new StatCurve(112,    -68),
            new StatCurve( 97,    -60),
            new StatCurve( 93,    -58),
            new StatCurve( 97,    -60),
            new StatCurve( 96,    -44),
            new StatCurve( 87,    -38)
        });

        public StatCurveLevelRange L42to51 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(112,   -102),
            new StatCurve(102,   -108),
            new StatCurve(104,    -98),
            new StatCurve(124,   -116),
            new StatCurve(108,   -104),
            new StatCurve(106,   -110),
            new StatCurve(108,   -104),
            new StatCurve(100,    -60),
            new StatCurve( 94,    -66)
        });

        public StatCurveLevelRange L52to61 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(112,   -102),
            new StatCurve(100,    -96),
            new StatCurve(104,    -98),
            new StatCurve(120,    -96),
            new StatCurve(112,   -126),
            new StatCurve(110,   -130),
            new StatCurve(108,   -104),
            new StatCurve(105,    -86),
            new StatCurve(104,   -116)
        });

        public StatCurveLevelRange L62to81 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve( 96,     -4),
            new StatCurve( 84,      0),
            new StatCurve( 92,    -26),
            new StatCurve(105,     -6),
            new StatCurve( 94,    -16),
            new StatCurve( 85,     20),
            new StatCurve( 94,    -20),
            new StatCurve( 97,     38),
            new StatCurve( 89,    -24)
        });

        public StatCurveLevelRange L82to99 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(73,    180),
            new StatCurve(63,    170),
            new StatCurve(72,    136),
            new StatCurve(82,    188),
            new StatCurve(66,    210),
            new StatCurve(72,    126),
            new StatCurve(70,    178),
            new StatCurve(84,     74),
            new StatCurve(69,    140)
        });
    }
}