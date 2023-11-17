namespace Domain.Exceptions;

public class ProductNotInLineItemException : Exception
{
    public ProductNotInLineItemException(Guid lineItemId, Guid productId)
        :base($"The LineItem with the ID = {lineItemId} and the product with the ID = {productId} are not match!")
    {
    }
}