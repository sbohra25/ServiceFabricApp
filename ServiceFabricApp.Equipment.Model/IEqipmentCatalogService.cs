using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabricApp.EquipmentCatalog.Model
{
    public interface IEqipmentCatalogService : IService
    {       
      Task<Equipment[]> GetAllEquipmentssAsync();

      Task AddEquipmentAsync(Equipment equipment);
    }
}
