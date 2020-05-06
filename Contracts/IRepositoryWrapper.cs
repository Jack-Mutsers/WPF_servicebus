using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IPlayerRepository player { get; }
        ISessionRepository session { get; }
        void Save();
    }
}
