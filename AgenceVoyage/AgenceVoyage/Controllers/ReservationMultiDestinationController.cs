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
    public class reservationMultiDestination : ControllerBase
    {

        public tpagencevoyageContext model = new tpagencevoyageContext();

        public reservationMultiDestination()
        {
        }

        [HttpPut("AjouterReservation")]
        public IActionResult AjouterReservation([FromBody()] ReservationMultiRequest reservationMultiRequest)
        {
            try
            {
                // V�rification du nombre de destinations
                if (reservationMultiRequest.villes.Count < 2)
                    return BadRequest("Deux destinations minimum.");

                // Binding des destinations avec les gares
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
                        msgDestinations.AppendLine(" - Gare n�" + destination.IdGare + ": " + item.NomVille);

                    destinations.Add(destination);
                }

                // Recherche des correspondances avec les gares
                var correspondances = new List<Train>();
                for (int i = 1; i < destinations.Count; i++)
                {
                    var dGare = destinations[i - 1];
                    var aGare = destinations[i];
                    var train = ToolsForTrain.TrouverTrainEntreDeuxDestintation(model, dGare, aGare);
                    if (train == null)
                        return BadRequest(msgDestinations.ToString() + "Aucune correspondance trouv�e entre " + dGare.NomGare + " et " + aGare.NomGare);

                    if (!correspondances.Contains(train))
                        correspondances.Add(train);
                }


                // Cr�ation de la r�servation
                var reservationModel = new Reservation();
                reservationModel.NbrPassager = reservationMultiRequest.nbrPassager;
                reservationModel.IdClient = reservationMultiRequest.idClient;
                reservationModel.Confirme = false;


                Reservation reservation = model.Reservation.Add(reservationModel).Entity;
                model.SaveChanges();

                // Ajout des r�servations aux correspondances
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

                // Message r�capitulatif
                StringBuilder msgResultat = new StringBuilder();
                msgResultat.AppendLine(msgDestinations.ToString());
                msgResultat.AppendLine(msgReservationTrain.ToString());
                msgResultat.AppendLine("R�sulat de la r�servation");
                msgResultat.AppendLine(" - " + destinations.Count() + " destinations");
                msgResultat.AppendLine(" - " + correspondances.Count() + " correspondance");
                msgResultat.AppendLine(" - " + reservationMultiRequest.nbrPassager + " passager(s) ajout� � la r�servation");
                msgResultat.AppendLine(" - Reservation ajout� au client n�" + reservationMultiRequest.idClient);


                return Created("/reservationMultiDestination/", msgResultat.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
