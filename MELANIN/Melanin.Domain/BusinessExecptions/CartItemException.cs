namespace Melanin.Domain.BusinessExecptions;

public class CartItemException : Exception
{
    public CartItemException(string? message) : base(message) { }
}

public class CartItemNotFoundException : CartItemException
{
    public CartItemNotFoundException() : base("Article introuvable dans le panier") { }
    public CartItemNotFoundException(int id) : base($"Article avec l'id {id} introuvable dans le panier") { }
}