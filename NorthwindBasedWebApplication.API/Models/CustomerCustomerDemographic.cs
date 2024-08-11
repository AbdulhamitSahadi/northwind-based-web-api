using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindBasedWebApplication.API.Models
{
    public class CustomerCustomerDemographic : BaseEntity
    {
        //Navigation Properties
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }



        [ForeignKey("CustomerType")]
        public int CustomerTypeId { get; set; }
        public CustomerDemographic CustomerType { get; set; }
    }
}
