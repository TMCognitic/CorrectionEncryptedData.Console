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
        //if (publicKeyInfo is not null)
        //{
        //    RsaEncryption rsaEncryption = new RsaEncryption(publicKeyInfo.PublicKey);

        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        httpClient.BaseAddress = new Uri("https://localhost:7019");

        //        HttpResponseMessage httpResponseMessage = httpClient.PostAsJsonAsync("api/auth/register", new { Nom = "Doe", Prenom = "John", Email = "john.doe@test.be", Passwd = rsaEncryption.Encrypt("Test1234=") }).Result;

        //        Console.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        //    }
        //}

        Utilisateur? utilisateur = null;
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

                    utilisateur = JsonSerializer.Deserialize<Utilisateur>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    Console.WriteLine(httpResponseMessage.StatusCode);
                }

            }
        }

        if(utilisateur is not null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {utilisateur.Token}");

                httpClient.BaseAddress = new Uri("https://localhost:7019");

                string[]? strings = httpClient.GetFromJsonAsync<string[]>("api/test").Result;

                if(strings is not null)
                {
                    foreach (string s in strings)
                    {
                        Console.WriteLine(s);
                    }
                }
                

            }
        }
    }
}
