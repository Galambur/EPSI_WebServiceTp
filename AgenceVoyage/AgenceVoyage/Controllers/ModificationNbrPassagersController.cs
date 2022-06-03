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
        /// Modifier le nombre de passager d'une r�servation
        /// </summary>
        /// <param name="idReservation">L'identifiant de la r�servation � modifier</param>
        /// <param name="nbrPassagers">Le nouveau nommbre de passagers</param>
        /// <returns>Un code 200 ou une erreur</returns>
        [HttpPost("{idReservation}")]
        public IActionResult ModifyNbrPassagers(int idReservation, [FromBody()] int nbrPassagers)
        {
            try
            {
                var reservation = model.Reservation.SingleOrDefault(r => r.IdReservation == idReservation);
                if (reservation == null || reservation == default(Reservation))
                    return BadRequest("Identifiant de r�servation introuvable.");
                reservation.NbrPassager = nbrPassagers;
                model.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
