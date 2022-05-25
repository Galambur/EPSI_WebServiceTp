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
    [Route("/reservation")]
    public class ReservationController : ControllerBase
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public ReservationController()
        {
        }

        [HttpGet]
        public List<Reservation> Get()
        {
            var reservations = model.Reservation.ToList();
            return reservations;
        }

        [HttpPost]
        [Route("/reservation/nbrPassagers")]
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
