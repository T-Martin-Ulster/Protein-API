using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/business")]
public class BusinessController : ControllerBase
{
    private readonly BusinessService _businessService;

    public BusinessController(BusinessService businessService) =>
        _businessService = businessService;

    [HttpGet]
    public async Task<List<Business>> Get() =>
        await _businessService.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Business>> Get(string id)
    {
        var business = await _businessService.GetAsync(id);

        if (business is null)
        {
            return NotFound();
        }

        return business;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Business newBusiness)
    {
        var output = UpdateTangle(newBusiness);

        if (output != null)
        {
            newBusiness.MessageId = output;

            await _businessService.CreateAsync(newBusiness);

            return CreatedAtAction(nameof(Get), new { id = newBusiness.BusinessId }, newBusiness);

        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Business updatedBusiness)
    {

        var output = UpdateTangle(updatedBusiness);

        var business = await _businessService.GetAsync(id);

        if (business is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedBusiness.BusinessId = business.BusinessId;

            await _businessService.UpdateAsync(id, updatedBusiness);

            return NoContent();
        }

        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var business = await _businessService.GetAsync(id);

        if (business is null)
        {
            return NotFound();
        }

        await _businessService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Business business)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(business);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Business" + " " + json;

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