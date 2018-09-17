using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Insert or update an item to database.
        /// </summary>
        /// <typeparam name="T">an Entity type</typeparam>
        /// <param name="context">current db context</param>
        /// <param name="obj">the object to insert or update</param>
        /// <returns></returns>
        public static EntityEntry<T> Save<T>(this DbSet<T> context, T obj) where T : BaseEntity
        {
            if (obj.ID > 0) { return context.Update(obj); }
            else { return context.Add(obj); }
        }

        //public static T Save<T>(this T obj) where T : BaseEntity
        //{
        //    if (obj.ID > 0)
        //}
    }
}
