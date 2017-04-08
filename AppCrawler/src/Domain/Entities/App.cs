﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class App
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public IList<string> Categories { get; set; }

        public IList<string> Screenshots { get; set; }

        public string Link { get; set; }
    }
}