using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrectionEncryptedData.ConsoleApp.Models
{
    public class Utilisateur
    {
        public int Id { get; }
        public string Nom { get; }
        public string Prenom { get; }
        public string Email { get; }
        public string Token { get; }

        public Utilisateur(int id, string nom, string prenom, string email, string token)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Token = token;
        }
    }
}
