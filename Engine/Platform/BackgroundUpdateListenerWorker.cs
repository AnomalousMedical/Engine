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

        private Thread workerThread;
        private bool running = true;
        private Clock currentClock;
        private ManualResetEventSlim workerThreadWait = new ManualResetEventSlim(false);

        private BackgroundUpdateListener updateListener;

        public BackgroundUpdateListenerWorker(BackgroundUpdateListener updateListener)
        {
            this.updateListener = updateListener;
            workerThread = new Thread(doWork);
            workerThread.IsBackground = true;
            workerThread.Start();
        }

        public void Dispose()
        {
            running = false;
            mainThreadWait.Dispose();
            workerThreadWait.Dispose();
        }

        public void waitFor()
        {
            mainThreadWait.Wait();
            mainThreadWait.Reset();
        }

        public void startBackgroundWork(Clock clock)
        {
            this.currentClock = clock;
            workerThreadWait.Set();
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

        private void doWork()
        {
            while (running)
            {
                workerThreadWait.Wait();
                if (running)
                {
                    workerThreadWait.Reset();
                    try
                    {
                        updateListener.doBackgroundWork(currentClock);
                    }
                    catch (Exception)
                    {

                    }
                    mainThreadWait.Set();
                }
            }
        }
    }
}
