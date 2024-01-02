using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelotransApp.Model
{
    internal class DossierModel
    {
        public int IdDossier { get; set; }
        public int IdUser { get; set; }
        public string NomDossier { get; set; }
        public string CommentaireDossier { get; set; }
        public string DestinataireDossier { get; set; }
        public string EmetteurDossier { get; set; }
        public int StatutDossier { get; set; }
        public int EnableDossier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
