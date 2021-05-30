using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Models
{
    public static class EntityHelper
    {
        /// <summary>
        /// Makes a shallow copy of an entity object. This works much like a MemberwiseClone()
        /// but directly instantiates a new object and copies only properties that work with
        /// EF and don't have the NotMappedAttribute.
        ///
        /// ** It also avoids copying the EF's proxy reference that would occur by using MemberwiseClone() **
        ///
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="source">The source entity.</param>
        public static TEntity ShallowCopy<TEntity>(TEntity source) where TEntity : class, new()
        {
            // get the properties from the entity that are read/write and not marked with he NotMappedAttribute
            var notMappedType = typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute);
            var sourceProperties = typeof(TEntity)
                                    .GetProperties()
                                    .Where(p => p.CanRead && p.CanWrite && (p.GetCustomAttributes(notMappedType, true).Length == 0));

            // copy the properties into a new entity
            var newObj = new TEntity();
            foreach (var property in sourceProperties)
            {
                property.SetValue(newObj, property.GetValue(source, null), null);
            }

            return newObj;
        }

    }

    public abstract partial class BaseEntity
    {
        public T ShallowCopy<T>() where T : BaseEntity
        {
            return this.MemberwiseClone() as T;
        }
        // other BaseEntity properties not relevent here
    }
}
