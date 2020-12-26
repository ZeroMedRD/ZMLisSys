using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ZMLISSys.Models
{
    public partial class LIS_HISEntities : DbContext
    {
        public LIS_HISEntities(string nameOrConnectionString)
           : base(nameOrConnectionString)
        {
        }
    }
}