using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/mix")]
public class MixedBatchController : ControllerBase
{
    private readonly MixedBatchService _mixedbatchService;

    public MixedBatchController(MixedBatchService mixedbatchService) =>
        _mixedbatchService = mixedbatchService;

    [HttpGet]
    public async Task<List<MixedBatch>> Get() =>
        await _mixedbatchService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<MixedBatch>> Get(string id)
    {
        var mixedbatch = await _mixedbatchService.GetAsync(id);

        if (mixedbatch is null)
        {
            return NotFound();
        }

        return mixedbatch;
    }

    [HttpPost]
    public async Task<IActionResult> Post(MixedBatch newMixedBatch)
    {
        var output = UpdateTangle(newMixedBatch);

        if (output != null)
        {
            newMixedBatch.MessageId = output;

            await _mixedbatchService.CreateAsync(newMixedBatch);

            return CreatedAtAction(nameof(Get), new { id = newMixedBatch.MixedBatchId }, newMixedBatch);
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, MixedBatch updatedMixedBatch)
    {

        var output = UpdateTangle(updatedMixedBatch);

        var mixedbatch = await _mixedbatchService.GetAsync(id);

        if (mixedbatch is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedMixedBatch.MixedBatchId = mixedbatch.MixedBatchId;

            await _mixedbatchService.UpdateAsync(id, updatedMixedBatch);

            return NoContent();
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var mixedbatch = await _mixedbatchService.GetAsync(id);

        if (mixedbatch is null)
        {
            return NotFound();
        }

        await _mixedbatchService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(MixedBatch mixedbatch)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(mixedbatch);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "MixedBatch" + " " + json;

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