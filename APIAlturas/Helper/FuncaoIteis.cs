using System;

namespace APIAlturas.Helper
{
    public static class FuncaoIteis
    {
        public static string SoNumeros(string texto)
        {
            return String.Join("", System.Text.RegularExpressions.Regex.Split(texto, @"[^\d]"));
        }
    }
}