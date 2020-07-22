using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using MonitoramentoBasesSql.Clients;
using MonitoramentoBasesSql.Models;

namespace MonitoramentoBasesSql
{
    public class MonitorBases
    {
        public readonly CanalSlackClient _canalSlackClient;

        public MonitorBases(CanalSlackClient canalSlackClient)
        {
            _canalSlackClient = canalSlackClient;
        }

        [FunctionName("MonitorBases")]
        public void Run([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var conexoes = Environment.GetEnvironmentVariable("Conexoes")
                .Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (string nomeConexao in conexoes)
            {
                try
                {
                    log.LogInformation($"Verificando a conexão {nomeConexao}...");
                    using var conexao = new SqlConnection(Environment.GetEnvironmentVariable(nomeConexao));
                    conexao.Open();
                    conexao.Close();
                    log.LogInformation($"Acesso à base da conexão {nomeConexao} efetuado com sucesso!");
                }
                catch (Exception ex)
                {
                    log.LogError($"Erro com a conexão {nomeConexao}: {ex.Message}");
                    _canalSlackClient.PostAlerta(nomeConexao, ex.Message);
                    log.LogInformation("Envio de alerta");
                }
            }
            
            log.LogInformation($"Monitoramento executado em: {DateTime.Now}");
        }
    }
}