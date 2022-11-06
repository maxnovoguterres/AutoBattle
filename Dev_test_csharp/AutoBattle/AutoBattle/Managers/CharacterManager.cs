using AutoBattle.Controllers;

namespace AutoBattle.Managers
{
    public static class CharacterManager
    {
        private static Character PlayerCharacter;
        private static Character EnemyCharacter;

        public static void SetPlayerCharacter (Character character) => PlayerCharacter = character;

        public static Character GetPlayerCharacter () => PlayerCharacter;

        public static void SetEnemyCharacter (Character character) => EnemyCharacter = character;

        public static Character GetEnemyCharacter () => EnemyCharacter;
    }
}
