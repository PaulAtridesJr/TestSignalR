using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace xemtest
{
    public class SimpleWorker : IWorker
    {
        private readonly ILogger<SimpleWorker> logger;

        public SimpleWorker(ILogger<SimpleWorker> logger) {
            this.logger = logger;
        }


        public ExecuteResult Execute()
        {
            ExecuteResult result = new();

            logger.LogInformation("Start execute");

            IntPtr hGlobal = Marshal.AllocHGlobal(10);
            IntPtr initial = hGlobal;
            byte[] data = new byte [] { 0x01, 0x03, 0x05, 0x07, 0x09, 0x08, 0x06, 0x04, 0x02, 0x00 };
            foreach(byte bt in data) {
                Marshal.WriteByte(hGlobal, bt);
                hGlobal++;
            }
            List<byte> test = new();
            for(int i = 0; i<data.Length;i++) {
                test.Add(Marshal.ReadByte(initial, i));
            }

            for(int i=0; i < data.Length; i++) 
            {
                if(data[i] != test[i]) {
                    throw new Exception("Wrong data");
                }
            }

            result.Address = initial;

            logger.LogInformation("Execute complete");

            return result;
        }

        public ExecuteResult ExecuteWithParams(ExecuteData data) 
        {
            ExecuteResult result = new();

            logger.LogInformation("Start execute");

            result.Address = new IntPtr(new Random(DateTime.Now.Millisecond).Next(10000000));

            logger.LogInformation("Execute complete");

            return result;
        }
    }
}