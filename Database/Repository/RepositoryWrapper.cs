using Database.Contracts;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private ISessionRepository _session;
        private IPlayerRepository _player;

        public ISessionRepository session
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionRepository(_repoContext);
                }

                return _session;
            }
        }

        public IPlayerRepository player
        {
            get
            {
                if (_player == null)
                {
                    _player = new PlayerRepository(_repoContext);
                }

                return _player;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
