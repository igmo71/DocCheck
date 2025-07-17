using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class DocumentStatus
    {
        public Guid Id { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public required string Name { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public required string Description { get; set; }


        public bool IsActive { get; set; }
    }
}
