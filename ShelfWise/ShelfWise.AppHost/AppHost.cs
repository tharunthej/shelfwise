using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// --- Postgres credentials (username visible, password kept as a secret) ---
var pgUser = builder.AddParameter("postgres-username", "shelfwise");
var pgPass = builder.AddParameter("postgres-password", secret: true); // set via user-secrets

// --- Postgres container with persistence + optional pgAdmin ---
var pg = builder.AddPostgres("postgres", pgUser, pgPass)
                .WithImage("postgres:16-alpine")   // pin version for reproducibility
                .WithDataVolume()                  // keep dev data across restarts
                .WithPgAdmin(pg => pg.WithHostPort(5050)); // optional admin UI (http://localhost:5050)

// Logical database resource; this name is the connection-name used in the API.
var db = pg.AddDatabase("shelfwise");

// --- .NET API project ---
// Make sure your API uses: builder.AddNpgsqlDbContext<AppDbContext>("shelfwise");
var api = builder.AddProject<Projects.ShelfWise_API>("api")
                 .WithReference(db)                // injects the shelfwise connection string
                 .WithExternalHttpEndpoints();     // expose to browser

// --- React (Vite) frontend ---
// Aspire allocates a free port and sets PORT; service discovery env vars point to the API.
//builder.AddNpmApp("web", "../shelfwise-web")
//       .WithReference(api)                         // gives env: services__api__http__0 / __https__0
//       .WithHttpEndpoint(env: "PORT")              // don't hardcode 5173; let Aspire pick
//       .WithEnvironment("BROWSER", "none")         // avoid auto-opening extra tabs
//       .WithExternalHttpEndpoints()
//       // .PublishAsDockerFile()                   // uncomment if you want a FE container image on publish
//       ;

builder.Build().Run();

