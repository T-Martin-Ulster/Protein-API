using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/data")]
public class DataController : ControllerBase
{
    private readonly DataService _dataService;

    public DataController(DataService dataService) =>
        _dataService = dataService;

    [HttpGet]
    public async Task<List<Data>> Get() =>
        await _dataService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Data>> Get(string id)
    {
        var data = await _dataService.GetAsync(id);

        if (data is null)
        {
            return NotFound();
        }

        return data;
    }

    [HttpGet("targetId/{id:length(24)}")]
    public async Task<List<Data>> GetUsingTagetId(string targetId)
    {
        var data = await _dataService.GetByTargetAsync(targetId);
        return data;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Data newData)
    {
        var output = UpdateTangle(newData);

        if (output != null)
        {
            newData.MessageId = output;

            await _dataService.CreateAsync(newData);

            return CreatedAtAction(nameof(Get), new { id = newData.DataId }, newData);
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Data updatedData)
    {

        var output = UpdateTangle(updatedData);

        var data = await _dataService.GetAsync(id);

        if (data is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedData.DataId = data.DataId;

            await _dataService.UpdateAsync(id, updatedData);

            return NoContent();
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var data = await _dataService.GetAsync(id);

        if (data is null)
        {
            return NotFound();
        }

        await _dataService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Data data)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Data" + " " + json;

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