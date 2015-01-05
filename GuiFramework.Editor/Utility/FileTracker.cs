using Anomalous.OSPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    /// <summary>
    /// This class keeps track of the current filenames for a series of
    /// open, save, and save as operations.
    /// </summary>
    public class FileTracker
    {
        public delegate void FileChosen(String file);

        private String currentFile = null;

        /// <summary>
        /// Constructor, takes a filter for the save and open dialogs.
        /// </summary>
        /// <param name="filter"></param>
        public FileTracker()
        {
            DefaultDirectory = "";
            Filter = "";
            ParentWindow = null;
        }

        /// <summary>
        /// Show the open file dialog and return the chosen file.  
        /// <param name="fileChosenCb">A function that is called when a file is chosen with the name of the chosen file.</param>
        /// </summary>
        public void openFile(FileChosen fileChosenCb)
        {
            FileOpenDialog fileOpen = new FileOpenDialog(ParentWindow, "", DefaultDirectory, "", Filter, false);
            fileOpen.showModal((result, files) =>
            {
                String file = files.FirstOrDefault();
                if (result == NativeDialogResult.OK && !String.IsNullOrEmpty(file))
                {
                    currentFile = file;
                    fileChosenCb(currentFile);
                }
            });
        }

        /// <summary>
        /// Gets the name of the current file for a save operation.  If no file is
        /// defined the save dialog box will be shown and that result will be returned.
        /// </summary>
        /// <param name="fileChosenCb">A function that is called when a file is chosen with the name of the chosen file.</param>
        public void saveFile(FileChosen fileChosenCb)
        {
            if (currentFile == null)
            {
                saveFileAs(fileChosenCb);
            }
            else
            {
                fileChosenCb(currentFile);
            }
        }

        /// <summary>
        /// This will show the save file dialog and will return the newly chosen file.
        /// </summary>
        /// <param name="parent">The parent window.</param>
        /// <returns>The current file or null if the user canceled the dialog.</returns>
        public void saveFileAs(FileChosen fileChosenCb)
        {
            FileSaveDialog save = new FileSaveDialog(ParentWindow, "", DefaultDirectory, "", Filter);
            save.showModal((result, file) =>
            {
                if (result == NativeDialogResult.OK && !String.IsNullOrEmpty(file))
                {
                    currentFile = file;
                    fileChosenCb(currentFile);
                }
            });
        }

        /// <summary>
        /// Call this function to clear the current file.  This will force the save dialog
        /// to open when save() is called.  Useful if a new thing to be saved was made.
        /// </summary>
        public void clearCurrentFile()
        {
            currentFile = null;
        }

        /// <summary>
        /// Get the currently selected filename.
        /// </summary>
        /// <returns>The current file.</returns>
        public String getCurrentFile()
        {
            return currentFile;
        }

        /// <summary>
        /// Force the current file to be a specific file.
        /// </summary>
        /// <param name="filename"></param>
        public void setCurrentFile(String filename)
        {
            currentFile = filename;
        }

        /// <summary>
        /// The file this File Tracker currently points to.
        /// </summary>
        public String CurrentFile
        {
            get
            {
                return currentFile;
            }
            set
            {
                currentFile = value;
            }
        }

        /// <summary>
        /// The initial directory to open.
        /// </summary>
        public String DefaultDirectory { get; set; }

        /// <summary>
        /// The window to use as a parent.
        /// </summary>
        public NativeOSWindow ParentWindow { get; set; }

        /// <summary>
        /// The file filter.
        /// </summary>
        public String Filter { get; set; }
    }
}
