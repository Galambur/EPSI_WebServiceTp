using AgenceVoyage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AgenceVoyage.Controllers
{
    [ApiController]
    [Route("/reservation")]
    public class ReservationController : Controller
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reservation = model.Reservation.Single(r => r.IdReservation == id);

            if (reservation == null)
                return BadRequest("Reservation non trouvée");

            return Ok(reservation);
        }
    }
}