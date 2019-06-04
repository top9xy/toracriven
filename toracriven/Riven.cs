namespace toracriven
{
    using System;
    using System.Linq;
    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using EnsoulSharp.SDK.MenuUI;
    using EnsoulSharp.SDK.Prediction;
    using EnsoulSharp.SDK.Utility;
    using SharpDX;

    internal class Riven
    {
        private static Menu MyMenu;
        private static AIHeroClient Player = ObjectManager.Player;
        private static readonly HpBarIndicator hpBarIndicator = new HpBarIndicator();
        private const string IsFirstR = "RivenFengShuiEngine";
        private const string IsSecondR = "RivenIzunaBlade";
        //private static readonly SpellSlot Flash = Player.SpellBlock("summonerFlash");
        private static Spell Q, Q1, W, E, R;
        private static int QStack = 1;
        public static Render.Text Timer, Timer2;
        private static bool forceQ;
        private static bool forceW;
        private static bool forceR;
        private static bool forceR2;
        private static bool forceItem;
        private static float LastQ;
        private static float LastR;
        private static AttackableUnit QTarget;
        private static bool Dind => MenuWrapper.Draw.Dind.Enabled;
        private static bool DrawCB => MenuWrapper.Draw.DrawCB.Enabled;
        private static bool KillstealW => MenuWrapper.Misc.killstealw.Enabled;
        private static bool KillstealR => MenuWrapper.Misc.killstealr.Enabled;
        private static bool DrawAlwaysR => MenuWrapper.Draw.DrawAlwaysR.Enabled;
        private static bool DrawUseTorac => MenuWrapper.Draw.DrawUseTorac.Enabled;
        private static bool DrawFH => MenuWrapper.Draw.DrawFH.Enabled;
        private static bool DrawTimer1 => MenuWrapper.Draw.DrawTimer1.Enabled;
        private static bool DrawTimer2 => MenuWrapper.Draw.DrawTimer2.Enabled;
        private static bool DrawHS => MenuWrapper.Draw.DrawHS.Enabled;
        private static bool DrawBT => MenuWrapper.Draw.DrawBT.Enabled;
        private static bool UseTorac => MenuWrapper.Combo.UseTorac.Enabled;
        private static bool AlwaysR => MenuWrapper.Combo.AlwaysR.Enabled;
        private static bool AutoShield => MenuWrapper.Misc.AutoShield.Enabled;
        private static bool Shield => MenuWrapper.Misc.Shield.Enabled;
        private static bool KeepQ => MenuWrapper.Misc.KeepQ.Enabled;
        private static int QD => MenuWrapper.Misc.QD.Value;
        private static int QLD => MenuWrapper.Misc.QLD.Value;
        private static int AutoW => MenuWrapper.Misc.AutoW.Value;
        private static bool ComboW => MenuWrapper.Combo.ComboW.Enabled;
        private static bool RMaxDam => MenuWrapper.Misc.RMaxDam.Enabled;
        private static bool RKillable => MenuWrapper.Combo.RKillable.Enabled;
        private static bool LaneE => MenuWrapper.Lane.LaneE.Enabled;
        private static bool WInterrupt => MenuWrapper.Misc.Winterrupt.Enabled;
        private static bool Qstrange => MenuWrapper.Misc.Qstrange.Enabled;
        private static bool FirstHydra => MenuWrapper.Misc.FirstHydra.Enabled;
        private static bool LaneQ => MenuWrapper.Lane.LaneQ.Enabled;
        private static bool Youmu => MenuWrapper.Misc.youmu.Enabled;
    }
}