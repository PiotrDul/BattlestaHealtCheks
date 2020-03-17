using BattlestaHealthChecks.Interfeces.Jobs;
using System;

namespace BattlestaHealthChecks.Jobs
{
    public class SendMail : ISendMail
    {
        public void PageLoadError()
        {
            throw new NotImplementedException();
        }

        public void PageLoadTimeExceeded()
        {
            throw new NotImplementedException();
        }
    }
}
