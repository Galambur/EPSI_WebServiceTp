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

        /// <summary>
        /// Ajoute une réservation avec plusieurs trains.
        /// </summary>
        /// <param name="reservationMultiRequest">L'objet de réservation et les trains voulus</param>
        /// <response code="201">La création s'est correctement déroulée</response>
        /// <response code="400">Un des éléments n'a pas été trouvé dans la base de données, ou il y a moins de deux destinations</response> 
        /// <remarks>Chaque gare est sélectionné avec l'id de la ville.</remarks>
        [HttpPut]
        public IActionResult AjouterReservation([FromBody()] Request_ReservationMulti reservationMultiRequest)
        {
            try
            {
                // Vérification du nombre de destinations
                if (reservationMultiRequest.villes.Count < 2)
                    return BadRequest("Le nombre de ville doit être supérieur à 2.");

                // Binding des destinations avec les gares
                StringBuilder msgDestinations = new StringBuilder();
                msgDestinations.AppendLine("Gares sélectionné : ");

                #region Recherche des gares à atteindre selon les villes entrées.
                // 500 : Renvoie la destinations introuvables à partir d'un des villes sélectionné.
                var destinations = new List<Gare>();
                foreach (Ville item in reservationMultiRequest.villes)
                {
                    var Gares =
                        from v in model.Ville
                        join gdv in model.Garedessertville on v.IdVille equals gdv.IdVille
                        join g in model.Gare on gdv.IdGare equals g.IdGare
                        where
                            v.IdVille == item.IdVille
                        select g;
                    var destination = Gares.SingleOrDefault();

                    if (destination == null || destination == default(Gare))
                    {
                        msgDestinations.AppendLine(" - Gare introuvable : " + item.NomVille);
                        return BadRequest(msgDestinations.ToString());
                    }
                    else
                        msgDestinations.AppendLine(" - Gare n°" + destination.IdGare + ": " + item.NomVille);

                    destinations.Add(destination);
                }
                #endregion

                // Recherche des correspondances avec les gares
                var correspondances = new List<Train>();
                for (int i = 1; i < destinations.Count; i++)
                {
                    var dGare = destinations[i - 1];
                    var aGare = destinations[i];
                    var train = ToolsForTrain.TrouverTrainEntreDeuxDestintation(model, dGare, aGare);
                    if (train == null)
                        return BadRequest(msgDestinations.ToString() + "Aucune correspondance trouvée entre " + dGare.NomGare + " et " + aGare.NomGare);

                    if (!correspondances.Contains(train))
                        correspondances.Add(train);
                }


                // Création de la réservation
                var reservationModel = new Reservation();
                reservationModel.NbrPassager = reservationMultiRequest.nbrPassager;
                reservationModel.IdClient = reservationMultiRequest.idClient;
                reservationModel.Confirme = false;

                Reservation reservation = model.Reservation.Add(reservationModel).Entity;
                model.SaveChanges();

                // Ajout des réservations aux correspondances
                StringBuilder msgReservationTrain = new StringBuilder();
                msgReservationTrain.AppendLine("Réservation de train ajouté : ");
                var reservationsTrain = new List<Trainreservation>();
                foreach (var train in correspondances)
                {
                    var trainReservModel = new Trainreservation();
                    trainReservModel.IdReservation = reservation.IdReservation;
                    trainReservModel.IdTrain = train.IdTrain;

                    Trainreservation trainReserv = model.Trainreservation.Add(trainReservModel).Entity;
                    model.SaveChanges();
                    reservationsTrain.Add(trainReserv);

                    msgReservationTrain.AppendLine(" - Réservation du train n°" + train.IdTrain + " ajouté à la réservation n°" + trainReserv.IdReservation);
                }

                model.SaveChanges();

                // Message récapitulatif
                StringBuilder msgResultat = new StringBuilder();
                msgResultat.AppendLine(msgDestinations.ToString());
                msgResultat.AppendLine(msgReservationTrain.ToString());
                msgResultat.AppendLine("Résulat de la réservation");
                msgResultat.AppendLine(" - " + destinations.Count() + " destinations");
                msgResultat.AppendLine(" - " + correspondances.Count() + " correspondance");
                msgResultat.AppendLine(" - " + reservationMultiRequest.nbrPassager + " passager(s) ajouté à la réservation");
                msgResultat.AppendLine(" - Reservation ajouté au client n°" + reservationMultiRequest.idClient);


                Response_ReservationMultiResponse response = new Response_ReservationMultiResponse();
                response.Trainreservation = reservationsTrain;
                response.CorrespondancesTrain = correspondances;
                response.Reservation = reservation;
                response.Recapitulatif = msgResultat.ToString();
                response.GareArret = destinations;

                return Created("/reservationMultiDestination/", response);
            }
            catch (Exception e)
            {
                //throw e;
                return BadRequest(e.Message);
            }
        }
    }
}
