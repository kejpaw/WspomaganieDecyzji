using TransportApp.Models;
using System;
using System.Linq;

namespace TransportApp.DbCore
{
    public static class DbInitializer
    {
        public static void Initialize(RepoContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}