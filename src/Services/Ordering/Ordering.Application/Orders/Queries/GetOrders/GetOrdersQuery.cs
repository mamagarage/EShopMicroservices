using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;


// what is it? 
// This is a record type that represents the result of the GetOrdersQuery.
// It contains a single property, Orders, which is a PaginatedResult of OrderDto objects.
// The PaginatedResult class likely contains information about the pagination, such as the current page index, page size,
// total count of items, and the list of items for the current page. 
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);