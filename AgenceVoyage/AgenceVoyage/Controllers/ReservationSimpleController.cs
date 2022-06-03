using AgenceVoyage.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        /// <response code="201">La réservation a été correctement créée</response>  
        /// <response code="400">L'identification du train ou du salarié est incorrect</response>  
        /// <response code="500">Une erreur s'est produite pendant le traitement</response>  
        /// <remarks>
        ///     Pour l'objet Reservation, il suffit de remplir idClient et nbrPassagers, le reste se fait tout seul
        /// </remarks>
        [HttpPut("{idTrain}")]
        public IActionResult AddNewReservationSimple(int idTrain, [FromBody()] Reservation reservationSimpleToAdd)
        {
            var trainDb = model.Train.SingleOrDefault(t => t.IdTrain == idTrain);
            if(trainDb == null)
                return BadRequest("Le train choisi est introuvable");

            var utilisateurDb = model.Client.SingleOrDefault(u => u.IdClient == reservationSimpleToAdd.IdClient);
            if (utilisateurDb == null)
                return BadRequest("Le client choisi n'existe pas");

            try
            {
                reservationSimpleToAdd.Confirme = false;
                reservationSimpleToAdd.Annule = false;
                var reservationAdd = model.Reservation.Add(reservationSimpleToAdd);
                model.SaveChanges();

                var trainReservation = new Trainreservation(reservationAdd.Entity.IdReservation, idTrain);
                model.Trainreservation.Add(trainReservation);
                model.SaveChanges();

                return Created("", reservationSimpleToAdd);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
