using System;
using System.Collections.Generic;
using System.Text;
using Vetris.Report.Core.Dependency;

namespace Vetris.Report.Core.Session
{
    public interface ISessionInfo: ISingletonDependency
    {
        Guid? UserId { get; }
        string UserName { get; }
    }
}
