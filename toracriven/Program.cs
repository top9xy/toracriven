namespace toracriven
{
    using System;
    using EnsoulSharp;
    using EnsoulSharp.SDK;
    internal class Program
    {
        public static void Main(string[] args)
        {
            GameEvent.OnGameLoad += OnGameLoad;
        }
        private static void OnGameLoad()
        {
            if (ObjectManager.Player.CharacterName != "Riven")
                return;

            Riven.OnLoad();
        }
    }
}
