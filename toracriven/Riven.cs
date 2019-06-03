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
        private static readonly HealthPrediction healthPrediction = new HealthPrediction();
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

        public static void OnLoad()
        {
            if (Player.CharacterName != "Riven") return;
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 300);
            R = new Spell(SpellSlot.R, 900);

            R.SetSkillshot(0.25f, 45, 1600, false, SkillshotType.Cone);

            OnMenuLoad();

            if (Timer != null)
            {
                Timer = new Render.Text("Q Expiry =>  " + ((double)(LastQ - Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0"), (int)Drawing.WorldToScreen(Player.Position).X - 140, (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.MidnightBlue, "calibri");
                Timer2 = new Render.Text("R Expiry =>  " + (((double)LastR - Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0"), (int)Drawing.WorldToScreen(Player.Position).X - 60, (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.IndianRed, "calibri");

                Game.OnUpdate += OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                AIBaseClient.OnProcessSpellCast += OnCast;
                AIBaseClient.OnDoCast += OnDoCast;
                AIBaseClient.OnDoCast += OnDoCastLC;
                AIBaseClient.OnPlayAnimation = += OnPlay;
                AIBaseClient.OnProcessSpellCast += OnCasting;
                Interrupter.OnInterrupterSpell += Interrupt;
            }
        }

        private static bool HasTitan() => Items.HasItem(3748) && Items.CanUseItem(3748);

        private static void CastTitan()
        {
            if (Items.HasItem(3748) && Items.CanUseItem(3748))
            {
                Items.UseItem(3748);
                Orbwalker.LastAutoAttackTick = 0;
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (Dind)
                {
                    Indicator.unit = enemy;
                    healthPrediction.
                    Indicator.drawDmg(getComboDamage(enemy), new ColorBGRA(255, 204, 0, 170));
                }

            }
        }

        private static void OnDoCastLC(AIBaseClient Sender, AIBaseClientProcessSpellCastEventArgs args)
        {

            if (!Sender.IsMe || !Orbwalker.IsAutoAttack((args.SData.Name))) return;
            QTarget = (AIBaseClient)args.Target;
            if (args.Target is AIMinionClient)
            {
                if (Orbwalker.ActiveMode == OrbwalkerMode.LaneClear)
                {
                    var Minions = MinionManager.GetMinions(70 + 120 + Player.BoundingRadius);
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (Q.IsReady() && LaneQ)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(Minions[0]));
                    }
                    if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && W.IsReady() && LaneW != 0 &&
                        Minions.Count >= LaneW)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, ForceW);
                    }
                    if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && (!W.IsReady() || (W.IsReady() && LaneW == 0) || Minions.Count < LaneW) &&
                        E.IsReady() && LaneE)
                    {
                        E.Cast(Minions[0].Position);
                        DelayAction.Add(1, ForceItem);
                    }
                }
            }
        }

        private static int Item => Items.CanUseItem(3077) && Items.HasItem(3077) ? 3077 : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;

        private static void OnDoCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalker.IsAutoAttack(spellName)) return;
            QTarget = (AIBaseClient)args.Target;

            if (args.Target is AIMinionClient)
            {
                if (Orbwalker.ActiveMode == OrbwalkerMode.LaneClear)
                {
                    var Mobs = MinionManager.GetMinions(120 + 70 + Player.BoundingRadius, MinionTypes.All,
                        MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    if (Mobs.Count != 0)
                    {
                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (Q.IsReady())
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, () => ForceCastQ(Mobs[0]));
                        }
                        else if (W.IsReady())
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, ForceW);
                        }
                        else if (E.IsReady())
                        {
                            E.Cast(Mobs[0].Position);
                        }
                    }
                }
            }
            if (args.Target is AITurretClient || args.Target is Barracks || args.Target is BarracksDampenerClient || args.Target is BuildingClient) if (args.Target.IsValid && args.Target != null && Q.IsReady() && LaneQ && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) ForceCastQ((Obj_AI_Base)args.Target);

            if (args.Target is AIHeroClient)
            {
            var target = (AIHeroClient)args.Target;
                if (KillstealR && R.IsReady() && R.Instance.Name == IsSecondR) if (target.Health < (Rdame(target, target.Health) + Player.GetAutoAttackDamage(target)) && target.Health > Player.GetAutoAttackDamage(target)) R.Cast(target.Position);
                if (KillstealW && W.IsReady()) if (target.Health < (W.GetDamage(target) + Player.GetAutoAttackDamage(target)) && target.Health > Player.GetAutoAttackDamage(target)) W.Cast();
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (Q.IsReady())
                    {
                        ForceItem();
                        DelayAction.Add(1, ()=>ForceCastQ(target));
                    }
                    else if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, ForceW);
                    }
                    else if (E.IsReady() && !Orbwalking.InAutoAttackRange(target)) E.Cast(target.Position);
                }
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, ForceW);
                        Utility.DelayAction.Add(2, () => ForceCastQ(target));
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1,()=>ForceCastQ(target));
                    }
                    else if (E.IsReady() && !Orbwalking.InAutoAttackRange(target) && !InWRange(target))
                    {
                        E.Cast(target.Position);
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (QStack == 2 && Q.IsReady())
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (R.IsReady() && R.Instance.Name == IsSecondR)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, ForceR2);
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, ()=>ForceCastQ(target));
                    }
                }
            }
        }

    }
}