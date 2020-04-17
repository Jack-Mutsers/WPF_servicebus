using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Contracts
{
    public interface ISessionRepository
    {
        bool ValidateIfActive(string sessionCode);
        void CreateSession(Session session);
        void UpdateSession(Session session);
        void DeleteSession(Session session);
    }
}
