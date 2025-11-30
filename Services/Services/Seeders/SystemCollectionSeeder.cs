using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Services.Interfaces;

namespace Services.Services.Seeders
{
    public class SystemCollectionSeeder(IUserRepository userRepository, ICollectionService collectionService) : ISeeder
    {
        public async Task SeedAsync()
        {
            var users = await userRepository.GetAllUsersAsync();

            foreach (var user in users)
            {
                var result = await collectionService.AddSystemCollectionsAsync(user.Id);
            }


        }
    }
}
