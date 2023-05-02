using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Distributor;
using WoodenAutomative.Domain.Dtos.Response.Distributor;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IDistributorService
    {
        /// <summary>
        /// Get List of Distributor data
        /// </summary>
        /// <param name="distributorListRequest">Request param for users</param>
        public Task<DistributorListResponse> GetDistributorList(DistributorListRequest distributorListRequest); 
        
        /// <summary>
        /// Get List of Distributor data
        /// </summary>
        /// <param name="id">Request param for users</param>
        public Task<DistributorRequest> GetSingleDistributor(int id);

        /// <summary>
        /// Add Distributor data
        /// </summary>
        /// <param name="distributorRequest">Request param for users</param>
        public Task<bool> AddDistributorData(DistributorRequest distributorRequest);

        /// <summary>
        /// Add users data
        /// </summary>
        /// <param name="distributorRequest">Request param for users</param>
        public Task<bool> AddUsersData(UserRequest userRequest);

        /// <summary>
        /// Get all territory data
        /// </summary>
        public Task<List<TerritoryResponse>> GetAllTerritory();
            }
}
