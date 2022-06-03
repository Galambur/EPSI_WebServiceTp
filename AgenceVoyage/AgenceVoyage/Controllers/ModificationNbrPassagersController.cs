using AgenceVoyage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AgenceVoyage.Controllers
{
    [ApiController]
    [Route("/modificationReservation")]
    public class ModificationNbrPassagersController : ControllerBase
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public ModificationNbrPassagersController()
        {
        }

        /// <summary>
        /// Modifier le nombre de passager d'une réservation
        /// </summary>
        /// <param name="idReservation">L'identifiant de la réservation à modifier</param>
        /// <param name="nbrPassagers">Le nouveau nombre de passagers</param>
        /// <response code="200">La modification s'est faite</response>
        /// <response code="400">L'identifiant de la reservation est introuvable</response>  
        /// <response code="500">Une erreur s'est produite pendant le traitement</response>  
        [HttpPost("{idReservation}")]
        public IActionResult ModifyNbrPassagers(int idReservation, [FromBody()] int nbrPassagers)
        {
            if(nbrPassagers <= 0)
                return BadRequest("Vous devez fournir un nombre de passagers supérieur à 0 dans le body");

            try
            {
                var reservation = model.Reservation.SingleOrDefault(r => r.IdReservation == idReservation);
                if (reservation == null || reservation == default(Reservation))
                    return BadRequest("La réservation est introuvable");
                reservation.NbrPassager = nbrPassagers;
                model.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
