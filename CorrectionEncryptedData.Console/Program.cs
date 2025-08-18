// See https://aka.ms/new-console-template for more information
using CorrectionEncryptedData.ConsoleApp.Models;
using System;
using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using Tools.Cryptographie;

namespace CorrectionEncryptedData.ConsoleApp;

public static class Program
{
    static void Main(string[] args)
    {
        PublicKeyInfo? publicKeyInfo = null;

        //Récupérer la clé publique
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri("https://localhost:7019");

            publicKeyInfo = httpClient.GetFromJsonAsync<PublicKeyInfo>("api/Security/PublicKey", new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).Result;
        }

        //Enregistrement de l'utilisateur
        if (publicKeyInfo is not null)
        {
            RsaEncryption rsaEncryption = new RsaEncryption(publicKeyInfo.PublicKey);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7019");

                HttpResponseMessage httpResponseMessage = httpClient.PostAsJsonAsync("api/auth/register", new { Nom = "Doe", Prenom = "John", Email = "john.doe@test.be", Passwd = rsaEncryption.Encrypt("Test1234=") }).Result;

                Console.WriteLine(httpResponseMessage.IsSuccessStatusCode);
            }
        }


        //Login
        if (publicKeyInfo is not null)
        {
            RsaEncryption rsaEncryption = new RsaEncryption(publicKeyInfo.PublicKey);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7019");

                HttpResponseMessage httpResponseMessage = httpClient.PostAsJsonAsync("api/auth/login", new { Email = "john.doe@test.be", Passwd = rsaEncryption.Encrypt("Test1234=") }).Result;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(json);
                }
                else
                {
                    Console.WriteLine(httpResponseMessage.StatusCode);
                }

            }
        }
    }
}
