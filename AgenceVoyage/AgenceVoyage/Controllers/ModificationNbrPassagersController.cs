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

        [HttpPost]
        [Route("/modificationReservation/nbrPassagers")]
        public IActionResult ModifyNbrPassagers([FromBody()] Reservation reservationToModify)
        {
            var reservation = model.Reservation.Single(r => r.IdReservation == reservationToModify.IdReservation);
            try
            {
                reservation.NbrPassager = reservationToModify.NbrPassager;
                model.SaveChanges();
                return Ok();
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}
