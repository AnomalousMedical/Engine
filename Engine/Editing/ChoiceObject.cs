using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This class represents an value that can have choices presented.
    /// </summary>
    public class ChoiceObject
    {
        List<Pair<String, Object>> choices = new List<Pair<string, Object>>();

        public ChoiceObject()
        {

        }

        public ChoiceObject(IEnumerable<Pair<String, Object>> choices)
        {
            addChoices(choices);
        }

        public void addChoice(String display, Object value)
        {
            choices.Add(new Pair<string, object>(display, value));
        }

        public void addChoices(IEnumerable<Pair<String, Object>> choices)
        {
            this.choices.AddRange(choices);
        }

        public void removeChoice(Object value)
        {
            foreach (var choice in choices)
            {
                if (choice.Second == value)
                {
                    choices.Remove(choice);
                    break;
                }
            }
        }

        public IEnumerable<Pair<String, Object>> Choices
        {
            get
            {
                return choices;
            }
        }
    }
}
