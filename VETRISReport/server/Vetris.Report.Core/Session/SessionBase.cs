using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Session
{
    public abstract class SessionBase : ISessionInfo
    {
        public abstract Guid? UserId { get; }
        public abstract string UserName { get; }
    }
}
