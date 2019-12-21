using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.SidescrollerCore
{
    public enum PlayerId
    {
        Player1,
        Player2,
        Player3,
        Player4,
    }

    public static class PlayerType
    {
        /// <summary>
        /// Get the player class for the given player id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Type GetPlayerType(this PlayerId id)
        {
            switch (id)
            {
                case PlayerId.Player1:
                    return typeof(Player1);
                case PlayerId.Player2:
                    return typeof(Player2);
                case PlayerId.Player3:
                    return typeof(Player3);
                case PlayerId.Player4:
                    return typeof(Player4);
            }

            throw new InvalidOperationException($"Cannot convert player type '{id}'");
        }

        /// <summary>
        /// Get a generic type of T that is keyed to the given player id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Type GetPlayerKeyedType(this PlayerId id, Type t)
        {
            var playerType = PlayerType.GetPlayerType(id);
            return t.MakeGenericType(new Type[] { playerType });
        }
    }

    public class Player1
    {
        
    }

    public class Player2
    {

    }

    public class Player3
    {

    }

    public class Player4
    {

    }
}
