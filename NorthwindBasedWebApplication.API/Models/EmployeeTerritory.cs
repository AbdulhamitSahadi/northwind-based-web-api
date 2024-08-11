using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindBasedWebApplication.API.Models
{
    public class EmployeeTerritory : BaseEntity
    {
        //Navigation Properties
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }



        [ForeignKey("Territory")]
        public int TerritoryId { get; set; }
        public Territory Territory { get; set; }
    }
}
