using Microsoft.AspNetCore.Mvc;
using ArithmeticAPI.Core;

namespace ArithmeticAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArithmeticController : ControllerBase
{
    private readonly ArithmeticService _arithmeticService;
    private readonly CalculationHistoryService _historyService;

    public ArithmeticController(ArithmeticService arithmeticService, CalculationHistoryService historyService)
    {
        _arithmeticService = arithmeticService;
        _historyService = historyService;
    }

    [HttpGet("add")]
    public ActionResult<CalculationResult> Add([FromQuery] double a, [FromQuery] double b)
    {
        var result = new CalculationResult
        {
            Operand1 = a,
            Operand2 = b,
            Operation = "add",
            Result = _arithmeticService.Add(a, b)
        };
        _historyService.AddToHistory(result);
        return Ok(result);
    }

    [HttpGet("subtract")]
    public ActionResult<CalculationResult> Subtract([FromQuery] double a, [FromQuery] double b)
    {
        var result = new CalculationResult
        {
            Operand1 = a,
            Operand2 = b,
            Operation = "subtract",
            Result = _arithmeticService.Subtract(a, b)
        };
        _historyService.AddToHistory(result);
        return Ok(result);
    }

    [HttpGet("multiply")]
    public ActionResult<CalculationResult> Multiply([FromQuery] double a, [FromQuery] double b)
    {
        var result = new CalculationResult
        {
            Operand1 = a,
            Operand2 = b,
            Operation = "multiply",
            Result = _arithmeticService.Multiply(a, b)
        };
        _historyService.AddToHistory(result);
        return Ok(result);
    }

    [HttpGet("divide")]
    public ActionResult<CalculationResult> Divide([FromQuery] double a, [FromQuery] double b)
    {
        if (b == 0)
            return BadRequest(new { error = "Divisor (b) cannot be zero." });

        var result = new CalculationResult
        {
            Operand1 = a,
            Operand2 = b,
            Operation = "divide",
            Result = _arithmeticService.Divide(a, b)
        };
        _historyService.AddToHistory(result);
        return Ok(result);
    }

    // GET /api/arithmetic/history
    [HttpGet("history")]
    public ActionResult<IEnumerable<CalculationResult>> GetHistory()
    {
        return Ok(_historyService.GetAll());
    }

    // GET /api/arithmetic/history/top2
    [HttpGet("history/top2")]
    public ActionResult<IEnumerable<CalculationResult>> GetTop2()
    {
        return Ok(_historyService.GetTopTwoResults());
    }

    // GET /api/arithmetic/history/by-operation?operation=add
    [HttpGet("history/by-operation")]
    public ActionResult<IEnumerable<CalculationResult>> GetByOperation([FromQuery] string operation)
    {
        return Ok(_historyService.GetByOperation(operation));
    }
}