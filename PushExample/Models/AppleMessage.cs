using System.ComponentModel.DataAnnotations;

namespace PushExample.Models
{
    public class AppleMessage : MessageBase
    {
        [Required, StringLength(50, MinimumLength = 50)]
        public string PushToken { get; set; }

        [Required, StringLength(2000)]
        public string Alert { get; set; }
        public int Priority { get; set; } = 10;
        public bool IsBackground { get; set; } = true;

        public override string ToString()
        {
            return $"{PushToken}: {Alert}, Priority: {Priority}, IsBackground: {IsBackground} at {CreatedAt:O}";
        }
    }
}
