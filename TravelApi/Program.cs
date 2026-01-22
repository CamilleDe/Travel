using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TravelApi;

var builder = WebApplication.CreateBuilder(args);

// Gère les références circulaires JSON (Trip -> Places -> Trip)
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// CORS pour le frontend Vue
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// Configuration EF Core avec PostgreSQL
builder.Services.AddDbContext<TravelDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TravelDb")));

var app = builder.Build();

app.UseCors();

// ============================================
// ENDPOINTS
// ============================================

// Health check simple
app.MapGet("/", () => "🌍 Travel API is running!");

// GET /trips - Liste tous les voyages avec leurs lieux
app.MapGet("/trips", async (TravelDb db) =>
    await db.Trips
        .Include(t => t.Places)
        .ToListAsync());

// GET /trips/{id} - Un voyage par son ID
app.MapGet("/trips/{id}", async (int id, TravelDb db) =>
    await db.Trips
        .Include(t => t.Places)
        .FirstOrDefaultAsync(t => t.Id == id)
        is Trip trip
            ? Results.Ok(trip)
            : Results.NotFound());

// POST /trips - Créer un voyage
app.MapPost("/trips", async (Trip trip, TravelDb db) =>
{
    db.Trips.Add(trip);
    await db.SaveChangesAsync();
    return Results.Created($"/trips/{trip.Id}", trip);
});

// PUT /trips/{id} - Modifier un voyage
app.MapPut("/trips/{id}", async (int id, Trip input, TravelDb db) =>
{
    var trip = await db.Trips.FindAsync(id);
    if (trip is null) return Results.NotFound();

    trip.Title = input.Title;
    trip.StartDate = input.StartDate;
    trip.EndDate = input.EndDate;
    trip.Notes = input.Notes;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE /trips/{id} - Supprimer un voyage
app.MapDelete("/trips/{id}", async (int id, TravelDb db) =>
{
    var trip = await db.Trips.FindAsync(id);
    if (trip is null) return Results.NotFound();

    db.Trips.Remove(trip);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// POST /trips/{tripId}/places - Ajouter un lieu à un voyage
app.MapPost("/trips/{tripId}/places", async (int tripId, Place place, TravelDb db) =>
{
    var trip = await db.Trips.FindAsync(tripId);
    if (trip is null) return Results.NotFound();

    place.TripId = tripId;
    db.Places.Add(place);
    await db.SaveChangesAsync();
    return Results.Created($"/trips/{tripId}/places/{place.Id}", place);
});

app.Run();

// ============================================
// DbContext - Simple et direct
// ============================================

public class TravelDb : DbContext
{
    public TravelDb(DbContextOptions<TravelDb> options) : base(options) { }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Place> Places => Set<Place>();
}
