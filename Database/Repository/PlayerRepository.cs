using Database.Contracts;
using Database.Entities;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Repository
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreatePlayer(Player player)
        {
            Create(player);
        }

        public void DeletePlayer(Player player)
        {
            Update(player);
        }

        public Player GetPlayerById(Guid player_Id)
        {
            return FindByCondition(player => player.userId.Equals(player_Id)).FirstOrDefault();
        }

        public void UpdatePlayer(Player player)
        {
            Update(player);
        }
    }
}
