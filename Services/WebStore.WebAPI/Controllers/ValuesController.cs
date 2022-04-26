using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers;

//[Route("api/[controller]")] // http://localhost:5209/api/values
[Route(WebAPIAddresses.V1.Values)] // http://localhost:5209/api/values
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ILogger<ValuesController> _Logger;

    private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
        .Select(i => (Id: i, Value: $"Value-{i}"))
        .ToDictionary(v => v.Id, v => v.Value);

    public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

    [HttpGet]
    public IActionResult GetAll()
    {
        var values = _Values.Values;
        return Ok(values);
    }

    [HttpGet("{Id}")]
    public IActionResult GetById(int Id)
    {
        //if(!_Values.ContainsKey(id))
        //    return NotFound();

        //return Ok(_Values[id]);

        if(_Values.TryGetValue(Id, out var value))
            return Ok(value);
        return NotFound(new {Id});
    }

    [HttpGet("count")]
    public int Count() => _Values.Count;

    [HttpPost] // POST -> http://localhost:5209/api/values
    [HttpPost("add")]  // POST -> http://localhost:5209/api/values/add
    public IActionResult Add([FromBody] string Value)
    {
        var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
        _Values[id] = Value;

        _Logger.LogInformation("Добавлено значение {0} с Id:{1}", Value, id);
        return CreatedAtAction(nameof(GetById), new { Id = id }, Value);
    }

    [HttpPut("{Id}")]
    public IActionResult Edit(int Id, [FromBody] string Value)
    {
        if (!_Values.ContainsKey(Id))
        {
            _Logger.LogWarning("Попытка редактирования отсутствующего значения с Id:{0}", Id);
            return NotFound(new { Id });
        }
        var old_value = _Values[Id];
        _Values[Id] = Value;

        _Logger.LogInformation("Выполнено изменение значения с Id:{0} с {1} на {2}", Id, old_value, Value);
        return Ok(new {Value, OldValue = old_value});
    }

    [HttpDelete("{Id}")]  // POST -> http://localhost:5209/api/values/42
    public IActionResult Delete(int Id)
    {
        if (!_Values.ContainsKey(Id))
        {
            _Logger.LogWarning("Попытка удаления отсутствующего значения с Id:{0}", Id);
            return NotFound(new { Id });
        }

        var value = _Values[Id];
        _Values.Remove(Id);

        _Logger.LogInformation("Значение {0} с id:{1} удалено", value, Id);

        return Ok(new {Value = value});
    }

    [HttpGet("throw/{Message}")]
    public void ThrowException(string? Message)
    {
        throw new ApplicationException(Message ?? "Controller error");
    }
}
