using AwesomeAssertions;
using Halo.Api.Models.Tickets;

namespace Halo.Api.Test.Models.Tickets;

public class TicketModelTests
{
	[Fact]
	public void Ticket_WithRequiredProperties_CanBeCreated()
	{
		// Arrange & Act
		var ticket = new Ticket
		{
			Id = 123,
			Summary = "Test ticket",
			Status = 1,
			ClientId = 1,
			UserId = 1
		};

		// Assert
		ticket.Id.Should().Be(123);
		ticket.Summary.Should().Be("Test ticket");
		ticket.Status.Should().Be(1);
		ticket.ClientId.Should().Be(1);
		ticket.UserId.Should().Be(1);
		ticket.Priority.Should().Be(1); // Default value
		ticket.DateOccurred.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
		ticket.IsClosed.Should().BeFalse(); // No DateClosed set, so should be false
		ticket.IsOnHold.Should().BeFalse(); // Default value
	}

	[Fact]
	public void Ticket_WithAllProperties_CanBeCreated()
	{
		// Arrange
		var now = DateTime.UtcNow;
		var customFields = new Dictionary<string, object?> { ["CustomField1"] = "Value1" };
		var assetIds = new List<int> { 1, 2, 3 }.AsReadOnly();
		var tags = new List<string> { "urgent", "hardware" }.AsReadOnly();

		// Act
		var ticket = new Ticket
		{
			Id = 456,
			Summary = "Comprehensive test ticket",
			Details = "Detailed description",
			Status = 2,
			StatusName = "In Progress",
			Priority = 3,
			PriorityName = "High",
			ClientId = 5,
			ClientName = "Test Client",
			SiteId = 10,
			SiteName = "Main Office",
			UserId = 15,
			UserName = "John Doe",
			UserEmail = "john.doe@test.com",
			AgentId = 20,
			AgentName = "Jane Smith",
			TeamId = 25,
			TeamName = "IT Support",
			CategoryId = 30,
			CategoryName = "Hardware",
			TicketTypeId = 35,
			TicketTypeName = "Incident",
			DateOccurred = now,
			LastUpdate = now.AddHours(1),
			DateClosed = now.AddHours(2), // This will make IsClosed = true
			Source = 1,
			IsOnHold = false,
			CustomFields = customFields,
			AssetIds = assetIds,
			Tags = tags
		};

		// Assert
		ticket.Id.Should().Be(456);
		ticket.Summary.Should().Be("Comprehensive test ticket");
		ticket.Details.Should().Be("Detailed description");
		ticket.Status.Should().Be(2);
		ticket.StatusName.Should().Be("In Progress");
		ticket.Priority.Should().Be(3);
		ticket.PriorityName.Should().Be("High");
		ticket.ClientId.Should().Be(5);
		ticket.ClientName.Should().Be("Test Client");
		ticket.SiteId.Should().Be(10);
		ticket.SiteName.Should().Be("Main Office");
		ticket.UserId.Should().Be(15);
		ticket.UserName.Should().Be("John Doe");
		ticket.UserEmail.Should().Be("john.doe@test.com");
		ticket.AgentId.Should().Be(20);
		ticket.AgentName.Should().Be("Jane Smith");
		ticket.TeamId.Should().Be(25);
		ticket.TeamName.Should().Be("IT Support");
		ticket.CategoryId.Should().Be(30);
		ticket.CategoryName.Should().Be("Hardware");
		ticket.TicketTypeId.Should().Be(35);
		ticket.TicketTypeName.Should().Be("Incident");
		ticket.DateOccurred.Should().Be(now);
		ticket.LastUpdate.Should().Be(now.AddHours(1));
		ticket.DateClosed.Should().Be(now.AddHours(2));
		ticket.Source.Should().Be(1);
		ticket.IsClosed.Should().BeTrue(); // Computed from DateClosed having a value
		ticket.IsOnHold.Should().BeFalse();
		ticket.CustomFields.Should().BeEquivalentTo(customFields);
		ticket.AssetIds.Should().BeEquivalentTo(assetIds);
		ticket.Tags.Should().BeEquivalentTo(tags);
	}

	[Fact]
	public void Ticket_IsClosed_ComputedCorrectly()
	{
		// Test when DateClosed is null
		var openTicket = new Ticket
		{
			Id = 1,
			Summary = "Open ticket", 
			Status = 1,
			ClientId = 1,
			UserId = 1,
			DateClosed = null
		};

		openTicket.IsClosed.Should().BeFalse();

		// Test when DateClosed has a value
		var closedTicket = new Ticket
		{
			Id = 2,
			Summary = "Closed ticket",
			Status = 2, 
			ClientId = 1,
			UserId = 1,
			DateClosed = DateTime.UtcNow
		};

		closedTicket.IsClosed.Should().BeTrue();
	}

	[Fact]
	public void CreateTicketRequest_WithRequiredProperties_CanBeCreated()
	{
		// Arrange & Act
		var request = new CreateTicketRequest
		{
			Summary = "New ticket",
			ClientId = 1,
			UserId = 1
		};

		// Assert
		request.Summary.Should().Be("New ticket");
		request.ClientId.Should().Be(1);
		request.UserId.Should().Be(1);
		request.Details.Should().BeNull();
		request.Status.Should().BeNull();
		request.Priority.Should().BeNull();
	}

	[Fact]
	public void CreateTicketRequest_WithAllProperties_CanBeCreated()
	{
		// Arrange
		var now = DateTime.UtcNow;
		var customFields = new Dictionary<string, object?> { ["Field1"] = "Value1" };
		var assetIds = new List<int> { 1, 2 }.AsReadOnly();
		var tags = new List<string> { "tag1", "tag2" }.AsReadOnly();

		// Act
		var request = new CreateTicketRequest
		{
			Summary = "Detailed new ticket",
			Details = "Full description",
			Status = 1,
			Priority = 2,
			ClientId = 3,
			SiteId = 4,
			UserId = 5,
			AgentId = 6,
			TeamId = 7,
			CategoryId = 8,
			TicketTypeId = 9,
			DateOccurred = now,
			Source = 1,
			CustomFields = customFields,
			AssetIds = assetIds,
			Tags = tags,
			SuppressEmails = true,
			Notes = "Initial notes"
		};

		// Assert
		request.Summary.Should().Be("Detailed new ticket");
		request.Details.Should().Be("Full description");
		request.Status.Should().Be(1);
		request.Priority.Should().Be(2);
		request.ClientId.Should().Be(3);
		request.SiteId.Should().Be(4);
		request.UserId.Should().Be(5);
		request.AgentId.Should().Be(6);
		request.TeamId.Should().Be(7);
		request.CategoryId.Should().Be(8);
		request.TicketTypeId.Should().Be(9);
		request.DateOccurred.Should().Be(now);
		request.Source.Should().Be(1);
		request.CustomFields.Should().BeEquivalentTo(customFields);
		request.AssetIds.Should().BeEquivalentTo(assetIds);
		request.Tags.Should().BeEquivalentTo(tags);
		request.SuppressEmails.Should().BeTrue();
		request.Notes.Should().Be("Initial notes");
	}

	[Fact]
	public void UpdateTicketRequest_WithPartialUpdate_CanBeCreated()
	{
		// Arrange & Act
		var request = new UpdateTicketRequest
		{
			Summary = "Updated summary",
			Priority = 3,
			AgentId = 10
		};

		// Assert
		request.Summary.Should().Be("Updated summary");
		request.Priority.Should().Be(3);
		request.AgentId.Should().Be(10);
		request.Details.Should().BeNull();
		request.Status.Should().BeNull();
		request.ClientId.Should().BeNull();
	}

	[Fact]
	public void UpdateTicketRequest_WithCloseTicket_CanBeCreated()
	{
		// Arrange & Act
		var request = new UpdateTicketRequest
		{
			CloseTicket = true,
			Resolution = "Issue resolved successfully",
			Notes = "Closing notes"
		};

		// Assert
		request.CloseTicket.Should().BeTrue();
		request.Resolution.Should().Be("Issue resolved successfully");
		request.Notes.Should().Be("Closing notes");
	}

	[Fact]
	public void TicketFilter_WithDefaultValues_HasExpectedDefaults()
	{
		// Arrange & Act
		var filter = new TicketFilter();

		// Assert
		filter.Count.Should().BeNull();
		filter.PageNo.Should().BeNull();
		filter.PageSize.Should().BeNull();
		filter.Paginate.Should().BeNull();
		filter.Status.Should().BeNull();
		filter.Search.Should().BeNull();
		filter.OpenOnly.Should().BeNull();
		filter.ClosedOnly.Should().BeNull();
	}

	[Fact]
	public void TicketFilter_WithAllProperties_CanBeCreated()
	{
		// Arrange
		var startDate = DateTime.UtcNow.AddDays(-7);
		var endDate = DateTime.UtcNow;

		// Act
		var filter = new TicketFilter
		{
			Count = 50,
			PageNo = 1,
			PageSize = 25,
			Paginate = true,
			Status = "1,2,3",
			Priority = "2,3",
			ClientId = 5,
			SiteId = 10,
			UserId = 15,
			AgentId = 20,
			TeamId = 25,
			CategoryId = 30,
			TicketTypeId = 35,
			Search = "test search",
			StartDate = startDate,
			EndDate = endDate,
			OpenOnly = true,
			MyTickets = true,
			IncludeDetails = true,
			Order = "dateoccurred",
			OrderDesc = true,
			AssetId = 100,
			ServiceId = 200,
			IncludeCustomFields = "1,2,3"
		};

		// Assert
		filter.Count.Should().Be(50);
		filter.PageNo.Should().Be(1);
		filter.PageSize.Should().Be(25);
		filter.Paginate.Should().BeTrue();
		filter.Status.Should().Be("1,2,3");
		filter.Priority.Should().Be("2,3");
		filter.ClientId.Should().Be(5);
		filter.SiteId.Should().Be(10);
		filter.UserId.Should().Be(15);
		filter.AgentId.Should().Be(20);
		filter.TeamId.Should().Be(25);
		filter.CategoryId.Should().Be(30);
		filter.TicketTypeId.Should().Be(35);
		filter.Search.Should().Be("test search");
		filter.StartDate.Should().Be(startDate);
		filter.EndDate.Should().Be(endDate);
		filter.OpenOnly.Should().BeTrue();
		filter.MyTickets.Should().BeTrue();
		filter.IncludeDetails.Should().BeTrue();
		filter.Order.Should().Be("dateoccurred");
		filter.OrderDesc.Should().BeTrue();
		filter.AssetId.Should().Be(100);
		filter.ServiceId.Should().Be(200);
		filter.IncludeCustomFields.Should().Be("1,2,3");
	}

	[Fact]
	public void TicketsResponse_WithEmptyCollection_CanBeCreated()
	{
		// Arrange & Act
		var response = new TicketsResponse
		{
			Tickets = new List<Ticket>().AsReadOnly(),
			RecordCount = 0
		};

		// Assert
		response.Tickets.Should().BeEmpty();
		response.RecordCount.Should().Be(0);
		response.HasMore.Should().BeFalse();
		response.IsPaginated.Should().BeFalse();
	}

	[Fact]
	public void TicketsResponse_WithPagination_CanBeCreated()
	{
		// Arrange
		var tickets = new List<Ticket>
		{
			new() { Id = 1, Summary = "Ticket 1", Status = 1, ClientId = 1, UserId = 1 },
			new() { Id = 2, Summary = "Ticket 2", Status = 1, ClientId = 1, UserId = 1 }
		}.AsReadOnly();

		// Act
		var response = new TicketsResponse
		{
			Tickets = tickets,
			RecordCount = 100,
			PageNo = 1,
			PageSize = 2,
			PageCount = 50,
			HasMore = true,
			IsPaginated = true
		};

		// Assert
		response.Tickets.Should().HaveCount(2);
		response.RecordCount.Should().Be(100);
		response.PageNo.Should().Be(1);
		response.PageSize.Should().Be(2);
		response.PageCount.Should().Be(50);
		response.HasMore.Should().BeTrue();
		response.IsPaginated.Should().BeTrue();
	}

	[Fact]
	public void TicketResponse_WithSuccess_CanBeCreated()
	{
		// Arrange
		var ticket = new Ticket
		{
			Id = 1,
			Summary = "Test ticket",
			Status = 1,
			ClientId = 1,
			UserId = 1
		};
		var messages = new List<string> { "Operation completed successfully" }.AsReadOnly();

		// Act
		var response = new TicketResponse
		{
			Ticket = ticket,
			Success = true,
			Messages = messages
		};

		// Assert
		response.Ticket.Should().Be(ticket);
		response.Success.Should().BeTrue();
		response.Messages.Should().ContainSingle("Operation completed successfully");
	}

	[Fact]
	public void CreateTicketResponse_ContainsTicketId()
	{
		// Arrange
		var ticket = new Ticket
		{
			Id = 123,
			Summary = "New ticket",
			Status = 1,
			ClientId = 1,
			UserId = 1
		};

		// Act
		var response = new CreateTicketResponse
		{
			Ticket = ticket,
			Success = true
		};

		// Assert
		response.TicketId.Should().Be(123);
		response.Ticket.Id.Should().Be(123);
		response.Success.Should().BeTrue();
	}

	[Fact]
	public void UpdateTicketResponse_ContainsTicketId()
	{
		// Arrange
		var ticket = new Ticket
		{
			Id = 456,
			Summary = "Updated ticket",
			Status = 2,
			ClientId = 1,
			UserId = 1
		};
		var messages = new List<string> { "Ticket updated successfully" }.AsReadOnly();

		// Act
		var response = new UpdateTicketResponse
		{
			Ticket = ticket,
			Success = true,
			Messages = messages
		};

		// Assert
		response.TicketId.Should().Be(456);
		response.Ticket.Id.Should().Be(456);
		response.Success.Should().BeTrue();
		response.Messages.Should().ContainSingle("Ticket updated successfully");
	}
}