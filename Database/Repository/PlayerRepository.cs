using Database.Contracts;
using Database.Entities;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Repository
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreatePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public Player GetPlayerById(Guid player_Id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
