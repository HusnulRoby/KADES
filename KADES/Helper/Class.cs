using System;

namespace KADES.Helper
{
    public class Class<T> : List<T>
    {
        public Class( List<T> items) { 
            this.AddRange(items);
        }

        public static Class<T> Create(IQueryable<T> source) {
            var items = source.ToList();
            return new Class<T>(items);
        }
    }
}
