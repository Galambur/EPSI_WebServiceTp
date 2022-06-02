using AgenceVoyage.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AgenceVoyage.Controllers
{
    [ApiController]
    [Route("/reservationSimple")]
    public class ReservationSimpleController : Controller
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public ReservationSimpleController()
        {
        }

        public IActionResult ReservationSimple()
        {
            return View();
        }

        [HttpPut("{idTrain}")]
        public IActionResult AddNewReservationSimple(int idTrain, [FromBody()] Reservation reservationSimpleToAdd)
        {
            try
            {
                reservationSimpleToAdd.Confirme = false;
                reservationSimpleToAdd.Annule = false;
                var reservationAdd = model.Reservation.Add(reservationSimpleToAdd);
                model.SaveChanges();

                var trainReservation = new Trainreservation(reservationAdd.Entity.IdReservation, idTrain);
                model.Trainreservation.Add(trainReservation);
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
