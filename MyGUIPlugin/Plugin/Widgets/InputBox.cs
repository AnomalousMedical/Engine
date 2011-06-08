using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using Engine.Editing;

namespace MyGUIPlugin
{
    public class InputBox : Dialog
    {
        private Edit input;
        private StaticText prompt;
        private SendResult<String> SendResult;
        private Size2 deltaSize;

        public InputBox(String title, String message, String text)
            :this(title, message, text, "MyGUIPlugin.Plugin.Widgets.InputBox.layout")
        {
            
        }

        public InputBox(String title, String message, String text, String customLayout)
            :base(customLayout)
        {
            input = window.findWidget("Input") as Edit;
            input.Caption = text;
            input.KeyButtonReleased += new MyGUIEvent(input_KeyButtonReleased);

            prompt = window.findWidget("Prompt") as StaticText;
            //Figure out the size difference of the prompt and window
            deltaSize = new Size2(window.Width - prompt.Width, window.Height - prompt.Height);
            Message = message;

            window.Caption = title;

            Button ok = window.findWidget("Ok") as Button;
            ok.MouseButtonClick += new MyGUIEvent(ok_MouseButtonClick);

            Button cancel = window.findWidget("Cancel") as Button;
            cancel.MouseButtonClick += new MyGUIEvent(cancel_MouseButtonClick);

            Accepted = false;
        }

        public String Text
        {
            get
            {
                return input.Caption;
            }
        }

        public String Message
        {
            get
            {
                return prompt.Caption;
            }
            set
            {
                prompt.Caption = value;
                Size2 textSize = prompt.getTextSize();
                textSize += deltaSize;
                window.setSize((int)textSize.Width, (int)textSize.Height);
            }
        }

        public bool Accepted { get; private set; }

        void input_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (ke.Key == KeyboardButtonCode.KC_RETURN)
            {
                ok_MouseButtonClick(source, e);
            }
        }

        void cancel_MouseButtonClick(Widget source, EventArgs e)
        {
            Accepted = false;
            close();
        }

        void ok_MouseButtonClick(Widget source, EventArgs e)
        {
            Accepted = true;
            close();
        }

        private delegate void InputGatheredCallback(String text, bool ok);

        public static void GetInput(String title, String message, bool modal, SendResult<String> sendResult)
        {
            InputBox inputBox = new InputBox(title, message, "");
            inputBox.SendResult = sendResult;
            inputBox.Closed += new EventHandler(inputBox_Closed);
        }

        private static void inputBox_Closed(object sender, EventArgs e)
        {
            InputBox inputBox = (InputBox)sender;
            String errorPrompt = null;
            if (inputBox.Accepted && !inputBox.SendResult(inputBox.Text, ref errorPrompt))
            {
                inputBox.Message = errorPrompt;
                inputBox.open(inputBox.Modal);
            }
            else
            {
                inputBox.Dispose();
            }
        }
    }
}
