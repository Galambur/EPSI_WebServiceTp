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
    public class DestinationDirecteController : ControllerBase
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public DestinationDirecteController()
        {
        }

        [HttpGet]
        public List<Reservation> Get()
        {
            var reservations = model.Reservation.ToList();
            return reservations;
        }

        [HttpPost]
        [Route("/reservation/{idTrain}")]
        public IActionResult Post([FromQuery()] int idTrain, [FromBody()] Reservation reservationToAdd)
        {
            var trainReservation = new Trainreservation();
            model.Reservation.Add(reservationToAdd);
            try
            {
                trainReservation.IdReservation = reservationToAdd.IdReservation;
                trainReservation.IdTrain = idTrain;
                model.Trainreservation.Add(trainReservation);
                model.SaveChanges();
                return Created("/reservation", reservationToAdd);
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}
