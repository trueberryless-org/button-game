var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.MultiplayerGame_ApiService>("apiservice");

var mongodb = builder
    .AddMongoDBContainer("mongodb")
    .WithVolumeMount("./Volumns/MultiplayerMongo", "/data/db");

builder.AddProject<Projects.MultiplayerGame_Web>("webfrontend")
    .WithReference(apiService)
    .WithReference(mongodb);

// builder.AddMongoDB("IMongoClient");

builder.Build().Run();