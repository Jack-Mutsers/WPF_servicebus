using AutoMapper;
using Contracts;
using Entities;
using Entities.Models;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace Database.Controllers
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
            optionsBuilder.UseMySql(sqlData.connectionString);

            _repository = new RepositoryWrapper(new RepositoryContext(optionsBuilder.Options));
        }
    }
}
