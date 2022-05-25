using System;
using System.Collections.Generic;

namespace AgenceVoyage.Models
{
    public partial class Trainreservation
    {
        public int IdReservation { get; set; }
        public int IdTrain { get; set; }

        public Trainreservation()
        {

        }

        public Trainreservation(int idReservation, int idTrain)
        {
            IdReservation = idReservation;
            IdTrain = idTrain;
        }
    }
}
