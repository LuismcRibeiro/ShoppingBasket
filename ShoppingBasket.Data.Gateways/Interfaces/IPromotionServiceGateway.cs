using ShoppingList.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Data.Gateways.Interfaces
{
    public interface IPromotionServiceGateway
    {
        IEnumerable<Promotion> GetActivePromotions();
    }
}
