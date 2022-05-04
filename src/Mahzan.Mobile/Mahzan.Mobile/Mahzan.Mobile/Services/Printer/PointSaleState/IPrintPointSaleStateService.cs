using System;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.PointSaleState;

namespace Mahzan.Mobile.Services.Printer.PointSaleState
{
    public interface IPrintPointSaleStateService
    {
        Task PrintOpenPointSaleState(GetPointSaleStateResponse getPointSaleStateResponse);
    }
}