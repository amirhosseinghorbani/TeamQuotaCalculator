using System;
using System.ComponentModel.DataAnnotations;

namespace TeamQuotaCalculator.Models
{
    public class Manager
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(16, MinimumLength=4)]
        public string Name { get; set; }
        public double Ceo { get; set; }
        public double Dev { get; set; }
        public double Biz { get; set; }
        public double Ratio { get; set; } = 1;
        public double Share {get; set;}

        #region Q
        public bool q1 { get; set; }
        public bool q2 { get; set; }
        public bool q3 { get; set; }
        public bool q4 { get; set; }
        public bool q5 { get; set; }
        public bool q6 { get; set; }
        public bool q7 { get; set; }
        public bool q8 { get; set; }
        public bool q9 { get; set; }
        public bool q10 { get; set; }
        public bool q11 { get; set; }
        public bool q12 { get; set; }
        public bool q13 { get; set; }
        public bool q14 { get; set; }
        public bool q15 { get; set; }
        #endregion
    }
}