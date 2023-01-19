namespace MyStore.Common.Application.Models
{
    using RExtension;
    using System.Collections.Generic;

    public class OrderModel
    {
        public OrderModel(string column)
        {
            Column = column;
        }

        public OrderModel(string column, string order)
            : this(column)
        {
            Order = order;
        }

        public string Column { get; set; }

        public string Order { get; set; } = "asc";

        public static IList<OrderModel>? GetOrderModels(string sc, string? so = null)
        {
            if (sc.IsEmpty())
            {
                return null;
            }
            var list = new List<OrderModel>();
            var om = new OrderModel(sc);
            if (so.IsNotEmpty())
            {
                om.Order = so ?? "asc";
            }
            list.Add(om);
            return list;
        }
    }
}
