﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDemo.Models
{
   public class PagedResult<T> : PagedResultBase where T : class
{
    public IList<T> Results { get; set; }
 
    public PagedResult()
    {
        Results = new List<T>();
    }
}
}
