using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleTemplate.Sequences
{
    /// <summary>
    /// This is our sequence container, where the sequences adopts it's template from.
    /// </summary>
    public class SequenceBase
    {
        public virtual void Start() { }
        public virtual void Terminate() { }
    }
}
