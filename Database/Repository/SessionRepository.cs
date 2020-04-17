using Database.Contracts;
using Database.Entities;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Repository
{
    class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreateSession(Session session)
        {
            throw new NotImplementedException();
        }

        public void DeleteSession(Session session)
        {
            throw new NotImplementedException();
        }

        public void UpdateSession(Session session)
        {
            throw new NotImplementedException();
        }

        public bool ValidateIfActive(string sessionCode)
        {
            throw new NotImplementedException();
        }
    }
}
