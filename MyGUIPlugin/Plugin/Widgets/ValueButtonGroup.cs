using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class ValueButtonGroup<EnumType>
    {
        private ButtonGroup buttonGroup;
        private EnumType defaultValue;

        public ValueButtonGroup(EnumType defaultValue)
        {
            buttonGroup = new ButtonGroup();
            this.defaultValue = defaultValue;
        }

        public void addButton(Button button, EnumType value)
        {
            button.UserObject = value;
            buttonGroup.addButton(button);
        }

        public void removeButton(Button button)
        {
            buttonGroup.removeButton(button);
        }

        public void removeButton(EnumType value)
        {
            removeButton(buttonGroup.findButtonWithUserData(value));
        }

        public EnumType SelectedValue
        {
            get
            {
                return buttonGroup.SelectedButton != null ? (EnumType)buttonGroup.SelectedButton.UserObject : defaultValue;
            }
            set
            {
                buttonGroup.SelectedButton = buttonGroup.findButtonWithUserData(value);
            }
        }
    }
}
