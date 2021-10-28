using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI.Models
{
    public class UserProjectsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
