using System;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;
using AutoBattle.Controllers;
using AutoBattle.Utils;
using AutoBattle.Enums;

namespace AutoBattle.Managers
{
    public static class GameManager
    {
        private static int CurrentTurn;
        private static GameState GameState;
        private static List<Character> AllPlayers;

        public static void SetCurrentTurn (int turn) => CurrentTurn = turn;

        public static int GetCurrentTurn () => CurrentTurn;

        public static void SetGameState (GameState gameState) => GameState = gameState;

        public static GameState GetGameState () => GameState;

        public static void Init ()
        {
            SetCurrentTurn(0);
            SetGameState(GameState.Playing);
            int randomGridSizeX = RandomExtensions.GetRandomInt(5, 10);
            int randomGridSizeY = RandomExtensions.GetRandomInt(5, 10);
            GridManager.CreateGrid(randomGridSizeX, randomGridSizeY);
            AllPlayers = new List<Character>();
            SetupCharacters();
        }

        private static void SetupCharacters ()
        {
            GetPlayerChoice();
            CreateEnemyCharacter();

            StartGame(GridManager.GetGrid(), CharacterManager.GetPlayerCharacter(), CharacterManager.GetEnemyCharacter());
        }

        private static void GetPlayerChoice ()
        {
            //asks for the player to choose between for possible classes via console.
            Console.WriteLine("\nChoose Between One of this Classes:\n");
            Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
            //store the player choice in a variable
            string choice = Console.ReadLine();
            Console.WriteLine("Write your name");
            string nameChoice = Console.ReadLine();
            string availableChoices = "1234";
            if (!availableChoices.Contains(choice))
            {
                GetPlayerChoice();
            }
            else
            {
                CreatePlayerCharacter(int.Parse(choice), nameChoice);
            }
        }

        private static void CreatePlayerCharacter (int classIndex, string name)
        {
            CharacterClass playerCharacterClass = (CharacterClass)classIndex;
            Character playerCharacter = new Character(playerCharacterClass);
            playerCharacter.health = 100 + playerCharacter.characterClassSpecific.hpModifier;
            playerCharacter.baseDamage = 10;
            playerCharacter.damageMultiplier = 1.2f;
            playerCharacter.playerIndex = 0;
            playerCharacter.name = name;
            CharacterManager.SetPlayerCharacter(playerCharacter);
        }

        private static void CreateEnemyCharacter ()
        {
            //randomly choose the enemy class and set up vital variables
            int randomInteger = RandomExtensions.GetRandomInt(1, 4);
            CharacterClass enemyCharacterClass = (CharacterClass)randomInteger;
            Character enemyCharacter = new Character(enemyCharacterClass);
            enemyCharacter.health = 100 + enemyCharacter.characterClassSpecific.hpModifier;
            enemyCharacter.baseDamage = 10;
            enemyCharacter.damageMultiplier = 1.2f;
            enemyCharacter.playerIndex = 1;
            enemyCharacter.name = $"Bot {enemyCharacter.playerIndex}";
            CharacterManager.SetEnemyCharacter(enemyCharacter);
        }

        private static void StartGame (Grid grid, Character playerCharacter, Character enemyCharacter)
        {
            //populates the character variables and targets
            playerCharacter.target = enemyCharacter;
            enemyCharacter.target = playerCharacter;
            AllPlayers.Add(playerCharacter);
            AllPlayers.Add(enemyCharacter);
            AlocatePlayers();
            grid.DrawBattlefield();
            StartTurn();
        }

        private static void StartTurn ()
        {
            int currentTurn = GetCurrentTurn();

            if (currentTurn == 0)
            {
                Random random = new Random();
                AllPlayers = AllPlayers.OrderBy(a => random.Next()).ToList();
            }

            foreach (Character character in AllPlayers)
            {
                character.StartTurn(GridManager.GetGrid());
            }

            currentTurn++;
            SetCurrentTurn(currentTurn);
            HandleTurn(CharacterManager.GetPlayerCharacter(), CharacterManager.GetEnemyCharacter());
        }

        private static void HandleTurn (Character playerCharacter, Character enemyCharacter)
        {
            if (playerCharacter.health > 0 && enemyCharacter.health > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Click on any key to start the next turn...\n");
                Console.Write(Environment.NewLine + Environment.NewLine);

                Console.ReadKey();
                StartTurn();
            }
        }

        private static void AlocatePlayers ()
        {
            AlocatePlayerCharacter(GridManager.GetGrid(), CharacterManager.GetPlayerCharacter());
            AlocateEnemyCharacter(GridManager.GetGrid(), CharacterManager.GetEnemyCharacter());
        }

        private static void AlocatePlayerCharacter (Grid grid, Character playerCharacter)
        {
            int random = RandomExtensions.GetRandomInt(0, grid.grids.Count - 1);
            GridBox randomLocation = grid.grids.ElementAt(random);
            if (!randomLocation.ocupied)
            {
                randomLocation.ocupied = true;
                grid.grids[random] = randomLocation;
                playerCharacter.currentBox = grid.grids[random];
                Console.WriteLine($"Player Class Choice: {CharacterManager.GetPlayerCharacter().characterClassSpecific.characterClass} positioned in tile {randomLocation.index}");
            }
            else
            {
                AlocatePlayerCharacter(grid, playerCharacter);
            }
        }

        private static void AlocateEnemyCharacter (Grid grid, Character enemyCharacter)
        {
            int random = RandomExtensions.GetRandomInt(0, grid.grids.Count - 1);
            GridBox randomLocation = grid.grids.ElementAt(random);
            if (!randomLocation.ocupied)
            {
                randomLocation.ocupied = true;
                grid.grids[random] = randomLocation;
                enemyCharacter.currentBox = grid.grids[random];
                Console.WriteLine($"Enemy Class Choice: {CharacterManager.GetEnemyCharacter().characterClassSpecific.characterClass} positioned in tile {randomLocation.index}");
            }
            else
            {
                AlocateEnemyCharacter(grid, enemyCharacter);
            }
        }
    }
}
