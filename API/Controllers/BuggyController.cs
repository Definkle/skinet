using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("badRequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Bad request");
    }

    [HttpGet("notFound")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("internalError")]
    public IActionResult GetInternalError()
    {
        throw new Exception("Internal error");
    }

    [HttpPost("validationError")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
    }
}