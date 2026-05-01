using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium5.Models
{
   internal class Contact { 
   public int Id { get; set; } // PK
   public string FirstName { get; set; } = ""; 
   public string LastName { get; set; } = ""; 
   public string? Phone { get; set; } 
   public string? Email { get; set; } 
   public override string ToString() => $"{Id} | {FirstName} | {LastName} | {Phone ?? "-"} | {Email ?? "-"}"; }
}
