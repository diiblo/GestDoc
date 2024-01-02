using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelotransApp.Model
{
    internal class DocumentModel
    {
        public int IdDossier { get; set; }
        public int IdDocument { get; set; }
        public string NomDocument { get; set; }
        public string FichierDocument { get; set; }
        public int EnableDocument { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
