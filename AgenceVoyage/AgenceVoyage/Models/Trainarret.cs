using System;
using System.Collections.Generic;

namespace AgenceVoyage.Models
{
    public partial class Trainarret
    {
        public int IdTrain { get; set; }
        public int IdGare { get; set; }
        public DateTime? DateDepart { get; set; }
        public DateTime? DateArret { get; set; }
        public int? Status { get; set; }
    }
}
