using System;
using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public interface IServiceManager
    {
        Estate Estate { get; }
        ConnectionInfo GetConnectionInfo();
        void SetConnectionInfo(ConnectionInfo connectionInfo);
    }
}
