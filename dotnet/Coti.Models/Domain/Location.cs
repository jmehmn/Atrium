using System;
using System.Collections.Generic;
using System.Text;

namespace Coti.Models.Domain
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LineOne { get; set; }

        public string LineTwo { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }

    }
}
