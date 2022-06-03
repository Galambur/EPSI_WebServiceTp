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
        /// Ajoute une r�servation avec plusieurs trains.
        /// </summary>
        /// <param name="reservationMultiRequest">L'objet de r�servation et les trains voulus</param>
        /// <response code="201">La cr�ation s'est correctement d�roul�e</response>
        /// <response code="400">Un des �l�ments n'a pas �t� trouv� dans la base de donn�es, ou il y a moins de deux destinations</response> 
        /// <remarks>Chaque gare est s�lectionn� avec l'id de la ville.</remarks>
        [HttpPut]
        public IActionResult AjouterReservation([FromBody()] Request_ReservationMulti reservationMultiRequest)
        {
            try
            {
                // V�rification du nombre de destinations
                if (reservationMultiRequest.villes.Count < 2)
                    return BadRequest("Le nombre de ville doit �tre sup�rieur � 2.");

                // Binding des destinations avec les gares
                StringBuilder msgDestinations = new StringBuilder();
                msgDestinations.AppendLine("Gares s�lectionn� : ");

                #region Recherche des gares � atteindre selon les villes entr�es.
                // 500 : Renvoie la destinations introuvables � partir d'un des villes s�lectionn�.
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
                        msgDestinations.AppendLine(" - Gare n�" + destination.IdGare + ": " + item.NomVille);

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
                var reservationsTrain = new List<Trainreservation>();
                foreach (var train in correspondances)
                {
                    var trainReservModel = new Trainreservation();
                    trainReservModel.IdReservation = reservation.IdReservation;
                    trainReservModel.IdTrain = train.IdTrain;

                    Trainreservation trainReserv = model.Trainreservation.Add(trainReservModel).Entity;
                    model.SaveChanges();
                    reservationsTrain.Add(trainReserv);

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
