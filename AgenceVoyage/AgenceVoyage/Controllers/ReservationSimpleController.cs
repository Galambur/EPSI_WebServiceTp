using AgenceVoyage.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AgenceVoyage.Controllers
{
    [ApiController]
    [Route("/reservationSimple")]
    public class ReservationSimpleController : ControllerBase
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public ReservationSimpleController()
        {
        }

        [HttpPut("{idTrain}")]
        public IActionResult ModifyNbrPassagers(int idTrain, [FromBody()] Reservation reservationSimpleToAdd)
        {
            try
            {
                var trainReservation = new Trainreservation(reservationSimpleToAdd.IdReservation, idTrain);
                model.Trainreservation.Add(trainReservation);

                model.Reservation.Add(reservationSimpleToAdd);

                model.SaveChanges();
                return Created("/reservationSimple/" + idTrain, reservationSimpleToAdd);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
