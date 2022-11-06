using System;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;
using AutoBattle.Controllers;
using AutoBattle.Utils;
using AutoBattle.Managers;
using AutoBattle.Enums;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager.Init();
        }
    }
}
