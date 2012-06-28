using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public delegate bool SendResult<ResultType>(ResultType result, ref String errorPrompt);
    public delegate bool SendResult<ResultType, InputType>(ResultType result, InputType input, ref String errorPrompt);

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
        void getInputString(String prompt, SendResult<String> resultCallback);

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
        void showBrowser<T>(Browser browser, SendResult<T> resultCallback);

        /// <summary>
        /// Call back to the UI to get a result from a displayed browser. This
        /// function will return true if the user entered valid input or false
        /// if they canceled or did not enter valid input. If false is returned
        /// the operation in progress should be stopped and any changes
        /// reverted.
        /// 
        /// This will also place an input box on the browser that must be filled out by the user.
        /// </summary>
        /// <param name="browser">The Browser to show to the user.</param>
        /// <param name="result">A reference to an object to store the result in.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        void showInputBrowser<T>(Browser browser, SendResult<T, String> resultCallback);

        /// <summary>
        /// Call back to the UI to open a open file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        void showOpenFileDialog(String filterString, SendResult<String> resultCallback);

        /// <summary>
        /// Call back to the UI to open a save file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        void showSaveFileDialog(String filterString, SendResult<String> resultCallback);

        /// <summary>
        /// Call back to the UI to open a folder browser dialog.
        /// </summary>
        /// <param name="folderName">The folder chosen by the folder browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        void showFolderBrowserDialog(SendResult<String> resultCallback);

        /// <summary>
        /// Get the EditInterface that is currently selected on the UI.
        /// </summary>
        /// <returns>The EditInterface that is currently selected.</returns>
        EditInterface getSelectedEditInterface();

        /// <summary>
        /// This method allows the interface to run a custom query on the
        /// UICallback. This can do anything and is not defined here.
        /// </summary>
        /// <param name="queryKey">The key for the query to run.</param>
        /// <param name="resultCallback">The callback with the results.</param>
        void runCustomQuery<Ret>(Object queryKey, SendResult<Ret> resultCallback);

        void runCustomQuery<Ret, Arg1>(Object queryKey, SendResult<Ret> resultCallback, Arg1 arg1);

        void runCustomQuery<Ret, Arg1, Arg2>(Object queryKey, SendResult<Ret> resultCallback, Arg1 arg1, Arg2 arg2);

        void runCustomQuery<Ret, Arg1, Arg2, Arg3>(Object queryKey, SendResult<Ret> resultCallback, Arg1 arg1, Arg2 arg2, Arg3 arg3);

        /// <summary>
        /// This method allows the interface to run a custom query on the
        /// UICallback. This can do anything and is not defined here. This is
        /// for a callback that expects no result.
        /// </summary>
        /// <param name="queryKey">The key for the query to run.</param>
        void runOneWayCustomQuery(Object queryKey);

        void runOneWayCustomQuery<Arg1>(Object queryKey, Arg1 arg1);

        void runOneWayCustomQuery<Arg1, Arg2>(Object queryKey, Arg1 arg1, Arg2 arg2);

        void runOneWayCustomQuery<Arg1, Arg2, Arg3>(Object queryKey, Arg1 arg1, Arg2 arg2, Arg3 arg3);

        /// <summary>
        /// This method allows the interface to run a custom query on the
        /// UICallback. This can do anything and is not defined here.
        /// </summary>
        /// <param name="queryKey">The key for the query to run.</param>
        /// <param name="resultCallback">The callback with the results.</param>
        Ret runSyncCustomQuery<Ret>(Object queryKey);

        Ret runSyncCustomQuery<Ret, Arg1>(Object queryKey, Arg1 arg1);

        Ret runSyncCustomQuery<Ret, Arg1, Arg2>(Object queryKey, Arg1 arg1, Arg2 arg2);

        Ret runSyncCustomQuery<Ret, Arg1, Arg2, Arg3>(Object queryKey, Arg1 arg1, Arg2 arg2, Arg3 arg3);
    }
}
