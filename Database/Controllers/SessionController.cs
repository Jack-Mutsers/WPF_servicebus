//using AutoMapper;
using Contracts;
using Entities;
using Entities.Models;
using Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Database.Controllers
{
    public class SessionController
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public SessionController()
        {
            var config = new MapperConfiguration(cfg => new MappingProfile());
            _mapper = config.CreateMapper();

            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            SqlData sqlData = new SqlData();
            optionsBuilder.UseMySql(sqlData.connectionString);

            _repository = new RepositoryWrapper(new RepositoryContext(optionsBuilder.Options));
        }

        public bool CheckIfSessionExists(string sessionCode)
        {
            return _repository.session.ValidateIfActive(sessionCode);
        }

        public void CreateSession(String sessionCode)
        {
            Session session = new Session() {
                active = true,
                session_code = sessionCode,
                creation_date = DateTime.Now
            };

            _repository.session.CreateSession(session);
            _repository.Save();
        }

        public void DeleteSession(string sessionCode)
        {
            Session session = _repository.session.GetByCode(sessionCode);

            _repository.session.DeleteSession(session);
            _repository.Save();
        }


    }
}
