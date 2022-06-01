using AgenceVoyage.Models;
using System.Collections.Generic;

namespace AgenceVoyage.ModelsRequest
{

    public class ReservationMultiRequest
    {
        public int idClient { get; set; }

        public int nbrPassager { get; set; }

        public List<Ville> villes { get; set; } = new List<Ville>();

    }
}
