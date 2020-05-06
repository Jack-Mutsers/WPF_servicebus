using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ISessionRepository
    {
        bool ValidateIfActive(string sessionCode);
        Session GetByCode(string sessionCode);
        void CreateSession(Session session);
        void UpdateSession(Session session);
        void DeleteSession(Session session);
    }
}
