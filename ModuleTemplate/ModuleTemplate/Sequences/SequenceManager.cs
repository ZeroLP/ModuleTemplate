using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oasys.SDK.Tools;
using Oasys.SDK.Events;

namespace ModuleTemplate.Sequences
{
    public class SequenceManager
    {
        private static List<SequenceBase> InitializedSequences { get; set; }

        static SequenceManager()
        {
            Logger.Log($"Initializing sequences.");

            //Each sequences follow after one another before, in the order added.
            InitializedSequences = new List<SequenceBase>()
            {
                new Combo()
            };
        }

        public static void StartSequences()
        {
            //Checks whether if the InitializedSequences list is initialized and populated.
            if (InitializedSequences != null && InitializedSequences.Count != 0)
            {
                foreach (var seq in InitializedSequences)
                {
                    try { seq.Start(); }
                    catch (Exception ex)
                    { Logger.Log($"Exception occured while starting sequence: {seq.GetType().Name}. Exception: {ex.Message}"); }
                }
            }
        }

        public static void TerminateSequences()
        {
            if (InitializedSequences != null && InitializedSequences.Count != 0)
            {
                foreach (var seq in InitializedSequences)
                {
                    try { seq.Terminate(); }
                    catch (Exception ex)
                    { Logger.Log($"Exception occured while terminating sequence: {seq.GetType().Name}. Exception: {ex.Message}"); }
                }
            }
        }
    }
}
