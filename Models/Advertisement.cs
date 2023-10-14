using System.ComponentModel.DataAnnotations;

namespace BharatMirror.Models
{
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public String? image_url { get; set; }

        [Required]
        public String? business_url { get; set; }

        [Required]
        public String? title { get; set; }

        [Required]
        public String? description { get; set; }

        [Required]
        public String? category { get; set; }

        [Required]
        public String? status { get; set; }

        [Required]
        public String? reason { get; set; }


        [Required]
        public DateTime start_time { get; set; }

        [Required]
        public DateTime end_time { get; set; }

        [Required]
        public int ctr { get; set; }

        [Required]
        public int views { get; set; }

        [Required]
        public String? payment { get; set; }


    }
    }
