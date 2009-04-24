using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine
{
    /// <summary>
    /// This is the controller class for Coroutines. To execute a new coroutine
    /// call the Start function.
    /// </summary>
    /// <remarks>
    /// Coroutines can be executed using this class. In order to create a new
    /// coroutine a funciton must be created that has a return type of
    /// IEnumerator&lt;YieldAction&gt;. This function can then be executed
    /// inside of a call to Coroutine.Start. Inside of a coroutine it is valid
    /// to call yield return Coroutine.Wait to make the coroutine wait. There
    /// may also be plugin specific wait extensions. It is not valid to call
    /// these functions without proceeding it with a yield return.
    /// </remarks>
    public class Coroutine
    {
        private static List<IEnumerator<YieldAction>> queued = new List<IEnumerator<YieldAction>>();
        private static List<WaitAction> waitActions = new List<WaitAction>();
        private static CoroutineUpdater updater = new CoroutineUpdater();

        /// <summary>
        /// Hide constructor.
        /// </summary>
        private Coroutine()
        {

        }

        /// <summary>
        /// Start executing a new coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to start executing.</param>
        public static void Start(IEnumerator<YieldAction> coroutine)
        {
            queued.Add(coroutine);
        }

        /// <summary>
        /// Make a coroutine wait for the given number of seconds before
        /// continuing execution. This function is only valid to be called as
        /// part of a yield return statement, or else it will cause an exception
        /// to be thrown when the timer expires. To call from within a coroutine
        /// write "yield return Coroutine.Wait(time);" This is the only valid
        /// way to call this function.
        /// </summary>
        /// <param name="seconds">The number of seconds to wait.</param>
        /// <returns>A YieldAction that will wait for the specified number of seconds before continuing execution of the coroutine.</returns>
        public static YieldAction Wait(double seconds)
        {
            WaitAction wait = new WaitAction(seconds);
            waitActions.Add(wait);
            return wait;
        }

        /// <summary>
        /// Set the timer that the Coroutines will listen to for updates at full speed.
        /// </summary>
        /// <param name="timer">The timer to listen to for updates.</param>
        public static void SetTimerFull(UpdateTimer timer)
        {
            timer.addFullSpeedUpdateListener(updater);
        }

        /// <summary>
        /// Set the timer that the Coroutines will listen to for updates at fixed speed.
        /// </summary>
        /// <param name="timer">The timer to listen to for updates.</param>
        public static void SetTimerFixed(UpdateTimer timer)
        {
            timer.addFixedUpdateListener(updater);
        }

        /// <summary>
        /// This is an internal function to continue executing a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to continue executing.</param>
        internal static void Continue(IEnumerator<YieldAction> coroutine)
        {
            queued.Add(coroutine);
        }

        /// <summary>
        /// This is an internal function to update all coroutines. It should be
        /// called once per frame. It will cause all queued coroutines to
        /// execute and then clear the queue. It will also update all the
        /// WaitAction timers and execute any coroutines that have waited their
        /// alloted time.
        /// </summary>
        /// <param name="time">The amount of time since the last update in seconds.</param>
        internal static void Update(double time)
        {
            for (int i = 0; i < waitActions.Count;)
            {
                if (waitActions[i].tick(time))
                {
                    waitActions.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            foreach (IEnumerator<YieldAction> coroutine in queued)
            {
                if (coroutine.MoveNext())
                {
                    coroutine.Current.setCoroutine(coroutine);
                }
            }
            queued.Clear();
        }
    }
}
