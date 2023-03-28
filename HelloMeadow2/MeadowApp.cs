using System;
using Meadow;
using System.Linq;
using Meadow.Devices;
using Meadow.Foundation;
using System.Threading.Tasks;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;

namespace HelloMeadow2
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        public override async Task Run()
        {
            Console.WriteLine("Run...");

            await CycleColors(TimeSpan.FromMilliseconds(2000));
            await base.Run();
        }
        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            return base.Initialize();
        }
        private async Task CycleColors(TimeSpan duration)
        {
            Console.WriteLine("Cycle colors...");
            var colors = typeof(Color).GetFields().Where(p => p.DeclaringType == typeof(Color));//.Where(p => p.DeclaringType == typeof(Color)).ToList();
            var index = 0;

            while (true)
            {
                index = 0;
                foreach (var color in colors)
                {
                    index++;
                    await Console.Out.WriteLineAsync($"Showing {index}: {color.Name}");
                    await ShowColorPulse((Color)color.GetValue(null), duration);
                }
            }
        }
        private async Task ShowColorPulse(Color color, TimeSpan duration)
        {
            onboardLed.StartPulse(color, duration / 2);
            await Task.Delay(duration);
            onboardLed.Stop();
        }
    }
}