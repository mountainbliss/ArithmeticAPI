using Microsoft.AspNetCore.Mvc;
using ArithmeticAPI.Core;
using ArithmeticAPI.Api.Filters;

namespace ArithmeticAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuditLogFilter))]
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
    //[TypeFilter(typeof(ResponseCacheFilter), Arguments = new object[] { 60 })]
    [ResponseCache(Duration = 60)]
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

    // POST /api/arithmetic/calculate
    // Body: { "operation": "add", "operand1": 10, "operand2": 5 }
    [HttpPost("calculate")]
    public ActionResult<CalculationResult> Calculate([FromBody] CalculationRequest request)
    {
        var result = request.Operation switch
        {
            "add"      => _arithmeticService.Add(request.Operand1, request.Operand2),
            "subtract" => _arithmeticService.Subtract(request.Operand1, request.Operand2),
            "multiply" => _arithmeticService.Multiply(request.Operand1, request.Operand2),
            "divide"   => _arithmeticService.Divide(request.Operand1, request.Operand2),
            _          => throw new ArgumentException($"Unknown operation: {request.Operation}")
        };

        return Ok(new CalculationResult
        {
            Operand1  = request.Operand1,
            Operand2  = request.Operand2,
            Operation = request.Operation,
            Result    = result
        });
    }
}