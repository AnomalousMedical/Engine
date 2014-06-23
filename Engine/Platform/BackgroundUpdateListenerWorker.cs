using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Platform
{
    class BackgroundUpdateListenerWorker : IDisposable
    {
        private ManualResetEventSlim mainThreadWait = new ManualResetEventSlim(false);

        private BackgroundUpdateListener updateListener;

        public BackgroundUpdateListenerWorker(BackgroundUpdateListener updateListener)
        {
            this.updateListener = updateListener;
        }

        public void Dispose()
        {
            mainThreadWait.Dispose();
        }

        public void waitFor()
        {
            mainThreadWait.Wait();
            mainThreadWait.Reset();
        }

        public void startBackgroundWork(Clock clock)
        {
            Task.Run(() =>
            {
                updateListener.doBackgroundWork(clock);
                mainThreadWait.Set();
            });
        }

        public void synchronizeResults()
        {
            updateListener.synchronizeResults();
        }

        public void loopStarting()
        {
            updateListener.loopStarting();
        }

        public void exceededMaxDelta()
        {
            updateListener.exceededMaxDelta();
        }

        public BackgroundUpdateListener UpdateListener
        {
            get
            {
                return updateListener;
            }
        }

        //private void doBackgroundWork()
        //{
        //    Logging.Log.Debug("Started Running background update worker thread");
        //    try
        //    {
        //        while (runBackgroundThread)
        //        {
        //            backgroundThreadWait.Wait(token);
        //            updateListener.doBackgroundWork(currentClock);
        //            mainThreadWait.Set();
        //        }
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Logging.Log.Debug("Wait operation canceled");
        //    }
        //    Logging.Log.Debug("Stopped Running background update worker thread");
        //}
    }
}
