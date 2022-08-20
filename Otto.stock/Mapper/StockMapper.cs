using Otto.stock.DTO;
using Otto.stock.Models;

namespace Otto.stock.Mapper
{
    public static class StockMapper
    {
        public static Stock GetStock(StockDTO dto)
        {
            var capitalizedState = "";
            if (!string.IsNullOrEmpty(dto.State)) 
            {
                capitalizedState = dto.State.ToUpper()[0] + dto.State.ToLower().Substring(1);
            }

            Enum.TryParse(capitalizedState, out State state);
            return new Stock
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Origin = dto.Origin,
                Quantity = dto.Quantity,
                SellerId = dto.SellerId,
                SellerIdMail = dto.SellerIdMail,
                MSellerId = dto.MSellerId,
                TSellerId = dto.TSellerId,
                MItemId = dto.MItemId,
                TItemId = dto.TItemId,
                SKU = dto.SKU,
                Code = dto.Code,
                Category = dto.Category,
                StateDescription = dto.StateDescription,
                State = state
            };
        }


        public static StockDTO GetStockDTO(Stock stock)
        {
            return new StockDTO
            {
                Id = stock.Id,
                Name = stock.Name,
                Description = stock.Description,
                Origin = stock.Origin,
                Quantity = stock.Quantity,
                SellerId = stock.SellerId,
                SellerIdMail = stock.SellerIdMail,
                MSellerId = stock.MSellerId,
                TSellerId = stock.TSellerId,
                MItemId = stock.MItemId,
                TItemId = stock.TItemId,
                SKU = stock.SKU,
                Code = stock.Code,
                Category = stock.Category,
                StateDescription = stock.StateDescription,
                State = stock.State.ToString()
            };
        }

        public static List<StockDTO> GetStockDTOs(List<Stock> stock) 
        {
            var result = new List<StockDTO>();
            foreach (var item in stock)
            {
                result.Add(GetStockDTO(item));
            }
            return result;
        }


    }
}
