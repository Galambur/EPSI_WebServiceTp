using System;
using System.Collections.Generic;

namespace AgenceVoyage.Models
{
    public partial class Train
    {
        public int IdTrain { get; set; }
        public DateTime? DateDepart { get; set; }
        public DateTime? DateArrive { get; set; }
    }
}
