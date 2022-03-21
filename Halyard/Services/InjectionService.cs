using Halyard.Extensions;
using Halyard.Properties;
using Halyard.Services.Interfaces;
using Halyard.Statics;
using Halyard.Structs;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Halyard.Services
{
    /// <inheritdoc cref="IInjectionService"/>
    sealed class InjectionService : IInjectionService
    {
        /// <inheritdoc />
        public async Task<bool> InjectAsync(
            int pid,
            Entry entry)
        {
            bool injected = false;
            try
            {
                IntPtr procHandle = WinApi.OpenProcess(
                    WinApi.PROCESS_CREATE_THREAD |
                    WinApi.PROCESS_QUERY_INFORMATION |
                    WinApi.PROCESS_VM_OPERATION |
                    WinApi.PROCESS_VM_WRITE |
                    WinApi.PROCESS_VM_READ,
                    false,
                    pid);
                var bootstrapper = Path.GetFullPath(".\\bootstrapper.dll");
                var nethost = Path.GetFullPath(".\\nethost.dll");
                await bootstrapper.CheckFileAsync(Resources.bootstrapper, exact: true);
                await nethost.CheckFileAsync(Resources.nethost, exact: true);
                await RemoteLoadLibrary(procHandle, "kernel32.dll", "LoadLibraryW", nethost);
                await RemoteLoadLibrary(procHandle, "kernel32.dll", "LoadLibraryW", bootstrapper);
                await RemoteLoadLibrary(procHandle, bootstrapper, "Launch", JsonConvert.SerializeObject(entry));
                injected = true;
                Log.Information("injection succeeded");
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
            }
            return injected;
        }

        /// <inheritdoc />
        public Task<bool> EjectAsync()
        {
            bool ejected = false;
            try
            {
                ejected = true;
                Log.Information("ejection succeeded");
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
            }
            return Task.FromResult(ejected);
        }

        Task RemoteLoadLibrary<T>(IntPtr procHandle, string dllPath, string function, T parameter)
        {
            byte[] bytes;
            uint allocSize;
            var result = (byte[])typeof(BitConverter).GetMethod("GetBytes", new[] { typeof(T) })
                .Invoke(null, new[] { parameter as object });
            bytes = result;
            allocSize = (uint)result.Length;
            var llHandle = WinApi.LoadLibrary(dllPath);
            IntPtr functionPtr = WinApi.GetProcAddress(WinApi.GetModuleHandle(Path.GetFileName(dllPath)), function);
            IntPtr allocMemAddress = WinApi.VirtualAllocEx(procHandle, IntPtr.Zero, allocSize, WinApi.MEM_COMMIT | WinApi.MEM_RESERVE, WinApi.PAGE_READWRITE);
            WinApi.WriteProcessMemory(procHandle, allocMemAddress, bytes, allocSize, out var bytesWritten);
            Log.Information(bytesWritten.ToString());
            var threadHandle = WinApi.CreateRemoteThread(procHandle, IntPtr.Zero, 0, functionPtr, allocMemAddress, 0, IntPtr.Zero);
            WinApi.WaitForSingleObject(threadHandle, uint.MaxValue);
            WinApi.VirtualFreeEx(procHandle, allocMemAddress, allocSize, WinApi.AllocationType.Release);
            WinApi.FreeLibrary(llHandle);
            return Task.CompletedTask;
        }

        Task RemoteLoadLibrary(IntPtr procHandle, string dllPath, string function, string parameter)
        {
            byte[] bytes;
            uint allocSize;
            var strParam = parameter.ToString();
            bytes = Encoding.Unicode.GetBytes(strParam);
            allocSize = (uint)((strParam.Length + 1) * 2);
            var llHandle = WinApi.LoadLibrary(dllPath);
            IntPtr functionPtr = WinApi.GetProcAddress(WinApi.GetModuleHandle(Path.GetFileName(dllPath)), function);
            IntPtr allocMemAddress = WinApi.VirtualAllocEx(procHandle, IntPtr.Zero, allocSize, WinApi.MEM_COMMIT | WinApi.MEM_RESERVE, WinApi.PAGE_READWRITE);
            WinApi.WriteProcessMemory(procHandle, allocMemAddress, bytes, allocSize, out var bytesWritten);
            Log.Information(bytesWritten.ToString());
            var threadHandle = WinApi.CreateRemoteThread(procHandle, IntPtr.Zero, 0, functionPtr, allocMemAddress, 0, IntPtr.Zero);
            WinApi.WaitForSingleObject(threadHandle, uint.MaxValue);
            WinApi.VirtualFreeEx(procHandle, allocMemAddress, allocSize, WinApi.AllocationType.Release);
            WinApi.FreeLibrary(llHandle);
            return Task.CompletedTask;
        }

        Task RemoteLoadLibrary(IntPtr procHandle, string dllPath, string function, Entry parameter)
        {
            uint allocSize = (uint)Marshal.SizeOf(parameter);
            byte[] bytes = new byte[allocSize];
            IntPtr ptr = Marshal.AllocHGlobal((int)allocSize);
            Marshal.StructureToPtr(parameter, ptr, false);
            Marshal.Copy(ptr, bytes, 0, (int)allocSize);
            Marshal.FreeHGlobal(ptr);
            var llHandle = WinApi.LoadLibrary(dllPath);
            IntPtr functionPtr = WinApi.GetProcAddress(WinApi.GetModuleHandle(Path.GetFileName(dllPath)), function);
            IntPtr allocMemAddress = WinApi.VirtualAllocEx(procHandle, IntPtr.Zero, allocSize, WinApi.MEM_COMMIT | WinApi.MEM_RESERVE, WinApi.PAGE_READWRITE);
            WinApi.WriteProcessMemory(procHandle, allocMemAddress, bytes, allocSize, out var bytesWritten);
            Log.Information(bytesWritten.ToString());
            var threadHandle = WinApi.CreateRemoteThread(procHandle, IntPtr.Zero, 0, functionPtr, allocMemAddress, 0, IntPtr.Zero);
            WinApi.WaitForSingleObject(threadHandle, uint.MaxValue);
            WinApi.VirtualFreeEx(procHandle, allocMemAddress, allocSize, WinApi.AllocationType.Release);
            WinApi.FreeLibrary(llHandle);
            return Task.CompletedTask;
        }
    }
}
