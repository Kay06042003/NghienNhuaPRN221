using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.ViewModels
{
    public class LoginViewModel
    {
        [JsonIgnore]
        public Account Account { get; set; }
    }
}