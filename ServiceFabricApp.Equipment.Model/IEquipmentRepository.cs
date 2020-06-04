using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabricApp.EquipmentCatalog.Model
{
    public interface IEquipmentsRepository
    {
        Task<IEnumerable<Equipment>> GetAllEquipment();

        Task AddEquipment(Equipment equipment);
    }
}
