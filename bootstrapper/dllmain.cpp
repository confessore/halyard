#include "pch.h"
#include "nativehost.h"
#include <cstdio>
#include <cstdio>
#include <cstdlib>
#include <iostream>
#include <filesystem>
#include <pathcch.h>
#include <thread>
#include "Winuser.h"
#include "jsonw.h"

using namespace std;

bool injected = false;
HMODULE handle;

typedef struct _entry {
    wchar_t const* config;
    wchar_t const* lib;
    wchar_t const* type;
    wchar_t const* method;
    wchar_t const* del;
} entry;

wchar_t const* strtowstr(char const* str) {
    size_t const strSize = strlen(str) + 1;
    wchar_t* wstr = new wchar_t[strSize];
    size_t wstrSize;
    mbstowcs_s(&wstrSize, wstr, strSize, str, _TRUNCATE);
    wchar_t const* cwstr = wstr;
    return cwstr;
}

extern "C" __declspec(dllexport) void Launch(wchar_t const* parameter) {
    if (injected) return;
    JsonW json(parameter);
    vector<string> keys;
    json.keys(keys);
    entry entry;
    for (size_t i = 0; i < keys.size(); i++) {
        string key = keys.at(i);
        string value = json[key].str();
        char const* strvalue = value.c_str();
        wchar_t const* wstrvalue = strtowstr(strvalue);
        if (key == "config")
            entry.config = wstrvalue;
        if (key == "lib")
            entry.lib = wstrvalue;
        if (key == "type")
            entry.type = wstrvalue;
        if (key == "method")
            entry.method = wstrvalue;
        if (key == "del")
            entry.del = wstrvalue;
    }
    wchar_t path[MAX_PATH];
    if (GetModuleFileNameW(handle, path, MAX_PATH) == 0)
    {
        int ret = GetLastError();
        fprintf(stderr, "GetModuleFileName failed, error = %d\n", ret);
    }
    nativehost host;
    host.main(
        path,
        entry.config,
        entry.lib,
        entry.type,
        entry.method,
        entry.del);
    injected = true;
}

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    {
        handle = hModule;
    }
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    default:;
    }
    return TRUE;
}
