﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Farmacias
    {
        public string  Cnpj { get; set; }
        public string ConString { get; set; }

            
        private static string _ConString = "";
        public static string Conexao
        {
            get { return _ConString; }
            set { _ConString = value; }
        }
    }
}
