using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerPlatform.Domain.Entities
{
    public class SuspeitaDuplicidade
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdOriginal { get; set; }
        public virtual Customer CustomerOriginal { get; set; }

        public Guid IdSuspeito { get; set; }
        public virtual Customer CustomerSuspeito { get; set; }
        public double Score { get; set; }

        // O campo que você sugeriu:
        // Armazena algo como: {"campos": ["Nome", "Email"], "detalhes": "Nome 85% similar, Email idêntico"}
        public string DetalhesSimilaridade { get; set; }

        public DateTime DataDeteccao { get; set; } = DateTime.UtcNow;
    }
}
