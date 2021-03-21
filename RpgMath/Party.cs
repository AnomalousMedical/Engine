using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class Party
    {
        private List<CharacterSheet> characters = new List<CharacterSheet>();

        public IEnumerable<CharacterSheet> ActiveCharacters => characters;

        public void AddCharacter(CharacterSheet character)
        {
            characters.Add(character);
        }
    }
}
