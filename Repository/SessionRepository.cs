using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
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

        public Session GetByCode(string sessionCode)
        {
            return FindByCondition(s => s.session_code == sessionCode).FirstOrDefault();
        }

        public void UpdateSession(Session session)
        {
            Update(session);
        }

        public bool ValidateIfActive(string sessionCode)
        {
            DateTime date = DateTime.Now;

            int count = FindByCondition(session => session.active == true && session.creation_date > date.AddDays(-1) && session.session_code == sessionCode  ).Count();
            return count > 0;
        }
    }
}
