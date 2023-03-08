using ProteinApi.Services;
using ProteinApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProteinIDatabaseSettings>(
    builder.Configuration.GetSection("ProteinIDatabase"));

builder.Services.AddSingleton<BatchService>();
builder.Services.AddSingleton<BusinessService>();
builder.Services.AddSingleton<CropService>();
builder.Services.AddSingleton<DataService>();
builder.Services.AddSingleton<FieldService>();
builder.Services.AddSingleton<MixedBatchService>();
builder.Services.AddSingleton<ShipmentService>();
builder.Services.AddSingleton<TransactionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

