using AutoBattle.Enums;

namespace AutoBattle
{
    public class Types
    {
        public struct CharacterClassSpecific
        {
            public CharacterClass characterClass;
            public float hpModifier;
            public float classDamage;
            public CharacterSkills[] skills;

            public CharacterClassSpecific (CharacterClass characterClass, float hpModifier, float classDamage, CharacterSkills[] skills)
            {
                this.characterClass = characterClass;
                this.hpModifier = hpModifier;
                this.classDamage = classDamage;
                this.skills = skills;
            }
        }

        public struct CharacterSkills
        {
            public string name;
            public float damage;
            public float damageMultiplier;
            public int turnsRemaining;
            public bool isActive;
            public bool used;
            public Skill skill;
        }

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int index;

            public GridBox(int x, int y, bool ocupied, int index)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.index = index;
            }
        }
    }
}
