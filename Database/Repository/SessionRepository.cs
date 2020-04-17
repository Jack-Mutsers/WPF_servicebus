using Database.Contracts;
using Database.Entities;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Repository
{
    class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreateSession(Session session)
        {
            Create(session);
        }

        public void DeleteSession(Session session)
        {
            session.active = false;
            Update(session);
        }

        public void UpdateSession(Session session)
        {
            Update(session);
        }

        public bool ValidateIfActive(string sessionCode)
        {
            int count = FindByCondition(session => session.active == true && session.session_code == sessionCode).Count();
            return count > 0;
        }
    }
}
