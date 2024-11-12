using System.ComponentModel.DataAnnotations;

namespace KoiPool_Project.Models
{
    public class HistoryModel
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Datetime { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}