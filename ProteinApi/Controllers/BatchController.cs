using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/batch")]
public class BatchController : ControllerBase
{
    private readonly BatchService _batchService;

    public BatchController(BatchService batchService) =>
        _batchService = batchService;

    [HttpGet]
    public async Task<List<Batch>> Get() =>
        await _batchService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Batch>> Get(string id)
    {
        var batch = await _batchService.GetAsync(id);

        if (batch is null)
        {
            return NotFound();
        }

        return batch;
    }

    [HttpGet("businessId/{id}")]
    public async Task<List<Batch>> GetUsingBusinessId(string businessId)
    {
        var batch = await _batchService.GetByBusinessAsync(businessId);
        return batch;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Batch newBatch)
    {
        var output = UpdateTangle(newBatch);

        if (output != null)
        {
            newBatch.MessageId = output;

            await _batchService.CreateAsync(newBatch);

            return CreatedAtAction(nameof(Get), new { id = newBatch.BatchId }, newBatch);
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Batch updatedBatch)
    {

        var output = UpdateTangle(updatedBatch);

        var batch = await _batchService.GetAsync(id);

        if (batch is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedBatch.BatchId = batch.BatchId;

            await _batchService.UpdateAsync(id, updatedBatch);

            return NoContent();
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var batch = await _batchService.GetAsync(id);

        if (batch is null)
        {
            return NotFound();
        }

        await _batchService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Batch batch)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(batch);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Batch" + " " + json;

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