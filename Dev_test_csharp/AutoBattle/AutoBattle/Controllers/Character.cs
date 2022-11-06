using System;
using System.Collections.Generic;
using static AutoBattle.Types;
using AutoBattle.Utils;
using AutoBattle.Enums;
using AutoBattle.Managers;

namespace AutoBattle.Controllers
{
    public class Character
    {
        public string name { get; set; }
        public float health;
        public float baseDamage;
        public float damageMultiplier { get; set; }
        public GridBox currentBox;
        public int playerIndex;
        public Character target { get; set; }
        public CharacterClassSpecific characterClassSpecific;

        public Character(CharacterClass characterClass)
        {
            List<CharacterSkills> skills = new List<CharacterSkills>();
            skills.Add(new CharacterSkills
            {
                name = "Invulnerable",
                damage = 0,
                damageMultiplier = 0,
                turnsRemaining = RandomExtensions.GetRandomInt(1, 3),
                isActive = false,
                used = false,
                skill = Skill.Invulnerable
            });
            skills.Add(new CharacterSkills
            {
                name = "Empower",
                damage = 15,
                damageMultiplier = 1.25f,
                turnsRemaining = RandomExtensions.GetRandomInt(1, 3),
                isActive = false,
                used = false,
                skill = Skill.Empower
            });

            switch (characterClass)
            {
                case CharacterClass.Paladin:
                    characterClassSpecific = new CharacterClassSpecific(characterClass, 30, 10, skills.ToArray());
                    break;
                case CharacterClass.Warrior:
                    characterClassSpecific = new CharacterClassSpecific(characterClass, 10, 30, skills.ToArray());
                    break;
                case CharacterClass.Cleric:
                    characterClassSpecific = new CharacterClassSpecific(characterClass, 50, 8, skills.ToArray());
                    break;
                case CharacterClass.Archer:
                    characterClassSpecific = new CharacterClassSpecific(characterClass, 20, 15, skills.ToArray());
                    break;
            }
        }

        public void TakeDamage(float amount)
        {
            if((health -= amount) <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            GameManager.SetGameState(GameState.GameOver);
            Console.Write(Environment.NewLine + Environment.NewLine);
            Console.WriteLine($"Game ended in turn {GameManager.GetCurrentTurn()}, {name} defeated. {target.name} Won!");
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

        public void StartTurn(Grid battlefield)
        {
            if (GameManager.GetGameState() == GameState.GameOver)
            {
                return;
            }

            if (CheckCloseTargets(battlefield)) 
            {
                Attack(target);
            }
            else
            {
                // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if(currentBox.xIndex > target.currentBox.xIndex)
                {
                    Move(battlefield, battlefield.grids.Find(x => x.index == currentBox.index - 1));
                    Console.WriteLine($"{name} walked left and is now positioned in tile {currentBox.index}\n");
                } 
                else if(currentBox.xIndex < target.currentBox.xIndex)
                {
                    Move(battlefield, battlefield.grids.Find(x => x.index == currentBox.index + 1));
                    Console.WriteLine($"{name} walked right and is now positioned in tile {currentBox.index}\n");
                }
                else if (currentBox.yIndex > target.currentBox.yIndex)
                {
                    Move(battlefield, battlefield.grids.Find(x => x.index == currentBox.index - battlefield.gridSize.y));
                    Console.WriteLine($"{name} walked up and is now positioned in tile {currentBox.index}\n");
                }
                else if(currentBox.yIndex < target.currentBox.yIndex)
                {
                    Move(battlefield, battlefield.grids.Find(x => x.index == currentBox.index + battlefield.gridSize.y));
                    Console.WriteLine($"{name} walked down and is now positioned in tile {currentBox.index}\n");
                }

                battlefield.DrawBattlefield();
            }

            for (int i = 0; i < characterClassSpecific.skills.Length; i++)
            {
                if (!characterClassSpecific.skills[i].isActive)
                {
                    continue;
                }

                characterClassSpecific.skills[i].turnsRemaining--;

                if (characterClassSpecific.skills[i].turnsRemaining == 0)
                {
                    characterClassSpecific.skills[i].isActive = false;
                    Console.WriteLine($"{characterClassSpecific.skills[i].name} skill used from {name} is now disabled\n");
                }
                else
                {
                    Console.WriteLine($"{characterClassSpecific.skills[i].name} skill used from {name} has {characterClassSpecific.skills[i].turnsRemaining} turns left\n");
                }
            }

            TrySkillsToUse();
        }

        private void Move (Grid battlefield, GridBox nextBox)
        {
            currentBox.ocupied = false;
            nextBox.ocupied = true;
            battlefield.grids[currentBox.index] = currentBox;
            battlefield.grids[nextBox.index] = nextBox;
            currentBox = nextBox;
        }

        private void TrySkillsToUse ()
        {
            int random = RandomExtensions.GetRandomInt(0, 10);
            if (random >= 7)
            {
                for (int i = 0; i < characterClassSpecific.skills.Length; i++)
                {
                    if (characterClassSpecific.skills[i].used)
                    {
                        continue;
                    }

                    switch (characterClassSpecific.skills[i].skill)
                    {
                        case Skill.Invulnerable:
                            Console.WriteLine($"{name} used Invulnerable skill for {characterClassSpecific.skills[i].turnsRemaining} turns\n");
                            break;
                        case Skill.Empower:
                            Console.WriteLine($"{name} used Empower skill for {characterClassSpecific.skills[i].turnsRemaining} turns\n");
                            break;
                    }

                    characterClassSpecific.skills[i].isActive = true;
                    characterClassSpecific.skills[i].used = true;
                    break;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = battlefield.grids.Find(x => x.index == currentBox.index - 1).ocupied;
            bool right = battlefield.grids.Find(x => x.index == currentBox.index + 1).ocupied;
            bool up = battlefield.grids.Find(x => x.index == currentBox.index + battlefield.gridSize.y).ocupied;
            bool down = battlefield.grids.Find(x => x.index == currentBox.index - battlefield.gridSize.y).ocupied;

            if (left | right | up | down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            foreach (CharacterSkills skill in target.characterClassSpecific.skills)
            {
                if (skill.skill == Skill.Invulnerable && skill.isActive)
                {
                    Console.WriteLine($"{name} is attacking the player {this.target.name} but miss attack because this player is invulnerable\n");
                    return;
                }
            }

            int empoweredDamage = 0;
            foreach (CharacterSkills skill in characterClassSpecific.skills)
            {
                if (skill.skill == Skill.Empower && skill.isActive)
                {
                    empoweredDamage = (int)(skill.damage * skill.damageMultiplier);
                }
            }
            int randomDamage = RandomExtensions.GetRandomInt(0, (int)(baseDamage * damageMultiplier) + (int)characterClassSpecific.classDamage + empoweredDamage);
            Console.WriteLine($"{name} is attacking the player {this.target.name} and did {randomDamage} damage\n");
            target.TakeDamage(randomDamage);
        }
    }
}
