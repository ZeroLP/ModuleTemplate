using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oasys.SDK;
using Oasys.SDK.InputProviders;
using Oasys.SDK.SpellCasting;
using Oasys.SDK.Events;
using Oasys.SDK.Tools;
using Oasys.Common.Enums.GameEnums;
using Oasys.Common.GameObject;

namespace ModuleTemplate.Sequences
{
    public class Combo : SequenceBase
    {
        public override void Start()
        {
            CoreEvents.OnCoreMainInput += CoreEvents_OnCoreMainInput;
            CoreEvents.OnCoreMainInputRelease += CoreEvents_OnCoreMainInputRelease;
            CoreEvents.OnCoreMainTick += CoreEvents_OnCoreMainTick;
        }

        public override void Terminate()
        {
            CoreEvents.OnCoreMainInput -= CoreEvents_OnCoreMainInput;
            CoreEvents.OnCoreMainInputRelease -= CoreEvents_OnCoreMainInputRelease;
            CoreEvents.OnCoreMainTick -= CoreEvents_OnCoreMainTick;


            base.Terminate();
        }

        private static GameObjectBase CachedTarget { get; set; }

        private void CoreEvents_OnCoreMainTick()
        {
            var mySpellBook = UnitManager.MyChampion.GetSpellBook();

            CachedTarget = UnitManager.EnemyChampions
                .Where(x => IsInRange(x, 1450/*Max Q Cast Range*/)
                         && IsInRange(x, mySpellBook.GetSpellClass(SpellSlot.W).SpellData.CastRange)
                         && IsInRange(x, mySpellBook.GetSpellClass(SpellSlot.E).SpellData.CastRange))
                .OrderBy(x => x.Distance)
                .ToList().FirstOrDefault();
        }

        private void CoreEvents_OnCoreMainInput()
        {
            if(CachedTarget != null)
            {
                SpellCastProvider.CastMultiSpell(new CastSlot[]
                {
                    CastSlot.E,
                    CastSlot.W
                }, CachedTarget.Position);

                if(ReleaseTick == 0)
                {
                    MouseProvider.SetCursor(new Oasys.Common.Tools.Pos(CachedTarget.W2S.X, CachedTarget.W2S.Y));
                    KeyboardProvider.PressKeyDown(KeyboardProvider.KeyBoardScanCodes.KEY_Q);

                    var timeToExtendFor = CalculateQExtendableTime(CachedTarget.Distance);
                    ReleaseTick = Main.CacheTick + timeToExtendFor;
                }

                if(Main.CacheTick >= ReleaseTick)
                {
                    KeyboardProvider.PressKeyUp(KeyboardProvider.KeyBoardScanCodes.KEY_Q);
                    ReleaseTick = 0;
                }
            }
        }

        private static int ReleaseTick = 0;
        private void CoreEvents_OnCoreMainInputRelease()
        {
            if (ReleaseTick > 0)
            {
                KeyboardProvider.PressKeyUp(KeyboardProvider.KeyBoardScanCodes.KEY_Q);
                ReleaseTick = 0;
            }
        }

        private static int CalculateQExtendableTime(float distance)
        {
            switch (distance)
            {
                case float dfl when dfl <= 735f:
                    return 0;
                case float df1 when df1 > 735f && df1 <= 837.14f:
                    return 250;
                case float dfl when dfl > 837.14f && dfl <= 939.29f:
                    return 500;
                case float dfl when dfl > 939.29f && dfl <= 1041.43f:
                    return 750;
                case float dfl when dfl > 1041.43f && dfl <= 1143.57f:
                    return 1000;
                case float dfl when dfl > 1143.57f && dfl <= 1245.71f:
                    return 1250;
                case float dfl when dfl > 1245.71f && dfl <= 1347.86f:
                    return 1500;
                case float dfl when dfl > 1347.86f && dfl <= 1450f:
                    return 1750;
            }

            return 0;
        }

        private static bool IsInRange(GameObjectBase targHero, float range)
        {
            return targHero.Distance <= range + targHero.UnitComponentInfo.UnitBoundingRadius + 5;
        }
    }
}
