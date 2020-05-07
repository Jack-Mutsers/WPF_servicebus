using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPlayerRepository
    {
        Player GetPlayerById(Guid player_Id);
        //Player GetUserWithDetails(string username, string password);
        void CreatePlayer(Player player);
        void UpdatePlayer(Player player);
        void DeletePlayer(Player player);
    }
}
