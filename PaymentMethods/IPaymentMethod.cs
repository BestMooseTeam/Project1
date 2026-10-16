namespace PaymentMethods
{
    /// <summary>Способ оплаты заказа в интернет-магазине.</summary>
    public interface IPaymentMethod
    {
        /// <summary>Человекочитаемое название способа оплаты.</summary>
        string Name { get; }

        /// <summary>Имитирует проведение платежа на указанную сумму.</summary>
        void Pay(decimal amount);
    }
}
