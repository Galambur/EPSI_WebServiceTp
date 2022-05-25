using System;
using System.Collections.Generic;

namespace AgenceVoyage.Models
{
    public partial class Client
    {
        public int IdClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
    }
}
