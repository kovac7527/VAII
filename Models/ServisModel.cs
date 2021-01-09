using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccesLib.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VAII.Models
{
    public class ServisModel
    {
      
        public IEnumerable<ServisDevice> DeviceSeries { get; set; }
        
    }
}
