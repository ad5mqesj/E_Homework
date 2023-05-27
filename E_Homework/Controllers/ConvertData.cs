using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using E_Homework.DTO;
using E_Homework.DTO.Models;
using E_Homework.DTO.Validators;
using E_Homework.Providers.Implementation;

namespace E_Homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertDataController : ControllerBase
    {
        #region PrivateProperties
        private ILogger logger;
        private IDataConverter converter;
        private JsonSerializerOptions options;
        #endregion PrivateProperties

        #region Construcor(s)
        public ConvertDataController(ILogger _logger, IDataConverter _conv)
        {
            logger = _logger;
            converter = _conv;

            //setup custom date deserializer
            options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("MM-dd-yyyy H:mm:ss"));
        }
        #endregion Construcor(s)

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommonDeviceData>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Convert([FromBody]  string jsonObject)
        {
            //try to convert to Foo1 and then Foo2, if neithe match then bail
            IActionResult? result;
            object? data = null;
            try
            {
                var validator = new Foo1Validator();

                data = JsonSerializer.Deserialize<Foo1>(jsonObject, options);
                if (data != null)
                {
                    var vresult = validator.Validate(data as Foo1);
                    if (!vresult.IsValid)
                        data = null;
                }
            }
            catch (Exception e)
            {
                logger.LogTrace($"Failed to convert input as Foo1 {e.Message}");
            }
            if (data == null)
            {
                try
                {
                    var validator = new Foo2Validator();
                    data = JsonSerializer.Deserialize<Foo2>(jsonObject, options);
                    if (data != null)
                    {
                        var vresult = validator.Validate(data as Foo2);
                        if (!vresult.IsValid)
                            return BadRequest(vresult);
                    }
                }
                catch (Exception e)
                {
                    logger.LogTrace($"Failed to convert input as Foo2 {e.Message}");
                }
            }
            //if it failed both ways bail out
            if (data == null)
            {
                result = BadRequest(ModelState);
            }

            IEnumerable < CommonDeviceData > convData = converter.ConvertData(data);
            result = Ok(convData);
            return result;
        }
    }
}
