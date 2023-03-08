using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/field")]
public class FieldController : ControllerBase
{
    private readonly FieldService _fieldService;

    public FieldController(FieldService fieldService) =>
        _fieldService = fieldService;

    [HttpGet]
    public async Task<List<Field>> Get() =>
        await _fieldService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Field>> Get(string id)
    {
        var field = await _fieldService.GetAsync(id);

        if (field is null)
        {
            return NotFound();
        }

        return field;
    }

    [HttpGet("businessId/{id}")]
    public async Task<List<Field>> GetUsingBusinessId(string businessId)
    {
        var field = await _fieldService.GetByBusinessAsync(businessId);
        return field;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Field newField)
    {
        var output = UpdateTangle(newField);

        if (output != null)
        {
            newField.MessageId = output;

            await _fieldService.CreateAsync(newField);

            return CreatedAtAction(nameof(Get), new { id = newField.FieldId }, newField);
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Field updatedField)
    {

        var output = UpdateTangle(updatedField);

        var field = await _fieldService.GetAsync(id);

        if (field is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedField.FieldId = field.FieldId;

            await _fieldService.UpdateAsync(id, updatedField);

            return NoContent();
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var field = await _fieldService.GetAsync(id);

        if (field is null)
        {
            return NotFound();
        }

        await _fieldService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Field field)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(field);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Field" + " " + json;

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