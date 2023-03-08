using ProteinApi.Models;
using ProteinApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProteinApi.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService) =>
        _transactionService = transactionService;

    [HttpGet]
    public async Task<List<Transaction>> Get() =>
        await _transactionService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Transaction>> Get(string id)
    {
        var transaction = await _transactionService.GetAsync(id);

        if (transaction is null)
        {
            return NotFound();
        }

        return transaction;
    }

    [HttpGet("sendorId/{id}")]
    public async Task<List<Transaction>> GetUsingSendorId(string businessId)
    {
        var transactions = await _transactionService.GetBySendorAsync(businessId);
        return transactions;
    }

    [HttpGet("reciverId/{id}")]
    public async Task<List<Transaction>> GetUsingReciverId(string businessId)
    {
        var transactions = await _transactionService.GetByReciverAsync(businessId);
        return transactions;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Transaction newTransaction)
    {
        var output = UpdateTangle(newTransaction);

        if (output != null)
        {
            newTransaction.MessageId = output;

            await _transactionService.CreateAsync(newTransaction);

            return CreatedAtAction(nameof(Get), new { id = newTransaction.TransactionId }, newTransaction);
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Transaction updatedTransaction)
    {

        var output = UpdateTangle(updatedTransaction);

        var transaction = await _transactionService.GetAsync(id);

        if (transaction is null)
        {
            return NotFound();
        }

        if (output != null)
        {

            updatedTransaction.TransactionId = transaction.TransactionId;

            await _transactionService.UpdateAsync(id, updatedTransaction);

            return NoContent();
        }
        return StatusCode(500, "Tangle not responding");
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var transaction = await _transactionService.GetAsync(id);

        if (transaction is null)
        {
            return NotFound();
        }

        await _transactionService.RemoveAsync(id);

        return NoContent();
    }

    public static string? UpdateTangle(Transaction transaction)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(transaction);

        string filename = Path.Combine(Environment.CurrentDirectory, @"Scripts/message_post");

        string tangelPath = Globals.tanglePath;

        string cParams = tangelPath + " " + "Transaction" + " " + json;

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