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

        /// <summary>
        /// Ajoute une réservation simple
        /// </summary>
        /// <param name="idTrain">L'id du train à prendre</param>
        /// <param name="reservationSimpleToAdd">Les informations de la réservation</param>
        /// <returns>Un code 201 ou une erreur</returns>
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
