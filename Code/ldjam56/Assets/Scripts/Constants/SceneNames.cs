using System;
using System.Collections.Generic;

namespace Assets.Scripts.Constants
{
    public class SceneNames
    {
        public const String MainMenu = "MainMenuScene";
        public const String Credits = "CreditsScene";
        public const String Options = "OptionsScene";
        public const String Game = "GameScene";
        public const String MovementTest = "MovementTestScene";
        public const String TerrainTest = "TerrainTestScene";

        public static List<String> scenes = new() { MainMenu, Options, Credits, Game };
        public static List<String> scenesDevelopment = new() { };
    }
}