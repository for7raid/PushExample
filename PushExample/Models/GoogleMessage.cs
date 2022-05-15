using System.ComponentModel.DataAnnotations;

namespace PushExample.Models
{
    public class GoogleMessage : MessageBase
    {
        [Required, StringLength(50, MinimumLength = 50)]
        public string DeviceToken { get; set; }
       
        [Required, StringLength(2000)]
        public string Message { get; set; }
        [Required, StringLength(255)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Condition { get; set; }

        public override string ToString()
        {
            return $"{DeviceToken}: {Title}, {Message}, {Condition} at {CreatedAt:O}";
        }
    }
}
