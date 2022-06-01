using AgenceVoyage.Models;
using AgenceVoyage.ModelsRequest;
using AgenceVoyage.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgenceVoyage.Controllers
{
    [ApiController]
    [Route("/reservationMultiDestination")]
    public class reservationMultiDestination : Controller
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public reservationMultiDestination()
        {
        }

        public IActionResult ReservationSimple()
        {
            return View();
        }

        
        [HttpPost("{listTrain}")]
        public IActionResult ModifyNbrPassagers([FromBody()] ReservationMultiRequest reservationMultiRequest)
        {
            try
            {
                if(reservationMultiRequest.villes.Count < 2)
                    return BadRequest("Deux destinations minimum.");

                var destinations = new List<Gare>();
                StringBuilder msgDestinations = new StringBuilder();
                msgDestinations.AppendLine("Gares s�lectionn� : ");
                foreach (Ville item in reservationMultiRequest.villes)
                {
                    var Gares =
                        from v in model.Ville
                        join gdv in model.Garedessertville on v.IdVille equals gdv.IdVille
                        join g in model.Gare on gdv.IdGare equals g.IdGare
                        where
                            v.NomVille == item.NomVille
                        select g;
                    var destination = Gares.SingleOrDefault();

                    if (destination == null || destination == default(Gare))
                    {
                        msgDestinations.AppendLine(" - Gare introuvable : " + item.NomVille);
                        return BadRequest(msgDestinations.ToString());
                    }
                    else
                        msgDestinations.AppendLine(" - Gare n�" + destination.IdGare + ": "+ item.NomVille);

                    destinations.Add(destination);
                }


                var correspondances = new List<Train>();
                for (int i = 1; i < destinations.Count; i++)
                {
                    var dGare = destinations[i-1];
                    var aGare = destinations[i];
                    var train = ToolsForTrain.TrouverTrainEntreDeuxDestintation(model, dGare, aGare);
                    if (train == null)
                        return BadRequest(msgDestinations.ToString() + "Aucune correspondance trouv�e entre " + dGare.NomGare + " et " + aGare.NomGare);

                    if(!correspondances.Contains(train))
                        correspondances.Add(train);
                }
                

                // La r�servation des trains
                var reservationModel = new Reservation();
                reservationModel.NbrPassager = reservationMultiRequest.nbrPassager;
                reservationModel.IdClient = reservationMultiRequest.idClient;
                reservationModel.Confirme = false;


                Reservation reservation = model.Reservation.Add(reservationModel).Entity;
                model.SaveChanges();

                StringBuilder msgReservationTrain = new StringBuilder();
                msgReservationTrain.AppendLine("R�servation de train ajout� : ");
                foreach (var train in correspondances)
                {
                    var trainReservModel = new Trainreservation();
                    trainReservModel.IdReservation = reservation.IdReservation;
                    trainReservModel.IdTrain = train.IdTrain;
                    
                    Trainreservation trainReserv = model.Trainreservation.Add(trainReservModel).Entity;
                    msgReservationTrain.AppendLine(" - R�servation du train n�" + train.IdTrain + " ajout� � la r�servation n�" + trainReserv.IdReservation);
                }

                model.SaveChanges();


                StringBuilder msgResultat = new StringBuilder();
                msgResultat.AppendLine(msgDestinations.ToString());
                msgResultat.AppendLine(msgReservationTrain.ToString());
                msgResultat.AppendLine("R�sulat de la r�servation");
                msgResultat.AppendLine(" - " + destinations.Count() + " destinations");
                msgResultat.AppendLine(" - " + correspondances.Count() + " correspondance");
                msgResultat.AppendLine(" - " + reservationMultiRequest.nbrPassager + " passager(s) ajout� � la r�servation");
                msgResultat.AppendLine(" - Reservation ajout� au client n�" + reservationMultiRequest.idClient);



                return Created("/reservationMultiDestination/",  msgResultat.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }



}
