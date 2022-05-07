using System.Collections.Generic;

namespace Mahzan.Mobile.Models.PointSaleState
{
    public class GetPointSaleStateResponse
    {
        public List<PointSaleStateResponse> Data { get; set; }
    }

    public class PointSaleStateResponse
    {
        public string StoreName { get; set; }
    
        public string PointSaleName { get; set; }
    
        public string UserName { get; set; }
    
        public PointSaleState PointSaleState { get; set; }

        public PointSaleCoins.PointSaleCoins Coins { get; set; }
    
        public PointSaleBills.PointSaleBills Bills { get; set; }
    }
}