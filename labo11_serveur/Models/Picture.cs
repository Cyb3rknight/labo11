namespace semaine11_serveur.Models
{
    public class Picture
    {
        public int Id { get; set; } // Identifiant unique
        public string FilePath { get; set; } // Chemin du fichier
        public DateTime CreatedAt { get; set; } // Date de création
    }
}