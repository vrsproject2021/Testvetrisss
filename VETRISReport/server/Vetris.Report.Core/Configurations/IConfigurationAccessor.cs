using Vetris.Report.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Configurations
{
    public interface IConfigurationAccessor : ISingletonDependency
    {
        string ConnectionString { get; }
        string Get(string key);
    }
}
