using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakery.Models
{
  public class Treat
  {
    public Treat()
    {
      JoinEntities = new HashSet<FlavorTreat> {};
    }

    public int TreatId { get; set; }

    [Required, StringLength(40, MinimumLength = 3)]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<FlavorTreat> JoinEntities { get; }
  }
}
