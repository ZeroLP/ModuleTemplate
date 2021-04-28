using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Oasys.SDK;
using Oasys.SDK.Tools;
using Oasys.SDK.Events;

namespace ModuleTemplate
{
    public class Main
    {
        [OasysModuleEntryPoint]
        public static void Execute()
        {
            GameEvents.OnGameLoadComplete += GameEvents_OnGameLoadComplete;
            GameEvents.OnGameMatchComplete += GameEvents_OnGameMatchComplete;
        }

        private static void GameEvents_OnGameLoadComplete()
        {
            if (UnitManager.MyChampion.ModelName == "Xerath")
            {
                Logger.Log("Thank you for using ModuleTemplate by Zero. Made with <3");

                CoreEvents.OnCoreMainTick += CoreEvents_OnCoreMainTick;

                Sequences.SequenceManager.StartSequences();
            }
            else
                Logger.Log("Thank you for using ModuleTemplate by Zero. Made with <3. However Xerath isn't detected for this match.");
        }

        public static int CacheTick = 0;
        private static void CoreEvents_OnCoreMainTick()
        {
            CacheTick += 10;
        }

        private static void GameEvents_OnGameMatchComplete()
        {
            Sequences.SequenceManager.TerminateSequences();
        }
    }
}
