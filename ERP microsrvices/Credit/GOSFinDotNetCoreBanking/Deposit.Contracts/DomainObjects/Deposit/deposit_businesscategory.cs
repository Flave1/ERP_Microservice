namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_businesscategory : GeneralEntity
    {
        [Key]
        public int BusinessCategoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; } 
    }
}
