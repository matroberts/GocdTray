using System;
using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public interface IServiceManager
    {
        event Action OnStatusChange;
        Estate Estate { get; }
        ConnectionInfo GetConnectionInfo();
        ValidationResult SetConnectionInfo(ConnectionInfo connectionInfo);
        ValidationResult TestConnectionInfo(ConnectionInfo connectionInfo);
    }
}
