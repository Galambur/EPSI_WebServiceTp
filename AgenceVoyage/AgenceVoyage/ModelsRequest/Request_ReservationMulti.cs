using AgenceVoyage.Models;
using System.Collections.Generic;

namespace AgenceVoyage.ModelsRequest
{

    public class Request_ReservationMulti
    {
        public int idClient { get; set; }

        public int nbrPassager { get; set; }

        public List<Ville> villes { get; set; } = new List<Ville>();

    }

    public class Response_ReservationMultiResponse
    {
        public Reservation Reservation { get; set; }

        public List<Train> CorrespondancesTrain { get; set; }

        public List<Trainreservation> Trainreservation { get; set; }

        public List<Gare> GareArret { get; set; }

        public string Recapitulatif;
    }



}
