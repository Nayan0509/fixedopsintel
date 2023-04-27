using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WoodenAutomative.Domain.Dtos.Request.Distributor;
using WoodenAutomative.Domain.Dtos.Response.Distributor;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class DistributorService : IDistributorService
    {
        #region fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly WoodenAutomativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region constructor
        public DistributorService(UserManager<ApplicationUser> userManager,
            WoodenAutomativeContext context, 
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        #endregion

        #region public method
        public async Task<DistributorListResponse> GetDistributorList(DistributorListRequest distributorListRequest)
        {
            #region declare field
            DistributorListResponse distributorListResponse;
            #endregion

            #region logic define
            if (distributorListRequest == null)
                throw new ArgumentNullException(nameof(distributorListRequest));

            var result = _context.DistributorAdmins.Join(_context.Users,
                                                            da => da.UserId,
                                                            u => u.Id,
                                                            (da, u) => new { da, u }
                                                    )
                                                   .Join(_context.Territories,
                                                            dau => dau.da.TerritoryId,
                                                            t => t.Id,
                                                            (dau, t) => new { dau, t }
                                                    )
                                                   .Join(_context.Distributors,
                                                            daut => daut.dau.da.DistributorId,
                                                            d => d.Id,
                                                            (daut, d) => new {
                                                                d.ProductDistributor,
                                                                d.DistributorName,
                                                                daut.t.TerritoryName,
                                                                daut.dau.u.FirstName,
                                                                daut.dau.da.Id,
                                                                distributorAdminActive = daut.dau.da.IsActive,
                                                                distributorAdminDeleted = daut.dau.da.IsDeleted,
                                                                userDeleted = daut.dau.u.IsDeleted,
                                                                userActive = daut.dau.u.IsActive,
                                                                distributorActive = d.IsActive,
                                                                distributorDelete = d.IsDeleted
                                                            }
                                                    )
                                                   .Where(all => all.distributorAdminDeleted == false && all.distributorAdminActive == true &&
                                                                 all.userActive == true && all.userDeleted == false &&
                                                                 all.distributorActive == true && all.distributorDelete == false)
                                                   .Select(all => all).ToList();

            #region SearchingConfigurationName
            var searchingScreen = distributorListRequest.Search.Value;
            if (!string.IsNullOrWhiteSpace(searchingScreen))
            {
                switch (searchingScreen.ToUpper())
                {
                    default:
                        result = result.Where(c => c.TerritoryName.Contains(searchingScreen) ||
                                                   c.DistributorName.Contains(searchingScreen) ||
                                                   c.ProductDistributor.Contains(searchingScreen) ||
                                                   c.FirstName.Contains(searchingScreen)).ToList();
                        break;
                }
            }
            #endregion

            #region SortingConfiguration
            if (distributorListRequest.Order != null)
            {
                var sortColumn = distributorListRequest.Columns[distributorListRequest.Order[0].Column].Name;
                var sortColumnDirection = distributorListRequest.Order[0].Dir.ToString();
                var sortOrder = "";

                if (!(string.IsNullOrWhiteSpace(sortColumn) && string.IsNullOrWhiteSpace(sortColumnDirection)))
                {
                    sortOrder = $"{sortColumn.ToUpper()}_{sortColumnDirection.ToUpper()}";
                }
                switch (sortOrder)
                {
                    case "FIRSTNAME_DESC":
                        result = result.OrderByDescending(c => c.FirstName).ToList();
                        break;
                    case "FIRSTNAME_ASC":
                        result = result.OrderBy(c => c.FirstName).ToList();
                        break;
                    case "DISTRIBUTORNAME_DESC":
                        result = result.OrderByDescending(c => c.DistributorName).ToList();
                        break;
                    case "DISTRIBUTORNAME_ASC":
                        result = result.OrderBy(c => c.DistributorName).ToList();
                        break;
                    case "PRODUCTDISTRIBUTOR_DESC":
                        result = result.OrderByDescending(c => c.ProductDistributor).ToList();
                        break;
                    case "PRODUCTDISTRIBUTOR_ASC":
                        result = result.OrderBy(c => c.ProductDistributor).ToList();
                        break;
                    case "TERRITORYNAME_DESC":
                        result = result.OrderByDescending(c => c.TerritoryName).ToList();
                        break;
                    case "TERRITORYNAME_ASC":
                        result = result.OrderBy(c => c.TerritoryName).ToList();
                        break;
                    default:
                        result = result.OrderByDescending(c => c.Id).ToList();
                        break;
                }
            }
            #endregion

            var totalRecords = result.Count();
            List<DistributorDetailsResponse> distributorDetailsResponses = result.Select(p => new DistributorDetailsResponse()
            {
                DistributorName = p.DistributorName,
                ProductDistributor = p.ProductDistributor,
                Username = p.FirstName,
                TerritoryName = p.TerritoryName,
                Id = p.Id
            }).ToList();
            #endregion

            #region response
            distributorListResponse = new DistributorListResponse()
            {
                DistributorDetails = distributorDetailsResponses,
                TotalRecords = totalRecords
            };
            return distributorListResponse;
            #endregion
        }

        public async Task<DistributorListResponse> AddDistributorData(DistributorRequest distributorRequest)
        {
            #region declare field
            DistributorListResponse distributorListResponse;
            Distributor distributor;

            #endregion

            #region logic define
            if (distributorRequest == null)
                throw new ArgumentNullException(nameof(distributorRequest));

            distributor = new Distributor()
            {
                DistributorName = distributorRequest.DistributorName,
                Address = distributorRequest.DistributorAddress,
                City = distributorRequest.DistributorCity,
                State= distributorRequest.DistributorState,
                Country = distributorRequest.DistributorCountry,
                ZipCode = distributorRequest.DistributorZipCode,
                ProductDistributor = distributorRequest.ProductDistributor,
                CreatedBy = distributorRequest.DistributorName,
                CreatedDate = DateTime.Now,
                ModifiedBy = distributorRequest.DistributorName,
                ModifiedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };
            var result = await _context.Distributors.AddAsync(distributor);
            var status = await _context.SaveChangesAsync(); 
            //List<DistributorDetailsResponse> distributorDetailsResponses = result.Select(p => new DistributorDetailsResponse()
            //{
            //    DistributorName = p.DistributorName,
            //    ProductDistributor = p.ProductDistributor,
            //    Username = p.FirstName,
            //    TerritoryName = p.TerritoryName,
            //    Id = p.Id
            //}).ToList();
            #endregion

            #region response
            distributorListResponse = new DistributorListResponse()
            {
                //DistributorDetails = distributorDetailsResponses,
                //TotalRecords = totalRecords
            };
            return distributorListResponse;
            #endregion
        }
        #endregion
    }
}
