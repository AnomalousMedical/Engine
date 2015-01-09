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
        private EditBox input;
        private TextBox prompt;
        private SendResult<String> SendResult;
        private Size2 deltaSize;
        private int minimumWidth;

        public InputBox(String title, String message, String text)
            :this(title, message, text, "MyGUIPlugin.Plugin.Widgets.InputBox.layout")
        {
            
        }

        public InputBox(String title, String message, String text, String customLayout)
            :base(customLayout)
        {
            input = window.findWidget("Input") as EditBox;
            input.Caption = text;
            input.KeyButtonPressed += input_KeyButtonPressed;

            prompt = window.findWidget("Prompt") as TextBox;
            //Figure out the size difference of the prompt and window
            minimumWidth = window.Width;
            deltaSize = new Size2(window.Width - prompt.Width, window.Height - prompt.Height);
            Message = message;

            window.Caption = title;

            Button ok = window.findWidget("Ok") as Button;
            ok.MouseButtonClick += new MyGUIEvent(ok_MouseButtonClick);

            Button cancel = window.findWidget("Cancel") as Button;
            cancel.MouseButtonClick += new MyGUIEvent(cancel_MouseButtonClick);

            Accepted = false;
        }

        protected override void onShown(EventArgs args)
        {
            base.onShown(args);
            InputManager.Instance.setKeyFocusWidget(input);
        }

        public void selectAllText()
        {
            input.setTextSelection(0, uint.MaxValue);
        }

        public String Text
        {
            get
            {
                return input.OnlyText;
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
                if(textSize.Width < minimumWidth)
                {
                    textSize.Width = minimumWidth;
                }
                window.setSize((int)textSize.Width, (int)textSize.Height);
            }
        }

        public bool Accepted { get; private set; }

        void input_KeyButtonPressed(Widget source, EventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            switch (ke.Key)
            {
                case KeyboardButtonCode.KC_RETURN:
                    ok_MouseButtonClick(source, e);
                    break;
                case KeyboardButtonCode.KC_ESCAPE:
                    cancel_MouseButtonClick(source, e);
                    break;
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

        public static void GetInput(String title, String message, bool modal, SendResult<String> sendResult)
        {
            InputBox inputBox = new InputBox(title, message, "");
            inputBox.SendResult = sendResult;
            inputBox.Closing += new EventHandler<DialogCancelEventArgs>(inputBox_Closing);
            inputBox.Closed += new EventHandler(inputBox_Closed);
            inputBox.center();
            inputBox.open(modal);
        }

        static void inputBox_Closing(object sender, DialogCancelEventArgs e)
        {
            InputBox inputBox = (InputBox)sender;
            String errorPrompt = null;
            if (inputBox.Accepted && !inputBox.SendResult(inputBox.Text, ref errorPrompt))
            {
                inputBox.Message = errorPrompt;
                inputBox.selectAllText();
                e.Cancel = true;
            }
        }

        private static void inputBox_Closed(object sender, EventArgs e)
        {
            InputBox inputBox = (InputBox)sender;
            inputBox.Dispose();
        }
    }
}
