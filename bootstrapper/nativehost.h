#pragma once

class nativehost
{
public:
    int main(
        LPCWSTR dir,
        LPCWSTR config,
        LPCWSTR lib,
        LPCWSTR type,
        LPCWSTR method,
        LPCWSTR del);
	typedef void entrypoint();
};
