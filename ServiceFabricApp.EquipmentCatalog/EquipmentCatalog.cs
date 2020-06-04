using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabricApp.EquipmentCatalog.Model;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabricApp.EquipmentCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class EquipmentCatalog : StatefulService, IEqipmentCatalogService
    {
        private IEquipmentsRepository _repo;
        public EquipmentCatalog(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
            //return new[]
            //{
            //    new ServiceReplicaListener(context =>
            //       new FabricTransportServiceRemotingListener(context, this))
            // };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            //var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            // Initilize base class  constructor 
            _repo = new ServiceFabricEquipmentRepository(this.StateManager);

            var equipment1 = new Equipment
            {
                Id = Guid.NewGuid(),
                Name = "Water Sensor",
                Description = "Water Sensor",
                Price = 500,
                Availability = 100
            };

            var equipment2 = new Equipment
            {
                Id = Guid.NewGuid(),
                Name = "Router",
                Description = "Router",
                Price = 400,
                Availability = 50
            };

            // Adding data to service fabric reliable statemanager database
            await _repo.AddEquipment(equipment1);
            await _repo.AddEquipment(equipment2);
            
            // This is to validate replica concept provided by service fabric
            IEnumerable<Equipment> all = await _repo.GetAllEquipment();

        }

        public async Task AddEquipmentAsync(Equipment equipment)
        {
            await _repo.AddEquipment(equipment);
        }

        public async Task<Equipment[]> GetAllEquipmentssAsync()
        {
            // Interface not not be serilized and Service remorting accepts array on network
            return (await _repo.GetAllEquipment()).ToArray();
        }

    }
}
