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
            var reservation = model.Reservation.Single(r => r.IdReservation == idReservation);
            try
            {
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
