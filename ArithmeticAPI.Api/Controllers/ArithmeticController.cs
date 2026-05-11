using Microsoft.AspNetCore.Mvc;
using ArithmeticAPI.Api.Services;
using ArithmeticAPI.Api.Models;

namespace ArithmeticAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArithmeticController : ControllerBase
    {
        private readonly ArithmeticService _arithmeticService;

        public ArithmeticController(ArithmeticService arithmeticService)
        {
            _arithmeticService = arithmeticService;
        }

        [HttpGet("add")]
        public ActionResult<CalculationResult> Add([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new CalculationResult
            {
                Operand1 = a,
                Operand2 = b,
                Operation = "add",
                Result = _arithmeticService.Add(a, b)
            });
        }

        [HttpGet("subtract")]
        public ActionResult<CalculationResult> Subtract([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new CalculationResult
            {
                Operand1 = a,
                Operand2 = b,
                Operation = "subtract",
                Result = _arithmeticService.Subtract(a, b)
            });
        }

        [HttpGet("multiply")]
        public ActionResult<CalculationResult> Multiply([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new CalculationResult
            {
                Operand1 = a,
                Operand2 = b,
                Operation = "multiply",
                Result = _arithmeticService.Multiply(a, b)
            });
        }

        [HttpGet("divide")]
        public ActionResult<CalculationResult> Divide([FromQuery] double a, [FromQuery] double b)
        {
            if (b == 0)
                return BadRequest(new { error = "Divisor (b) cannot be zero." });

            return Ok(new CalculationResult
            {
                Operand1 = a,
                Operand2 = b,
                Operation = "divide",
                Result = _arithmeticService.Divide(a, b)
            });
        }
    }
}
