using EquipmentManagementPlatform.API.Websockets;
using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.DomainServices.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EquipmentManagementPlatform.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly ILogger<EquipmentController> _logger;
        private readonly IEquipmentService _equipmentService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EquipmentController(ILogger<EquipmentController> logger, IEquipmentService equipmentService, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _equipmentService = equipmentService;
            _hubContext = hubContext;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetEquipment()
        {
            _logger.LogInformation("Received request to get all equipment");

            var equipmentModelsList = await _equipmentService.GetAllEquipment();

            return Ok(equipmentModelsList.ToList().ConvertAll(eq =>
                new EquipmentDto(eq)
            ));
        }

        [HttpGet("{equipmentId}")]
        public async Task<ActionResult<EquipmentDto>> GetEquipment(int equipmentId)
        {
            _logger.LogInformation("Received request to get equipment with id: {equipmentId}", equipmentId);

            var equipmentModel = await _equipmentService.GetById(equipmentId);

            return Ok(new EquipmentDto(equipmentModel));
        }

        [HttpPatch("{equipmentId}/UpdateState")]
        public async Task<ActionResult> UpdateEquipmentState(int equipmentId, [FromBody]UpdateStateDto updateStateDto)
        {
            _logger.LogInformation("Received request to update equipment state to {newState}", updateStateDto.NewState);

            ValidateUpdateStateDto(updateStateDto);

            var domainRequest = new UpdateEquipmentStateRequest(equipmentId, updateStateDto.NewState, updateStateDto.OrderId);

            await _equipmentService.UpdateEquipmentState(domainRequest);

            await _hubContext.Clients.All.SendAsync("StateUpdate", equipmentId);

            return NoContent();
        }

        [HttpPost("{equipmentId}/Start")]
        public async Task<ActionResult> StartEquipment(int equipmentId, [FromBody] StartEquipmentDto startEquipmentDto)
        {
            _logger.LogInformation("Received request to start equipment with id {equipmentId}", equipmentId);

            await _equipmentService.StartEquipment(equipmentId, startEquipmentDto?.OrderId);

            await _hubContext.Clients.All.SendAsync("StateUpdate", equipmentId);

            return NoContent();
        }

        [HttpPost("{equipmentId}/Stop")]
        public async Task<ActionResult> StopEquipment(int equipmentId)
        {
            _logger.LogInformation("Received request to stop equipment with id {equipmentId}", equipmentId);

            await _equipmentService.StopEquiment(equipmentId);

            await _hubContext.Clients.All.SendAsync("StateUpdate", equipmentId);

            return NoContent();
        }

        [HttpPatch("{equipmentId}/AddOrders")]
        public async Task<ActionResult> AddEquipmentOrders(int equipmentId, [FromBody] AddEquipmentOrdersDto addEquipmentOrdersDto)
        {

            ValidateAddEquipmentOrdersDto(addEquipmentOrdersDto);

            _logger.LogInformation("Received request to assign additional orders to equipment with id: {equipmentId} with orders: {orders}", equipmentId, string.Join(",", addEquipmentOrdersDto.EquipmentOrders));

            await _equipmentService.AddEquipmentOrders(equipmentId, addEquipmentOrdersDto.EquipmentOrders);

            return NoContent();
        }

        [HttpPatch("{equipmentId}/AssignOperator")]
        public async Task<ActionResult> AssignOperator(int equipmentId, [FromBody] AssignEquipmentOperatorDto assignOperatorDto)
        {
            ValidateAssignEquipmentOperatorDto(assignOperatorDto);

            _logger.LogInformation("Persisting assignment employee: {employee} to equipment with id: {id}", assignOperatorDto.Employee, equipmentId);

            await _equipmentService.AssignOperator(equipmentId, assignOperatorDto.Employee);

            return NoContent();
        }

        private static void ValidateAddEquipmentOrdersDto(AddEquipmentOrdersDto addEquipmentOrdersDto)
        {
            if(addEquipmentOrdersDto is null)
                throw new ArgumentException("The requestBody is missing");
            if (addEquipmentOrdersDto.EquipmentOrders is null || !addEquipmentOrdersDto.EquipmentOrders.Any())
                throw new ArgumentException($"{nameof(addEquipmentOrdersDto.EquipmentOrders)} is null or empty");
        }

        private static void ValidateUpdateStateDto(UpdateStateDto updateStateDto)
        {
            if (updateStateDto is null)
                throw new ArgumentException("The requestBody is missing");
            if (updateStateDto.NewState.Equals(string.Empty))
                throw new ArgumentException($"{nameof(updateStateDto.NewState)} is missing in the request");
        }

        private static void ValidateRegisterEquipmentDto(RegisterEquipmentDto registerEquipmentDto)
        {
            if (registerEquipmentDto is null)
                throw new ArgumentException("The requestBody is missing");
            if (registerEquipmentDto.Name.Equals(string.Empty))
                throw new ArgumentException($"{nameof(registerEquipmentDto.Name)} is missing in the request");
            if(registerEquipmentDto.Location.Equals(string.Empty))
                throw new ArgumentException($"{nameof(registerEquipmentDto.Location)} is missing in the request");
        }

        private static void ValidateAssignEquipmentOperatorDto(AssignEquipmentOperatorDto assignEquipmentOperatorDto)
        {
            if (assignEquipmentOperatorDto is null)
                throw new ArgumentException("The requestBody is missing");
            if (assignEquipmentOperatorDto.Employee.Equals(string.Empty))
                throw new ArgumentException($"{nameof(assignEquipmentOperatorDto.Employee)} is missing in the request");
        }
    }
}
