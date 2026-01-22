using System.Text.Json.Serialization;

namespace TravelApi;

// Entités simples, pas de DTO, pas d'abstractions
// On ajoutera des propriétés au besoin

public class Trip
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Notes { get; set; }

    // Navigation : un Trip contient plusieurs Places
    public List<Place> Places { get; set; } = [];
}

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Clé étrangère vers Trip
    public int TripId { get; set; }
    
    [JsonIgnore] // Évite la boucle infinie JSON : Place -> Trip -> Places -> Trip...
    public Trip? Trip { get; set; }
}
