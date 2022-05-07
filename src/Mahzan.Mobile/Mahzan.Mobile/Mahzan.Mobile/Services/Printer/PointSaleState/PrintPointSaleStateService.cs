using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.PointSaleState;
using Mahzan.Mobile.Services.BlueTooth;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Xamarin.Forms;

namespace Mahzan.Mobile.Services.Printer.PointSaleState
{
    public class PrintPointSaleStateService: IPrintPointSaleStateService
    {
        private readonly IRepository<BluetoothDevice> _bluetoothDeviceRepository;
        private readonly IBlueToothService _blueToothService;
        
        private Printer _printer;
        private List<BluetoothDevice> _bluetoothDevices;

        public PrintPointSaleStateService(
            IRepository<BluetoothDevice> bluetoothDeviceRepository)
        {
            _bluetoothDeviceRepository = bluetoothDeviceRepository;
            _blueToothService = Xamarin.Forms.DependencyService.Get<IBlueToothService>();;

            Task.Run(InitializePrinter);
        }

        private async Task InitializePrinter()
        {
            _bluetoothDevices = await _bluetoothDeviceRepository.Get();
        }

        public async Task PrintOpenPointSaleState(GetPointSaleStateResponse getPointSaleStateResponse)
        {
            await ValidateSelectedPrint();
            
            await Print(getPointSaleStateResponse);
        }

        private async Task ValidateSelectedPrint()
        {
            if (!_bluetoothDevices.Any())
            {
                throw new Exception("No existe una impresora asignada, ve al menú Configuracion/Impresora");
            }
        }

        private async Task Print(GetPointSaleStateResponse getPointSaleStateResponse)
        { 
            _printer = new Printer(_blueToothService);
            _printer.MyPrinter = "MTP-2";
            
            var pointSaleState = getPointSaleStateResponse.Data.FirstOrDefault();
            
            await _printer.Reset();
            await _printer.SetAlignCenter();
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("INFORME DE PUNTO DE VENTA");
            await _printer.WriteLine("-------------------------------");
            await _printer.SetAlignLeft();
            await _printer.WriteLine("TIENDA:" + pointSaleState.StoreName);
            await _printer.WriteLine("TPV:" + pointSaleState.PointSaleName);
            await _printer.WriteLine("USR:" + pointSaleState.UserName);
            await _printer.WriteLine("ESTADO:" + await ConvertState(pointSaleState.PointSaleState.State));
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("************MONEDAS************");
            await _printer.WriteLine("-------------------------------");
            await _printer.SetAlignRight();
            await _printer.WriteLine(pointSaleState.Coins.TenCents + " de $    .10");
            await _printer.WriteLine(pointSaleState.Coins.TwentyCents + " de $    .20");
            await _printer.WriteLine(pointSaleState.Coins.FiftyCents + " de $    .50");
            await _printer.WriteLine(pointSaleState.Coins.One + " de $   1.00");
            await _printer.WriteLine(pointSaleState.Coins.Two + " de $   2.00");
            await _printer.WriteLine(pointSaleState.Coins.Five + " de $   5.00");
            await _printer.WriteLine(pointSaleState.Coins.Ten + " de $  10.00");
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("Monto en monedas $ " + pointSaleState.PointSaleState.AmountCoins);
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("************BILLETES***********");
            await _printer.WriteLine("-------------------------------");
            await _printer.SetAlignRight();
            await _printer.WriteLine(pointSaleState.Bills.Twenty + " de $  10.00");
            await _printer.WriteLine(pointSaleState.Bills.Fifty + " de $  50.00");
            await _printer.WriteLine(pointSaleState.Bills.Hundred + " de $ 100.00");
            await _printer.WriteLine(pointSaleState.Bills.TwoHundred + " de $ 200.00");
            await _printer.WriteLine(pointSaleState.Bills.FiveHundred + " de $ 500.00");
            await _printer.WriteLine(pointSaleState.Bills.OneThousand + " de $1000.00");
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("Monto en billetes $ " + pointSaleState.PointSaleState.AmountBills);
            await _printer.WriteLine("-------------------------------");
            await _printer.WriteLine("TOTAL $ " + 
                                     (pointSaleState.PointSaleState.AmountBills 
                                      + pointSaleState.PointSaleState.AmountCoins)
                                     );
            await _printer.Reset();
        }

        private async Task<string> ConvertState(string state)
        {
            string result = string.Empty;

            switch (@state)
            {
                case "opened":
                    result = "ABIERTO";
                    break;;
            }
            
            return result;
        }
    }
}