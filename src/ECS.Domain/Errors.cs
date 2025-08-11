namespace ECS.Domain;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) {}
}

public class ToolNotAvailableException : DomainException
{
    public ToolNotAvailableException() : base("Tool is not available for checkout.") {}
}

public class NotCustodianException : DomainException
{
    public NotCustodianException() : base("Employee is not the current custodian.") {}
}

public class UnknownBarcodeException : DomainException
{
    public UnknownBarcodeException() : base("Unknown barcode.") {}
}
