using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace MonitoramentoBasesSql.Clients
{
    public class CanalSlackClient
    {
        private HttpClient _client;

        public CanalSlackClient(HttpClient client)
        {
            _client = client;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void PostAlerta(string nomeConexao, string mensagemErro)
        {
            _client.BaseAddress = new Uri(
                  Environment.GetEnvironmentVariable("UrlLogicAppSlack"));
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var requestMessage =
                  new HttpRequestMessage(HttpMethod.Post, String.Empty);

            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    baseDados = nomeConexao,
                    alerta = mensagemErro,
                }), Encoding.UTF8, "application/json");

            var respLogicApp = _client
                .SendAsync(requestMessage).Result;
            respLogicApp.EnsureSuccessStatusCode();
        }
    }
}