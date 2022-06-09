using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Engineer
    {
        public Engineer()
        {
          this.JoinEntities = new HashSet<EngineerMachine>();
        }

        public int EngineerId { get; set; }
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<EngineerMachine> JoinEntities { get; set; }
    }
}