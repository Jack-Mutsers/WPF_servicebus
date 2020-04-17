using AutoMapper;
using Database.Contracts;
using Database.Entities;
using Database.Entities.Models;
using Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Database.Controlers
{
    class PlayerController
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public PlayerController()
        {
            var config = new MapperConfiguration(cfg => new MappingProfile());
            _mapper = config.CreateMapper();

            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            SqlData sqlData = new SqlData();
            optionsBuilder.UseSqlServer(sqlData.connectionString);

            _repository = new RepositoryWrapper(new RepositoryContext(optionsBuilder.Options));
        }
    }
}
