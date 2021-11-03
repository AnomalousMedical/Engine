using RpgMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace SceneTest
{
    class Party
    {
        private List<Character> characters = new List<Character>();

        public IEnumerable<Character> ActiveCharacters => characters;

        public long Gold { get; set; }

        public void AddCharacter(Character character)
        {
            characters.Add(character);
        }
    }
}
