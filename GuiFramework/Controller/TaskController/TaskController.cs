using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public delegate void TaskRemovedDelegate(Task task, bool willReload);

    public class TaskController
    {
        public event TaskDelegate TaskAdded;
        public event TaskRemovedDelegate TaskRemoved;
        public event Action<String, IEnumerable<Task>> HighlightTasks;

        private Dictionary<String, Task> items = new Dictionary<String, Task>();

        public TaskController()
        {

        }

        public void addTask(Task item)
        {
            items.Add(item.UniqueName, item);
            if (TaskAdded != null)
            {
                TaskAdded.Invoke(item);
            }
        }

        public void removeTask(Task item, bool willReload)
        {
            items.Remove(item.UniqueName);
            if (TaskRemoved != null)
            {
                TaskRemoved.Invoke(item, willReload);
            }
        }

        public Task getTask(String uniqueName)
        {
            Task item = null;
            items.TryGetValue(uniqueName, out item);
            return item;
        }

        public IEnumerable<Task> Tasks
        {
            get
            {
                return items.Values;
            }
        }

        public void highlightTasks(String highlightTaskCategory, IEnumerable<Task> highlightTasks)
        {
            if(HighlightTasks != null)
            {
                HighlightTasks.Invoke(highlightTaskCategory, highlightTasks);
            }
        }
    }
}
