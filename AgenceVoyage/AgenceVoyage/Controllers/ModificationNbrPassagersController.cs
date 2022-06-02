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
    public class ModificationNbrPassagersController : Controller
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public ModificationNbrPassagersController()
        {
        }

        public IActionResult ModificationNbrPassagers()
        {
            return View();
        }

        [HttpPost("{idReservation}")]
        public IActionResult ModifyNbrPassagers(int idReservation, [FromBody()] int nbrPassagers)
        {
            try
            {
                var reservation = model.Reservation.SingleOrDefault(r => r.IdReservation == idReservation);
                if (reservation == null || reservation == default(Reservation))
                    return BadRequest("Identifiant de réservation introuvable.");
                reservation.NbrPassager = nbrPassagers;
                model.SaveChanges();
                return Ok();
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}
