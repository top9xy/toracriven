namespace toracriven
{
    using System.Windows.Forms;
    using EnsoulSharp.SDK.MenuUI.Values;
    internal class MenuWrapper
    {
        public class Combo
        {
            public static readonly MenuBool AlwaysR = new MenuBool("AlwaysR", "Always Use R");
            public static readonly MenuBool UseTorac = new MenuBool("AlwaysR", "Use Combo Logic");
            public static readonly MenuBool ComboW = new MenuBool("AlwaysR", "Always use W");
            public static readonly MenuBool RKillable = new MenuBool("RKillable", "Use R When Target Can Killable");
        }

        public class Lane
        {
            public static readonly MenuBool LaneQ = new MenuBool("LaneQ", "Use Q While Laneclear");
            public static readonly MenuBool LaneE = new MenuBool("LaneE", "Use E While Laneclear");
        }

        public class Misc
        {
            public static readonly MenuBool youmu = new MenuBool("youmu", "Use Youmus When E");
            public static readonly MenuBool FirstHydra = new MenuBool("FirstHydra", "Flash Burst Hydra Cast before W");
            public static readonly MenuBool Qstrange = new MenuBool("Qstrange", "Strange Q For Speedr");
            public static readonly MenuBool Winterrupt = new MenuBool("Winterrupt", "W interrupt");
            public static readonly MenuSlider AutoW = new MenuSlider("AutoW", "Auto W When x Enemy", 5, 0, 5);
            public static readonly MenuBool RMaxDam = new MenuBool("RMaxDam", "Use Second R Max Damage");
            public static readonly MenuBool killstealw = new MenuBool("killstealw", "Killsteal W");
            public static readonly MenuBool killstealr = new MenuBool("killstealr", "Killsteal Second R");
            public static readonly MenuBool AutoShield = new MenuBool("AutoShield", "Auto Cast E");
            public static readonly MenuBool Shield = new MenuBool("Shield", "Auto Cast E While LastHit");
            public static readonly MenuBool KeepQ = new MenuBool("KeepQ", "Keep Q Alive");
            public static readonly MenuSlider QD = new MenuSlider("QD", "First,Second Q Delay", 29, 23, 43);
            public static readonly MenuSlider QLD = new MenuSlider("QLD", "Third Q Delay", 39, 36, 53);
        }

        public class Draw
        {
            public static readonly MenuBool DrawAlwaysR = new MenuBool("DrawAlwaysR", "Use Q While Laneclear");
            public static readonly MenuBool DrawTimer1 = new MenuBool("DrawTimer1", "Use Q While Laneclear");
            public static readonly MenuBool DrawTimer2 = new MenuBool("DrawTimer2", "Use Q While Laneclear");
            public static readonly MenuBool DrawUseTorac = new MenuBool("DrawUseTorac", "Draw Use Torac Logic");
            public static readonly MenuBool Dind = new MenuBool("Dind", "Draw Damage Indicator");
            public static readonly MenuBool DrawCB = new MenuBool("DrawCB", "Draw Combo Engage Range");
            public static readonly MenuBool DrawBT = new MenuBool("DrawBT", "Draw Burst Engage Range");
            public static readonly MenuBool DrawFH = new MenuBool("DrawFH", "Draw FastHarass Engage Range");
            public static readonly MenuBool DrawHS = new MenuBool("DrawHS", "Draw Harass Engage Range");
        }
    }
}
