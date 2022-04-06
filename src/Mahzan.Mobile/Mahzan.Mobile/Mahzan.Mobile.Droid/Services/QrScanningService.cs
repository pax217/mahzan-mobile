using System.Threading.Tasks;
using Mahzan.Mobile.QrScanning;
using ZXing.Mobile;

[assembly: Xamarin.Forms.Dependency(typeof(Mahzan.Mobile.Droid.Services.QrScanningService))]
namespace Mahzan.Mobile.Droid.Services
{
    public class QrScanningService: IQrScanningService
    {
        public async Task<string> ScanAsync()
        {

            var optionsCustom = new MobileBarcodeScanningOptions() { };

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Acerca la camara al elemento",
                BottomText = "Toca la pantalla para enfocar",
            };

            var scanResults = await scanner.Scan();

            return scanResults == null ? string.Empty : scanResults.Text;

        }
    }
}