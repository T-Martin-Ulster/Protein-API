using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/shipment")]
public class ShipmentController : ControllerBase
{
    private readonly ShipmentService _shipmentService;

    public ShipmentController(ShipmentService shipmentService) =>
        _shipmentService = shipmentService;

    [HttpGet]
    public async Task<List<Shipment>> Get() =>
        await _shipmentService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Shipment>> Get(string id)
    {
        var shipment = await _shipmentService.GetAsync(id);

        if (shipment is null)
        {
            return NotFound();
        }

        return shipment;
    }

    [HttpGet("businessId/{id}")]
    public async Task<List<Shipment>> GetUsingBusinessId(string businessId)
    {
        var shipment = await _shipmentService.GetByBusinessAsync(businessId);
        return shipment;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Shipment newShipment)
    {
   
        await _shipmentService.CreateAsync(newShipment);

        return CreatedAtAction(nameof(Get), new { id = newShipment.ShipmentId }, newShipment);
   
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Shipment updatedShipment)
    {

        var shipment = await _shipmentService.GetAsync(id);

        if (shipment is null)
        {
            return NotFound();
        }

        updatedShipment.ShipmentId = shipment.ShipmentId;

        await _shipmentService.UpdateAsync(id, updatedShipment);

        return NoContent();

    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var shipment = await _shipmentService.GetAsync(id);

        if (shipment is null)
        {
            return NotFound();
        }

        await _shipmentService.RemoveAsync(id);

        return NoContent();
    }

}