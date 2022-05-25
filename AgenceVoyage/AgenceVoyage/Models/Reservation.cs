using System;
using System.Collections.Generic;

namespace AgenceVoyage.Models
{
    public partial class Reservation
    {
        public int IdReservation { get; set; }
        public bool? Confirme { get; set; }
        public bool? Annule { get; set; }
        public int? NbrPassager { get; set; }
        public int IdClient { get; set; }
    }
}
