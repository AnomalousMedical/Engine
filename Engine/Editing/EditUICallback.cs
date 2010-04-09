using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public delegate bool ValidateUIInput(String input, out String newPrompt);

    /// <summary>
    /// This interface allows an EditInterface command to request information
    /// from the user in a general way. The method employed to get the data is
    /// up to the implementing UI.
    /// </summary>
    public interface EditUICallback
    {
        /// <summary>
        /// Call back to the UI to get an input string for a given prompt. This
        /// function will return true if the user entered valid input or false
        /// if they canceled or did not enter valid input. If false is returned
        /// the operation in progress should be stopped and any changes
        /// reverted.
        /// </summary>
        /// <param name="prompt">The propmpt to show the user.</param>
        /// <param name="result">The result of the user input.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        bool getInputString(String prompt, out String result, ValidateUIInput validate);

        /// <summary>
        /// Call back to the UI to get a result from a displayed browser. This
        /// function will return true if the user entered valid input or false
        /// if they canceled or did not enter valid input. If false is returned
        /// the operation in progress should be stopped and any changes
        /// reverted.
        /// </summary>
        /// <param name="browser">The Browser to show to the user.</param>
        /// <param name="result">A reference to an object to store the result in.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        bool showBrowser(Browser browser, out Object result);

        /// <summary>
        /// Call back to the UI to open a open file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        bool showOpenFileDialog(String filterString, out String filename);

        /// <summary>
        /// Call back to the UI to open a save file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        bool showSaveFileDialog(String filterString, out String filename);

        /// <summary>
        /// Call back to the UI to open a folder browser dialog.
        /// </summary>
        /// <param name="folderName">The folder chosen by the folder browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        bool showFolderBrowserDialog(out String folderName);

        /// <summary>
        /// Get the EditInterface that is currently selected on the UI.
        /// </summary>
        /// <returns>The EditInterface that is currently selected.</returns>
        EditInterface getSelectedEditInterface();
    }
}
