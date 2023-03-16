using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KADES.Helper
{
    public class PaginationHelper<T> : List<T>
    {
        
        public PaginationHelper(List<T> items)
        {
           
            this.AddRange(items);
        }


        public static PaginationHelper<T> Create(IQueryable<T> source)
        {
            var items = source.ToList();

            return new PaginationHelper<T>(items);
        }
    }
}