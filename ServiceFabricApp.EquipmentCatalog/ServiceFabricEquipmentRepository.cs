using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricApp.EquipmentCatalog.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabricApp.EquipmentCatalog
{
    class ServiceFabricEquipmentRepository : IEquipmentsRepository
    {
        // Readonly dictoinary directly maaped to service fabric storage 
        private readonly IReliableStateManager _stateManager;

        //Built in service fabric storage 
        public ServiceFabricEquipmentRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddEquipment(Equipment equipment)
        {
            // GetorAdd will return empty referene is it's not exist
            IReliableDictionary<Guid, Equipment> equipments = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Equipment>>("equipements");

            // We can use inbuilt trasaction provided by service fabric, it will only commit if things go well
            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await equipments
                    .AddOrUpdateAsync(tx, equipment.Id, equipment, (id, value) => equipment);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipment()
        {
            // GetorAdd will return empty referene is it's not exist
            IReliableDictionary<Guid, Equipment> equipements = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Equipment>>("equipements");

            var result = new List<Equipment>();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                // Service fabric store all data in replica
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Equipment>> allEquipments = await equipements.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Equipment>> enumerator =
                   allEquipments.GetAsyncEnumerator())
                {
                    // Get all records
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Equipment> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }
            return result;
        }
    }
}
