using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Engine.Platform
{
    class UpdateListenerWithBackgrounding : UpdateListener, IDisposable
    {
        private List<BackgroundUpdateListenerWorker> workers = new List<BackgroundUpdateListenerWorker>();
        private UpdateListener mainThreadUpdate;

        public UpdateListenerWithBackgrounding(UpdateListener mainThreadUpdate)
        {
            this.mainThreadUpdate = mainThreadUpdate;
        }

        public void Dispose()
        {
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.Dispose();
            }
            workers.Clear();
        }

        public void addBackgroundListener(BackgroundUpdateListener backgroundUpdate)
        {
            workers.Add(new BackgroundUpdateListenerWorker(backgroundUpdate));
        }

        public void removeBackgroundListener(BackgroundUpdateListener listener)
        {
            BackgroundUpdateListenerWorker foundWorker = null;
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                if (worker.UpdateListener == listener)
                {
                    foundWorker = worker;
                    break;
                }
            }
            if (foundWorker != null)
            {
                workers.Remove(foundWorker);
                foundWorker.Dispose();
            }
        }

        public void sendUpdate(Clock clock)
        {
            //Start all background work
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.startBackgroundWork(clock);
            }
            //Do foreground work
            if (mainThreadUpdate != null)
            {
                mainThreadUpdate.sendUpdate(clock);
            }
            //Wait for all background tasks to be done
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.waitFor();
            }
            //Synchronize all background results
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.synchronizeResults();
            }
        }

        public void loopStarting()
        {
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.loopStarting();
            }
            if (mainThreadUpdate != null)
            {
                mainThreadUpdate.loopStarting();
            }
        }

        public void exceededMaxDelta()
        {
            foreach (BackgroundUpdateListenerWorker worker in workers)
            {
                worker.exceededMaxDelta();
            }
            if (mainThreadUpdate != null)
            {
                mainThreadUpdate.exceededMaxDelta();
            }
        }

        public bool Empty
        {
            get
            {
                return mainThreadUpdate == null && workers.Count == 0;
            }
        }
    }
}
