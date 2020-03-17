using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Interfeces.Jobs
{
    public interface ISendMail
    {
        void PageLoadError();

        void PageLoadTimeExceeded();
    }
}
