using System.ComponentModel.DataAnnotations;

namespace ArithmeticAPI.Core;

public class CalculationRequest
{
    [Required]
    [RegularExpression("^(add|subtract|multiply|divide)$",
        ErrorMessage = "Operation must be add, subtract, multiply or divide")]
    public string Operation { get; set; } = string.Empty;

    [Range(-1000000, 1000000,
        ErrorMessage = "Operand1 must be between -1000000 and 1000000")]
    public double Operand1 { get; set; }

    [Range(-1000000, 1000000,
        ErrorMessage = "Operand2 must be between -1000000 and 1000000")]
    public double Operand2 { get; set; }
}
