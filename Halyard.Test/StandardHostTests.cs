using Halyard.Interfaces;
using Halyard.Structs;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Halyard.Test
{
    [TestFixture]
    public class StandardHostTests
    {
        readonly IHost _host;

        public StandardHostTests()
        {
            _host = new StandardHost();
        }

        #region Injection

        [Test]
        public async Task InjectAsync_Invoked_Always_InjectsLibrary()
        {
            var processes = Process.GetProcesses();
            var notepad = processes.FirstOrDefault(x => x.ProcessName.ToLower() == "wow");
            if (notepad == null)
                Assert.Fail("open notepad");
            var entry = new Entry()
            {
                config = "Halyard.Test.runtimeconfig.json",
                lib = "Halyard.Test.dll",
                type = "Halyard.Test.EntryClassTest, Halyard.Test",
                method = "EntryMethodTest",
                del = "Halyard.Test.EntryClassTest+EntryDelegateTest, Halyard.Test"
            };
            Assert.IsTrue(await _host.InjectAsync(
                pid: notepad.Id,
                entry: entry));
        }

        [Test]
        public async Task EjectAsync_Invoked_Always_EjectsLibrary()
        {
            Assert.IsTrue(await _host.EjectAsync());
        }

        [Test]
        public async Task GetInjectionServiceAsync_Invoked_Always_GetsInjectionService()
        {
            var injectionService = await _host.GetInjectionServiceAsync();
            Assert.NotNull(injectionService);
        }

        #endregion
    }
}
