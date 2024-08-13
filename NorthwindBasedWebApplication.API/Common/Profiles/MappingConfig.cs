using AutoMapper;
using NorthwindBasedWebApplication.API.Models.DTOs.CategoryDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.EmployeeDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.RegionDTO;
using NorthwindBasedWebApplication.API.Models.DTOs.RegionDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.ShipperDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.UserDTOs;
using NorthwindBasedWebApplication.API.Models;

namespace NorthwindBasedWebApplication.API.Common.Profiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, ReadCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();


            CreateMap<Customer, ReadCustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();


            CreateMap<CustomerDemographic, ReadCustomerDemographicDto>().ReverseMap();
            CreateMap<CustomerDemographic, CreateCustomerDemographicDto>().ReverseMap();
            CreateMap<CustomerDemographic, UpdateCustomerDemographicDto>().ReverseMap();


            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();
            CreateMap<Employee, ReadEmployeeDto>().ReverseMap();


            CreateMap<Order, ReadOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();



            CreateMap<Product, ReadProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();



            CreateMap<Region, ReadRegionDto>().ReverseMap();
            CreateMap<Region, CreateRegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();



            CreateMap<Shipper, ReadShipperDto>().ReverseMap();
            CreateMap<Shipper, CreateShipperDto>().ReverseMap();
            CreateMap<Shipper, UpdateShipperDto>().ReverseMap();



            CreateMap<Supplier, ReadSupplierDto>().ReverseMap();
            CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
            CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();



            CreateMap<Territory, ReadTerritoryDto>().ReverseMap();
            CreateMap<Territory, UpdateTerritoryDto>().ReverseMap();
            CreateMap<Territory, CreateTerritoryDto>().ReverseMap();


            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
