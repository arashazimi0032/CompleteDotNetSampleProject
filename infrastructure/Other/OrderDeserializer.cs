using Domain.Customers.ValueObjects;
using Domain.Orders.Entities;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace infrastructure.Other;

public class OrderDeserializer : JsonConverter<Order>
{
    public override void WriteJson(JsonWriter writer, Order value, JsonSerializer serializer)
    {
        // Serialize the Order object as usual
        serializer.Serialize(writer, value);
    }

    public override Order ReadJson(JsonReader reader, Type objectType, Order existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Read the JSON object as usual
        var jsonObject = JObject.Load(reader);

        // Deserialize the properties of the Order object
        if (!Guid.TryParse(jsonObject["Id"]["Value"].Value<string>(), out var id))
            throw new Exception("Wrong Id format for Deserialization");

        if (!Guid.TryParse(jsonObject["CustomerId"]["Value"].Value<string>(), out var customerId))
            throw new Exception("Wrong CustomerId format for Deserialization");

        // Deserialize the list of LineItem objects
        var lineItems = jsonObject["LineItems"].ToObject<List<LineItem>>(serializer);

        // Create a new Order object with the deserialized properties
        var order = Order.Create(OrderId.Create(id), CustomerId.Create(customerId));
        order.Update(lineItems!);

        return order;
    }
}