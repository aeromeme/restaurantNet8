using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public abstract class DomainModel
    {
        public virtual int GetId()
        {
            var property = this.GetType().GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && p.PropertyType == typeof(int));
            if (property != null)
            {
                return (int)property.GetValue(this)!;
            }
            throw new InvalidOperationException("No Id property found.");
        }
    }
}
