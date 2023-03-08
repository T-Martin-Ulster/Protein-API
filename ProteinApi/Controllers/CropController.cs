using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/crop")]
public class CropController : ControllerBase
{
    private readonly CropService _cropService;

    public CropController(CropService cropService) =>
        _cropService = cropService;

    [HttpGet]
    public async Task<List<Crop>> Get() =>
        await _cropService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Crop>> Get(string id)
    {
        var crop = await _cropService.GetAsync(id);

        if (crop is null)
        {
            return NotFound();
        }

        return crop;
    }

    [HttpGet("businessId/{id}")]
    public async Task<List<Crop>> GetUsingBusinessId(string businessId)
    {
        var crops = await _cropService.GetByBusinessAsync(businessId);
        return crops;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Crop newCrop)
    {

        var output = UpdateTangle(newCrop);

        if (output != null)
        {
            newCrop.MessageId = output;

            await _cropService.CreateAsync(newCrop);

            return CreatedAtAction(nameof(Get), new { id = newCrop.CropId }, newCrop);

        }

        return StatusCode(500, "Tangle not responding");
        
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Crop updatedCrop)
    {

        var output = UpdateTangle(updatedCrop);

        var crop = await _cropService.GetAsync(id);

        if (crop is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedCrop.CropId = crop.CropId;

            await _cropService.UpdateAsync(id, updatedCrop);

            return NoContent();
        }

        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var crop = await _cropService.GetAsync(id);

        if (crop is null)
        {
            return NotFound();
        }

        await _cropService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Crop crop)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(crop);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Crop" + " " + json;

        //Runs Exe file
        var proc = new Process();

        proc.StartInfo.RedirectStandardOutput = true;

        proc.StartInfo.UseShellExecute = false;

        proc.StartInfo.FileName = filename;

        proc.StartInfo.Arguments = cParams;

        proc.Start();

        var output = proc.StandardOutput.ReadLine();

        return output;

    }
}