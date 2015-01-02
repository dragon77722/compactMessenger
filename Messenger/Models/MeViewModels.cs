using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    // Models returned by MeController actions.
    public class GetViewModel
    {
        public string Hometown { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string University { get; set; }

    }
}