using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class PrimaryStatCurves
    {
        /// <summary>
        /// Get the stat curve for the level being attained. If you are level 1 becoming level 2 you pass 2 to this function.
        /// </summary>
        /// <param name="attainingLevel">The level being attained.</param>
        /// <param name="rank">The rank of the curve. The lower the better the curve is.</param>
        /// <returns></returns>
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
            new StatCurve(130, 12),
            new StatCurve(120, 13),
            new StatCurve(130, 12),
            new StatCurve(130, 12),
            new StatCurve(120, 10),
            new StatCurve(120, 12),
            new StatCurve(110, 10),
            new StatCurve(120, 11),
            new StatCurve(100, 12),
            new StatCurve(110,  9),
            new StatCurve(100,  9),
            new StatCurve(110, 11),
            new StatCurve(100,  9),
            new StatCurve(110, 10),
            new StatCurve(110,  8),
            new StatCurve(115,  9),
            new StatCurve(114, 10),
            new StatCurve(112, 10),
            new StatCurve(100, 10),
            new StatCurve(100,  9),
            new StatCurve(100, 10),
            new StatCurve(100,  8),
            new StatCurve(100,  9),
            new StatCurve(100,  9),
            new StatCurve( 95,  8),
            new StatCurve( 80,  7),
            new StatCurve( 72,  6),
            new StatCurve( 70,  6),
            new StatCurve( 70,  5),
            new StatCurve( 65,  5)
        });

        public StatCurveLevelRange L12to21 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(160,  9),
            new StatCurve(130, 12),
            new StatCurve(140, 10),
            new StatCurve(120, 13),
            new StatCurve(128,  9),
            new StatCurve(125, 12),
            new StatCurve(130,  8),
            new StatCurve(135, 10),
            new StatCurve(130,  9),
            new StatCurve(120,  8),
            new StatCurve(115,  9),
            new StatCurve(120, 10),
            new StatCurve(122,  9),
            new StatCurve(122,  9),
            new StatCurve(105,  9),
            new StatCurve(127,  9),
            new StatCurve(118,  9),
            new StatCurve(115, 10),
            new StatCurve(108, 10),
            new StatCurve(111,  8),
            new StatCurve(108,  9),
            new StatCurve(110,  7),
            new StatCurve(102,  9),
            new StatCurve(100,  9),
            new StatCurve( 90,  9),
            new StatCurve( 85,  7),
            new StatCurve( 69,  7),
            new StatCurve( 53,  9),
            new StatCurve( 70,  6),
            new StatCurve( 63,  6)
        });

        public StatCurveLevelRange L22to31 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(160,  9),
            new StatCurve(133, 11),
            new StatCurve(140, 11),
            new StatCurve(135, 11),
            new StatCurve(130,  8),
            new StatCurve(117, 14),
            new StatCurve(145,  5),
            new StatCurve(130, 11),
            new StatCurve(125, 10),
            new StatCurve(122,  8),
            new StatCurve(124,  7),
            new StatCurve(115, 12),
            new StatCurve(140,  6),
            new StatCurve(130,  7),
            new StatCurve(104, 11),
            new StatCurve(121, 11),
            new StatCurve(114, 10),
            new StatCurve(111, 10),
            new StatCurve(115,  9),
            new StatCurve(112,  9),
            new StatCurve(114,  8),
            new StatCurve(127,  4),
            new StatCurve(101, 10),
            new StatCurve(107,  8),
            new StatCurve( 88, 12),
            new StatCurve(115,  1),
            new StatCurve( 76,  6),
            new StatCurve( 63,  8),
            new StatCurve( 70,  7),
            new StatCurve( 76,  4)
        });

        public StatCurveLevelRange L32to41 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(120, 21),
            new StatCurve(135, 11),
            new StatCurve(110, 21),
            new StatCurve(120, 15),
            new StatCurve(130,  8),
            new StatCurve(118, 14),
            new StatCurve(110, 17),
            new StatCurve(110, 16),
            new StatCurve(120, 11),
            new StatCurve(123,  8),
            new StatCurve(118,  8),
            new StatCurve(102, 17),
            new StatCurve(135,  8),
            new StatCurve( 98, 16),
            new StatCurve(102, 13),
            new StatCurve(108, 15),
            new StatCurve( 95, 16),
            new StatCurve(103, 13),
            new StatCurve(108, 11),
            new StatCurve( 87, 17),
            new StatCurve(106, 11),
            new StatCurve( 77, 20),
            new StatCurve( 88, 15),
            new StatCurve( 85, 14),
            new StatCurve( 85, 13),
            new StatCurve( 92,  8),
            new StatCurve( 77,  6),
            new StatCurve( 70,  6),
            new StatCurve( 71,  7),
            new StatCurve( 61,  9)
        });

        public StatCurveLevelRange L42to51 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve( 70, 44),
            new StatCurve(120, 17),
            new StatCurve( 90, 32),
            new StatCurve( 85, 33),
            new StatCurve( 77, 30),
            new StatCurve( 93, 23),
            new StatCurve(100, 17),
            new StatCurve( 85, 27),
            new StatCurve( 77, 29),
            new StatCurve( 80, 26),
            new StatCurve(107, 11),
            new StatCurve( 91, 21),
            new StatCurve( 83, 29),
            new StatCurve( 83, 22),
            new StatCurve( 93, 16),
            new StatCurve( 86, 23),
            new StatCurve( 82, 22),
            new StatCurve( 83, 21),
            new StatCurve( 83, 21),
            new StatCurve( 53, 32),
            new StatCurve( 63, 29),
            new StatCurve( 50, 31),
            new StatCurve( 70, 21),
            new StatCurve( 77, 18),
            new StatCurve( 62, 22),
            new StatCurve( 78, 13),
            new StatCurve( 68, 10),
            new StatCurve( 69,  7),
            new StatCurve( 67,  9),
            new StatCurve( 49, 14)
        });

        public StatCurveLevelRange L52to61 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(60, 50),
            new StatCurve(72, 43),
            new StatCurve(70, 43),
            new StatCurve(70, 40),
            new StatCurve(72, 33),
            new StatCurve(52, 49),
            new StatCurve(75, 30),
            new StatCurve(70, 33),
            new StatCurve(67, 34),
            new StatCurve(75, 29),
            new StatCurve(78, 26),
            new StatCurve(37, 49),
            new StatCurve(40, 51),
            new StatCurve(45, 43),
            new StatCurve(87, 18),
            new StatCurve(68, 32),
            new StatCurve(71, 28),
            new StatCurve(48, 39),
            new StatCurve(55, 35),
            new StatCurve(45, 37),
            new StatCurve(45, 39),
            new StatCurve(41, 36),
            new StatCurve(57, 28),
            new StatCurve(60, 25),
            new StatCurve(52, 29),
            new StatCurve(64, 20),
            new StatCurve(50, 19),
            new StatCurve(58, 13),
            new StatCurve(48, 18),
            new StatCurve(36, 20)
        });

        public StatCurveLevelRange L62to81 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(50, 57),
            new StatCurve(55, 53),
            new StatCurve(48, 56),
            new StatCurve(53, 51),
            new StatCurve(61, 40),
            new StatCurve(44, 55),
            new StatCurve(44, 50),
            new StatCurve(60, 37),
            new StatCurve(43, 49),
            new StatCurve(55, 42),
            new StatCurve(42, 48),
            new StatCurve(40, 48),
            new StatCurve(30, 57),
            new StatCurve(44, 45),
            new StatCurve(51, 40),
            new StatCurve(41, 48),
            new StatCurve(37, 49),
            new StatCurve(39, 45),
            new StatCurve(31, 51),
            new StatCurve(39, 42),
            new StatCurve(33, 47),
            new StatCurve(40, 37),
            new StatCurve(45, 35),
            new StatCurve(30, 44),
            new StatCurve(39, 38),
            new StatCurve(27, 42),
            new StatCurve(22, 36),
            new StatCurve(28, 31),
            new StatCurve(16, 38),
            new StatCurve(28, 24)
        });

        public StatCurveLevelRange L82to99 { get; set; } = new StatCurveLevelRange(new List<StatCurve>()
        {
            new StatCurve(30, 73),
            new StatCurve(21, 80),
            new StatCurve(27, 73),
            new StatCurve(32, 69),
            new StatCurve(35, 61),
            new StatCurve(35, 62),
            new StatCurve(31, 61),
            new StatCurve(35, 58),
            new StatCurve(31, 58),
            new StatCurve(44, 48),
            new StatCurve(36, 53),
            new StatCurve(40, 48),
            new StatCurve(25, 62),
            new StatCurve(33, 54),
            new StatCurve(25, 60),
            new StatCurve(24, 62),
            new StatCurve(30, 55),
            new StatCurve(25, 57),
            new StatCurve(24, 57),
            new StatCurve(34, 47),
            new StatCurve(26, 53),
            new StatCurve(31, 46),
            new StatCurve(24, 53),
            new StatCurve(24, 50),
            new StatCurve(18, 55),
            new StatCurve(21, 46),
            new StatCurve(21, 37),
            new StatCurve(20, 37),
            new StatCurve(16, 38),
            new StatCurve(20, 30)
        });
    }
}
