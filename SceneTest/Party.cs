using RpgMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace SceneTest
{
    public class Party
    {
        private List<Character> characters = new List<Character>();

        public IEnumerable<Character> ActiveCharacters => characters;

        public void AddCharacter(Character character)
        {
            characters.Add(character);
        }
    }
}
