using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class XpTable
    {
        public long GetStart(long attainingLevel, long rank)
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
                    switch (rank)
                    {
                        case 68:
                        case 69:
                            return 6;
                        case 70:
                            return 7;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 70:
                            return 3542;
                        case 71:
                            return 3588;
                        case 72:
                            return 3639;
                        case 73:
                            return 3689;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 72:
                            return 23831;
                        case 73:
                            return 24161;
                        case 74:
                            return 24493;
                        case 75:
                            return 24827;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 74:
                            return 77066;
                        case 75:
                            return 78112;
                        case 76:
                            return 79149;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 74:
                            return 176259;
                        case 75:
                            return 178647;
                        case 76:
                            return 181023;
                        case 77:
                            return 183403;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 74:
                            return 336872;
                        case 75:
                            return 341432;
                        case 76:
                            return 345977;
                        case 77:
                            return 350527;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 75:
                            return 581467;
                        case 76:
                            return 589211;
                        case 77:
                            return 596961;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 75:
                            return 913752;
                        case 76:
                            return 925925;
                        case 77:
                            return 938105;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
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
                    switch (rank)
                    {
                        case 76:
                            return 1371319;
                        case 77:
                            return 1389359;
                        case 78:
                            return 1407407;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                    switch (rank)
                    {
                        case 76:
                            return 1940593;
                        case 77:
                            return 1966123;
                        case 78:
                            return 1991662;
                        default:
                            throw new InvalidOperationException("Should not happen");
                    }
                default:
                    throw new InvalidOperationException($"Invalid level {attainingLevel}");
            }
        }

        public long GetRank(Archetype archetype, long attainingLevel)
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
                    return archetype.Xp02to11;
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
                    return archetype.Xp12to21;
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
                    return archetype.Xp22to31;
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
                    return archetype.Xp32to41;
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
                    return archetype.Xp42to51;
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
                    return archetype.Xp52to61;
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
                    return archetype.Xp62to81;
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
                    return archetype.Xp82to99;
                default:
                    throw new InvalidOperationException($"Invalid level {attainingLevel}");
            }
        }
    }
}
