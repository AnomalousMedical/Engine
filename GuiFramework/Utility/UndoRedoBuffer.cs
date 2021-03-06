﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    /// <summary>
    /// Provides all the management for a standard undo / redo buffer that runs off commands.
    /// That is this will push commands to the location of the cursor, if there are commands
    /// after that one they are trimmed off. You can also limit the size of the buffer.
    /// </summary>
    public class UndoRedoBuffer
    {
        private TwoWayCommandBuffer buffer = new TwoWayCommandBuffer();
        private int currentItemCount = 0;
        private int maxItemCount;

        public UndoRedoBuffer(int maxItemCount = 1000)
        {
            this.maxItemCount = maxItemCount;
        }

        /// <summary>
        /// Push a command and run its execute (redo) funciton.
        /// </summary>
        public void pushAndExecute(TwoWayCommand command)
        {
            push(command);
            execute();
        }

        /// <summary>
        /// Push a command and skip its execute (redo) function to only run when manually invoked.
        /// </summary>
        public void pushAndSkip(TwoWayCommand command)
        {
            push(command);
            buffer.skipExecute();
        }

        /// <summary>
        /// Executes the current command in the buffer and increments to the next command or does nothing if you are already at the last command.
        /// </summary>
        public void execute()
        {
            buffer.execute();
        }

        /// <summary>
        /// Undoes the current command in the buffer and decrements to the previous command or does nothing if you are already at the first command.
        /// </summary>
        public void undo()
        {
            buffer.undo();
        }

        private void push(TwoWayCommand command)
        {
            if (!buffer.OnLast)
            {
                //If we are not on the last item, trim and recount
                buffer.trim();
                currentItemCount = buffer.Count;
            }

            if (currentItemCount == maxItemCount)
            {
                //If we are full, pop the first item
                buffer.popFirst();
            }
            else
            {
                ++currentItemCount;
            }

            buffer.push(command);
        }

        public void clear()
        {
            buffer.clear();
        }

        /// <summary>
        /// True if this buffer can undo.
        /// </summary>
        public bool HasUndo
        {
            get
            {
                return !buffer.OnFirst;
            }
        }

        /// <summary>
        /// True if this buffer can redo.
        /// </summary>
        public bool HasRedo
        {
            get
            {
                return !buffer.OnLast;
            }
        }
    }
}
