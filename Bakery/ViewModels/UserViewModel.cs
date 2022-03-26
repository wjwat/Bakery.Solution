using System.Collections.Generic;

using Bakery.Models;

namespace Bakery.ViewModels
{
  public class UserViewModel
  {
    public List<Flavor> Flavors { get; init; }
    public List<Treat> Treats { get; init; }
    public string UserId { get; init; }
  }
}
