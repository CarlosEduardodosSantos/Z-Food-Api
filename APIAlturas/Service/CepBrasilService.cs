using System;
using System.Collections.Generic;
using APIAlturas.ViewModels;
using RestSharp;

namespace APIAlturas.Service
{
    public class CepBrasilService
    {
        public CepViewModel ObterPorCep(string cep)
        {
            try
            {
                var client = new RestClient("https://viacep.com.br");
                var request = new RestRequest($"ws/{cep}/json/", Method.GET);
                var queryResult = client.Execute<CepViewModel>(request).Data;

                return queryResult;

            }
            catch (Exception e)
            {

                return null;
            }
        }

        public IEnumerable<CepViewModel> ObterPorEndereco(string estado, string cidade, string endereco)
        {
            try
            {
                var client = new RestClient("https://viacep.com.br");
                var request = new RestRequest($"ws/{estado}/{cidade}/{endereco}/json/", Method.GET);
                var queryResult = client.Execute< IEnumerable<CepViewModel>>(request).Data;

                return queryResult ?? new List<CepViewModel>();

            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}